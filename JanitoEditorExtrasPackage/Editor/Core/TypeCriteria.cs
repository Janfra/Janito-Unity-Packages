using System;
using System.Collections.Generic;

namespace Janito.EditorExtras.Editor
{
    /// <summary>
    /// Checks that a type meets all flagged requirements to be elegible
    /// </summary>
    public readonly struct TypeCriteria
    {
        [Flags]
        public enum TypeRequirementFlags
        {
            None = 0,
            Interface = 1 << 0, NotInterface = 1 << 1,
            Abstract = 1 << 2, NotAbstract = 1 << 3,
            Generic = 1 << 4, NotGeneric = 1 << 5,
            MustBeAll = Abstract | Interface | Generic,
            MustNotBeAll = NotAbstract | NotInterface | NotGeneric
        }

        public readonly TypeRequirementFlags TypeRequirements;
        public TypeCriteria(TypeRequirementFlags requirementType = TypeRequirementFlags.None)
        {
            TypeRequirements = requirementType;

            // Check for bitwise contradictions (Include + Exclude on same property)
            if (HasContradiction(TypeRequirements, TypeRequirementFlags.Interface, TypeRequirementFlags.NotInterface))
                throw new ArgumentException("Cannot require both Interface and NotInterface.", nameof(requirementType));

            if (HasContradiction(TypeRequirements, TypeRequirementFlags.Abstract, TypeRequirementFlags.NotAbstract))
                throw new ArgumentException("Cannot require both Abstract and NotAbstract.", nameof(requirementType));

            if (HasContradiction(TypeRequirements, TypeRequirementFlags.Generic, TypeRequirementFlags.NotGeneric))
                throw new ArgumentException("Cannot require both Generic and NotGeneric.", nameof(requirementType));
        }

        /// <summary>
        /// Returns if a type meets the flagged criteria
        /// </summary>
        /// <remarks>The flags are inclusive, so all flagged requirements must be meet</remarks>
        public bool MeetsCriteria(Type type)
        {
            // Interface check
            if (TypeRequirements.HasFlag(TypeRequirementFlags.Interface) && !type.IsInterface) return false;
            if (TypeRequirements.HasFlag(TypeRequirementFlags.NotInterface) && type.IsInterface) return false;

            // Abstract check
            if (TypeRequirements.HasFlag(TypeRequirementFlags.Abstract) && !type.IsAbstract) return false;
            if (TypeRequirements.HasFlag(TypeRequirementFlags.NotAbstract) && type.IsAbstract) return false;

            // Generic check
            if (TypeRequirements.HasFlag(TypeRequirementFlags.Generic) && !type.IsGenericType) return false;
            if (TypeRequirements.HasFlag(TypeRequirementFlags.NotGeneric) && type.IsGenericType) return false;

            return true;
        }

        /// <summary>
        /// Removes any types that do not meet the criteria set on this object
        /// </summary>
        /// <param name="types">List of types to filter through</param>
        public void FilterTypeList(IList<Type> types)
        {
            for (int i = types.Count - 1; i >= 0; i--)
            {
                var type = types[i];
                if (type == null || !MeetsCriteria(type)) types.RemoveAt(i);
            }
        }

        // Helper to identify if both 'Include' and 'Exclude' flags are set
        private static bool HasContradiction(TypeRequirementFlags requirement, TypeRequirementFlags include, TypeRequirementFlags exclude)
        {
            return (requirement & include) != 0 && (requirement & exclude) != 0;
        }
    }
}
