namespace Janito.EditorExtras.Editor
{
    public static class TypeCriteriaExtensions
    {
        public static TypeCriteria RequireInterface(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.NotInterface;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.Interface);
        }

        public static TypeCriteria RequireAbstract(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.NotAbstract;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.Abstract);
        }

        public static TypeCriteria RequireGeneric(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.NotGeneric;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.Generic);
        }
        public static TypeCriteria ExcludeInterface(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.Interface;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.NotInterface);
        }

        public static TypeCriteria ExcludeAbstract(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.Abstract;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.NotAbstract);
        }

        public static TypeCriteria ExcludeGeneric(this TypeCriteria typeCriteria)
        {
            var filteredExistingFlags = typeCriteria.TypeRequirements & ~TypeCriteria.TypeRequirementFlags.Generic;
            return new TypeCriteria(filteredExistingFlags | TypeCriteria.TypeRequirementFlags.NotGeneric);
        }
    }
}
