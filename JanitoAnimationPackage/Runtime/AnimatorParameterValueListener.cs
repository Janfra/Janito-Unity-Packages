using System;
using UnityEngine;

namespace Janito.Animations
{
    [Serializable]
    public abstract class AnimatorParameterValueListener
    {
        [SerializeField]
        private AnimatorParameterHasher m_Parameter;
        public AnimatorParameterHasher Parameter => m_Parameter;

        abstract public ParameterUpdateType UpdateType { get; }

        public abstract void Update(Animator animator);
    }
}