using UnityEngine;

namespace Janito.Animations
{
    [CreateAssetMenu(fileName = "AnimatorParameterEvent", menuName = "Scriptable Objects/Animation/Animator Bool Parameter Event")]
    public class AnimatorBoolParameterEvent : AnimatorParameterEvent
    {
        [SerializeField]
        private bool _value;

        public override void ApplyEventValue(AnimatorModifierComponent modifierComponent)
        {
            modifierComponent.SetParameterBool(Parameter, _value);
        }
    }
}
