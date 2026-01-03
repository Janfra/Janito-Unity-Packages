using UnityEngine;

namespace Janito.Animations
{
    [CreateAssetMenu(fileName = "AnimatorParameterEvent", menuName = "Scriptable Objects/Animation/Animator Bool Parameter Event")]
    public class AnimatorBoolParameterEvent : AnimatorParameterEvent
    {
        [SerializeField]
        private bool m_Value;

        public override void ApplyEventValue(AnimatorModifierComponent modifierComponent)
        {
            modifierComponent.SetParameterBool(Parameter, m_Value);
        }
    }
}
