using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        /// Returns the type of a field info, handling arrays and generic types.
        /// </summary>
        /// <param name="fieldInfo">Field info to extract the type from</param>
        /// <returns>The type of the field info</returns>
        public static Type GetFieldType(FieldInfo fieldInfo)
        {
            if (fieldInfo == null) return null;
            var type = fieldInfo.FieldType;
            if (type.IsArray)
            {
                type = type.GetElementType();
            }
            else if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                if (genericType == typeof(IList<>) || genericType == typeof(ICollection<>) || genericType == typeof(IEnumerable<>))
                {
                    type = type.GetGenericArguments()[0];
                }
            }
            return type;
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
