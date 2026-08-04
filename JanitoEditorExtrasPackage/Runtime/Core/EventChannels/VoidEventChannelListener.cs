using UnityEngine;
using UnityEngine.Events;

namespace Janito.EditorExtras.EventChannel
{
    public class VoidEventChannelListener : MonoBehaviour
    {
        [SerializeField]
        private VoidEventChannelSO m_Channel = default;

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
