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

        public static IEnumerable<Type> GetChildTypes<T>(TypeCriteria elegibilityCriteria = new(), bool sortedByName = false)
        {
            Type requestedType = typeof(T);
            return GetChildTypes(requestedType, elegibilityCriteria, sortedByName);
        }

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
