using Janito.EditorExtras;
using System.Collections.Generic;
using UnityEngine;

namespace Janito.Animations
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorModifierComponent : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Animator m_Animator;
        [SerializeReference, ChildTypeSelection(typeof(AnimatorParameterValueListener))]
        private AnimatorParameterValueListener[] m_StaticValueListeners;

        private List<AnimatorParameterValueListener> m_OnUpdateValueListeners = new();
        private List<AnimatorParameterValueListener> m_OnLateUpdateValueListeners = new();

        private void Awake()
        {
            m_Animator = m_Animator ? m_Animator : GetComponent<Animator>();
            if (m_Animator == null)
            {
                throw new System.NullReferenceException($"Animator reference is null on {nameof(AnimatorModifierComponent)} attached to GameObject: {gameObject.name}. Please set the reference before playing.");
            }

            InitialiseStaticValueListeners();
        }

        private void Update()
        {
            foreach (AnimatorParameterValueListener listener in m_OnUpdateValueListeners)
            {
                listener.Update(m_Animator);
            }
        }

        private void LateUpdate()
        {
            foreach (AnimatorParameterValueListener listener in m_OnLateUpdateValueListeners)
            {
                listener.Update(m_Animator);
            }
        }

        public void ApplyAnimatorParameterEvent(AnimatorParameterEvent parameterEvent)
        {
            parameterEvent.ApplyEventValue(this);
        }

        public void SetParameterFloat(AnimatorParameterHasher parameter, float value)
        {
            m_Animator.SetFloat(parameter, value);
        }

        public void SetParameterInt(AnimatorParameterHasher parameter, int value)
        {
            m_Animator.SetInteger(parameter, value);
        }

        public void SetParameterBool(AnimatorParameterHasher parameter, bool value)
        {
            m_Animator.SetBool(parameter, value);
        }

        public void SetParameterTrigger(AnimatorParameterHasher parameter)
        {
            m_Animator.SetTrigger(parameter);
        }

        private void InitialiseStaticValueListeners()
        {
            foreach (AnimatorParameterValueListener listener in m_StaticValueListeners)
            {
                if (listener == null) continue;
                if (!IsListenerParameterValid(listener))
                {
                    Debug.LogWarning($"Ignored {listener} due to having invalid parameter for animator in {name}. Parameter: {(listener.Parameter ? listener.Parameter.ReadableParameterName : null)}");
                    continue;
                }

                if (listener.UpdateType == ParameterUpdateType.OnUpdate)
                {
                    m_OnUpdateValueListeners.Add(listener);
                }
                else if (listener.UpdateType == ParameterUpdateType.OnLateUpdate)
                {
                    m_OnLateUpdateValueListeners.Add(listener);
                }
            }
        }

        private bool IsListenerParameterValid(AnimatorParameterValueListener listener)
        {
            return listener.Parameter && listener.Parameter.HasParameter(m_Animator);
        }

        private void OnValidate()
        {
            m_Animator = m_Animator ? m_Animator : GetComponent<Animator>();
        }
    }
}