using System;
using UnityEngine;

namespace Janito.Animations
{
    [Serializable]
    public abstract class AnimatorParameterValueListener
    {
        [SerializeField]
        private AnimatorParameterHasher m_Parameter;
        public AnimatorParameterHasher Parameter { 
            get { return m_Parameter; }
            protected set { m_Parameter = m_Parameter ?? value; }
        } 

        abstract public ParameterUpdateType UpdateType { get; }

        public abstract void Update(Animator animator);
    }
}