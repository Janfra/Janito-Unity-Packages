namespace Janito.EditorExtras.Editor
{
    public static class TypeCriteriaExtensions
    {
        /// <summary>
        /// Adds a requirement to the TypeCriteria that the type must be an interface. If the TypeCriteria already has a requirement that the type must not be an interface, this will override that requirement.
        /// </summary>
        /// <param name="typeCriteria">The TypeCriteria to modify</param>
        /// <returns>A new TypeCriteria with the interface requirement added</returns>
        public static TypeCriteria RequireInterface(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.NotInterface;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.Interface);
        }

        /// <summary>
        /// Adds a requirement to the TypeCriteria that the type must be abstract. If the TypeCriteria already has a requirement that the type must not be abstract, this will override that requirement.
        /// </summary>
        /// <param name="typeCriteria">The TypeCriteria to modify</param>
        /// <returns>A new TypeCriteria with the abstract requirement added</returns>
        public static TypeCriteria RequireAbstract(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.NotAbstract;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.Abstract);
        }

        /// <summary>
        /// Adds a requirement to the TypeCriteria that the type must be generic. If the TypeCriteria already has a requirement that the type must not be generic, this will override that requirement. 
        /// </summary>
        /// <param name="typeCriteria">The TypeCriteria to modify</param>
        /// <returns>A new TypeCriteria with the generic requirement added</returns>
        public static TypeCriteria RequireGeneric(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.NotGeneric;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.Generic);
        }

        /// <summary>
        /// Adds a requirement to the TypeCriteria that the type must not be an interface. If the TypeCriteria already has a requirement that the type must be an interface, this will override that requirement.
        /// </summary>
        /// <param name="typeCriteria">The TypeCriteria to modify</param>
        /// <returns>A new TypeCriteria with the not interface requirement added</returns>
        public static TypeCriteria ExcludeInterface(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.Interface;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.NotInterface);
        }

        /// <summary>
        /// Adds a requirement to the TypeCriteria that the type must not be abstract. If the TypeCriteria already has a requirement that the type must be abstract, this will override that requirement.
        /// </summary>
        /// <param name="typeCriteria">The TypeCriteria to modify</param>
        /// <returns>A new TypeCriteria with the not abstract requirement added</returns>
        public static TypeCriteria ExcludeAbstract(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.Abstract;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.NotAbstract);
        }

        /// <summary>
        /// Adds a requirement to the TypeCriteria that the type must not be generic. If the TypeCriteria already has a requirement that the type must be generic, this will override that requirement.
        /// </summary>
        /// <param name="typeCriteria">The TypeCriteria to modify</param>
        /// <returns>A new TypeCriteria with the not generic requirement added</returns>
        public static TypeCriteria ExcludeGeneric(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.Generic;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.NotGeneric);
        }
    }
}
