
namespace Janito.EditorExtras.Singletons
{
    public abstract class LazySingleton<TDerived> 
        where TDerived : LazySingleton<TDerived>, new()
    {
        private static TDerived m_Instance;

        public static TDerived Get()
        {
            m_Instance = m_Instance ?? new();
            return m_Instance;
        }
    }
}
