using UnityEngine;
using UnityEngine.Events;

namespace Janito.EditorExtras.EventChannel
{
    public class VoidEventChannelListener : MonoBehaviour
    {
        [CreateButton]
        [SerializeField]
        private VoidEventChannelSO m_Channel = default;

        [Space]
        public UnityEvent OnEventRaised;

        private void OnEnable()
        {
            if (m_Channel)
            {
                m_Channel.OnEventRaised += Respond;
            }
        }

        private void OnDisable()
        {
            if (m_Channel)
            {
                m_Channel.OnEventRaised -= Respond;
            }
        }

        private void Respond()
        {
            OnEventRaised?.Invoke();
        }
    }
}
