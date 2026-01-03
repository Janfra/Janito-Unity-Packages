using System;
using UnityEngine;

namespace Janito.Animations
{
    [Serializable]
    public class AnimatorParameterVelocityListener : AnimatorParameterValueListener
    {
        [SerializeField]
        private Rigidbody m_Rigidbody;
        public override ParameterUpdateType UpdateType => ParameterUpdateType.OnLateUpdate;

        public override void Update(Animator animator)
        {
            // Sqr velocity for quicker calculation
            animator.SetFloat(Parameter.ID, m_Rigidbody.linearVelocity.sqrMagnitude);
        }
    }
}
