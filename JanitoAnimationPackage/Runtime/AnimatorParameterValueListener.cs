using System;
using UnityEngine;

namespace Janito.Animations
{
    [Serializable]
    public abstract class AnimatorParameterValueListener
    {
        [SerializeField]
        private AnimatorParameterHasher _parameter;
        public AnimatorParameterHasher Parameter => _parameter;

        abstract public ParameterUpdateType UpdateType { get; }

        public abstract void Update(Animator animator);
    }
}