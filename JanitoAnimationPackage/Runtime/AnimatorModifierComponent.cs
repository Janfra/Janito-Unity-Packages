using Janito.EditorExtras;
using System.Collections.Generic;
using UnityEngine;

namespace Janito.Animations
{
    public class AnimatorModifierComponent : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Animator _animator;
        [SerializeReference, ChildTypeSelection(typeof(AnimatorParameterValueListener))]
        private AnimatorParameterValueListener[] _staticValueListeners;

        private List<AnimatorParameterValueListener> _onUpdateValueListeners = new();
        private List<AnimatorParameterValueListener> _onLateUpdateValueListeners = new();

        private void Awake()
        {
            if (_animator == null)
            {
                throw new System.NullReferenceException($"Animator reference is null on {nameof(AnimatorModifierComponent)} attached to GameObject: {gameObject.name}. Please set the reference before playing.");
            }

            InitialiseStaticValueListeners();
        }

        private void Update()
        {
            foreach (AnimatorParameterValueListener listener in _onUpdateValueListeners)
            {
                listener.Update(_animator);
            }
        }

        private void LateUpdate()
        {
            foreach (AnimatorParameterValueListener listener in _onLateUpdateValueListeners)
            {
                listener.Update(_animator);
            }
        }

        public void ApplyAnimatorParameterEvent(AnimatorParameterEvent parameterEvent)
        {
            parameterEvent.ApplyEventValue(this);
        }

        public void SetParameterFloat(AnimatorParameterHasher parameter, float value)
        {
            _animator.SetFloat(parameter.ID, value);
        }

        public void SetParameterInt(AnimatorParameterHasher parameter, int value)
        {
            _animator.SetInteger(parameter.ID, value);
        }

        public void SetParameterBool(AnimatorParameterHasher parameter, bool value)
        {
            _animator.SetBool(parameter.ID, value);
        }

        public void SetParameterTrigger(AnimatorParameterHasher parameter)
        {
            _animator.SetTrigger(parameter.ID);
        }

        private void InitialiseStaticValueListeners()
        {
            foreach (AnimatorParameterValueListener listener in _staticValueListeners)
            {
                if (listener == null) continue;
                if (!IsListenerParameterValid(listener))
                {
                    Debug.LogWarning($"Ignored {listener} due to having invalid parameter for animator in {name}. Parameter: {(listener.Parameter ? listener.Parameter.ReadableParameterName : null)}");
                    continue;
                }

                if (listener.UpdateType == ParameterUpdateType.OnUpdate)
                {
                    _onUpdateValueListeners.Add(listener);
                }
                else if (listener.UpdateType == ParameterUpdateType.OnLateUpdate)
                {
                    _onLateUpdateValueListeners.Add(listener);
                }
            }
        }

        private bool IsListenerParameterValid(AnimatorParameterValueListener listener)
        {
            return listener.Parameter && listener.Parameter.HasParameter(_animator);
        }

        private void OnValidate()
        {
            _animator = _animator ? _animator : GetComponent<Animator>();
        }
    }
}