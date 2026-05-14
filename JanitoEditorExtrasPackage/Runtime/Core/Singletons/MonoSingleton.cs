using UnityEngine;

namespace Janito.EditorExtras.Singletons
{
    public abstract class MonoSingleton<TDerived> : MonoBehaviour
    where TDerived : Component
    {
        private static TDerived m_Instance;

        public bool IsPersistentThroughScenes = false;
        public static bool HasInstance => m_Instance != null;
        public static TDerived Instance
        {
            get
            {
                if (!HasInstance)
                {
                    m_Instance = FindAnyObjectByType<TDerived>();
                    if (!HasInstance)
                    {
                        var gameObject = new GameObject(typeof(TDerived).Name + " Generated Singleton");
                        m_Instance = gameObject.AddComponent<TDerived>();
                    }
                }

                return m_Instance;
            }
        }

        /// <summary>
        /// Override must execute <c>base.Awake()</c> for singleton initialisation.
        /// </summary>
        protected virtual void Awake()
        {
            InitialiseSingleton();
        }

        protected virtual void InitialiseSingleton()
        {
            if (!Application.isPlaying) return;

            if (!HasInstance)
            {
                m_Instance = this as TDerived;

                if (IsPersistentThroughScenes)
                {
                    transform.SetParent(null);
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (m_Instance != this)
            {
                Destroy(this);
            }
        }
    }
}
