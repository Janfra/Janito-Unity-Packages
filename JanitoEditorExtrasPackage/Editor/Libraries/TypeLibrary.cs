using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace Janito.EditorExtras.Editor
{
    public static class TypeLibrary
    {
        [Obsolete("GetEnumerableOfTypeChildren is deprecated, please use GetChildTypes instead.")]
        public static IEnumerable<Type> GetEnumerableOfTypeChildren<T>(TypeCriteria elegibilityCriteria = new(), bool sortedByName = false) where T : class
        {
            Type requestedType = typeof(T);
            return GetEnumerableOfTypeChildren(requestedType, elegibilityCriteria, sortedByName);
        }

        [Obsolete("GetEnumerableOfTypeChildren is deprecated, please use GetChildTypes instead.")]
        public static IEnumerable<Type> GetEnumerableOfTypeChildren(Type requestedType, TypeCriteria elegibilityCriteria = new(), bool sortedByName = false)
        {
            List<Type> childrenTypes = new();
            Type[] types = Assembly.GetAssembly(requestedType).GetTypes();

            foreach (Type type in types.Where(childType => IsTypeValidClassChild(childType, requestedType) && elegibilityCriteria.MeetsCriteria(childType)))
            {
                childrenTypes.Add(type);
            }

            if (sortedByName)
            {
                childrenTypes.Sort(SortTypeByName);
            }

            return childrenTypes;
        }

        /// <summary>
        /// Returns a list of child types of the requested type, filtered by the provided eligibility criteria and optionally sorted by name.
        /// </summary>
        /// <typeparam name="T">The type to find child types for</typeparam>
        /// <param name="elegibilityCriteria">Criteria to filter the child types</param>
        /// <param name="sortedByName">Whether to sort the resulting list by name</param>
        /// <returns>A list of child types that meet the criteria</returns>
        public static IEnumerable<Type> GetChildTypes<T>(TypeCriteria elegibilityCriteria = new(), bool sortedByName = false)
        {
            Type requestedType = typeof(T);
            return GetChildTypes(requestedType, elegibilityCriteria, sortedByName);
        }

        /// <summary>
        /// Returns a list of child types of the requested type, filtered by the provided eligibility criteria and optionally sorted by name.
        /// </summary>
        /// <param name="requestedType">The type to find child types for</param>
        /// <param name="elegibilityCriteria">Criteria to filter the child types</param>
        /// <param name="sortedByName">Whether to sort the resulting list by name</param>
        /// <returns>A list of child types that meet the criteria</returns>
        public static IEnumerable<Type> GetChildTypes(Type requestedType, TypeCriteria elegibilityCriteria = new(), bool sortedByName = false)
        {
            var typeCollection = TypeCache.GetTypesDerivedFrom(requestedType);
            List<Type> validTypes = new();

            foreach (var type in typeCollection.Where(childType => elegibilityCriteria.MeetsCriteria(childType)))
            {
                validTypes.Add(type);
            }

            if (sortedByName)
            {
                validTypes.Sort(SortTypeByName);
            }

            return validTypes;
        }

        /// <summary>
        /// Returns the main type, stripping away array or standard generic collection wrappers if present.
        /// </summary>
        /// <param name="type">The type to extract the core type from</param>
        /// <returns>The element type for arrays and generic collections, otherwise the original type</returns>
        public static Type GetCoreType(this Type type)
        {
            if (type == null) return null;

            if (type.IsArray)
            {
                return type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                // Handle type is one of target interfaces
                var genericDefinition = type.GetGenericTypeDefinition();
                if (genericDefinition == typeof(IList<>) ||
                    genericDefinition == typeof(ICollection<>) ||
                    genericDefinition == typeof(IEnumerable<>))
                {
                    return type.GetGenericArguments()[0];
                }

                // Handle classes implementing the target interfaces
                var collectionInterface = type.GetInterfaces()
                    .FirstOrDefault(x => x.IsGenericType && (
                        x.GetGenericTypeDefinition() == typeof(IList<>) ||
                        x.GetGenericTypeDefinition() == typeof(ICollection<>) ||
                        x.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                    ));

                if (collectionInterface != null)
                {
                    return type.GetGenericArguments()[0];
                }
            }
            return type;
        }

        public static string FormatWithTypeInfo<T>(this string format, T typeInstance, string nullFallback = "Null", int bufferSize = 32) 
            where T : class
        {
            Type type = typeof(T);
            return FormatWithTypeInfo(type, format, typeInstance, nullFallback, bufferSize);
        }

        public static string FormatWithTypeInfo(this Type type, string format, object typeInstance, string nullFallback = "Null", int bufferSize = 32)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "Type cannot be null.");
            }

            if (typeInstance == null)
            {
                throw new ArgumentNullException(nameof(typeInstance), "Type instance cannot be null.");
            }

            if (!type.IsAssignableFrom(typeInstance.GetType()))
            {
                throw new ArgumentException($"The provided type instance is of type '{typeInstance.GetType().FullName}' and is not assignable to the specified type '{type.FullName}'.", nameof(typeInstance));
            }

            const BindingFlags k_searchBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            int length = format.Length;
            StringBuilder sb = new StringBuilder(length + bufferSize);
            int index = 0;

            while (index < length)
            {
                int openBraceIndex = format.IndexOf('{', index);

                // If no more placeholders are found, append the rest of the string and break
                if (openBraceIndex == -1)
                {
                    sb.Append(format, index, length - index);
                    break;
                }

                // Append the text before the placeholder
                if (openBraceIndex > index)
                {
                    sb.Append(format, index, openBraceIndex - index);
                }

                int closeBraceIndex = format.IndexOf('}', openBraceIndex);
                if (closeBraceIndex == -1)
                {
                    // If there's no closing brace, treat it as a literal '{'. Maybe add a warning here in the future.
                    sb.Append(format, openBraceIndex, length - openBraceIndex);
                    break;
                }

                string infoName = format.Substring(openBraceIndex + 1, closeBraceIndex - openBraceIndex - 1);

                // Try to fill in the placeholder with the actual field value if found, otherwise treat it as a literal
                FieldInfo field = type.GetField(infoName, k_searchBindingFlags);
                object value = null;
                if (field != null)
                {
                    value = field.GetValue(typeInstance);
                    sb.Append(value != null ? value.ToString() : nullFallback);
                }
                else
                {
                    PropertyInfo property = type.GetProperty(infoName, k_searchBindingFlags);
                    if (property != null && property.CanRead)
                    {
                        value = property.GetValue(typeInstance);
                        sb.Append(value != null ? value.ToString() : nullFallback);
                    }
                    else
                    {
                        sb.Append(format, openBraceIndex, closeBraceIndex - openBraceIndex + 1);
                    }
                }

                index = closeBraceIndex + 1;
            }
            
            return sb.ToString();
        }

        private static int SortTypeByName(Type a, Type b)
        {
            return a.Name.CompareTo(b.Name);
        }

        private static bool IsTypeValidClassChild(Type childType, Type parentType) 
        {
            return childType.IsClass && childType.IsSubclassOf(parentType);
        }
    }
}
