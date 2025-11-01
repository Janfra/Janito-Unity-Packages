using UnityEngine;

namespace Janito.Animations
{
    [CreateAssetMenu(fileName = "AnimatorParameterEvent", menuName = "Scriptable Objects/Animation/Animator Trigger Parameter Event")]
    public class AnimatorTriggerParameterEvent : AnimatorParameterEvent
    {
        public override void ApplyEventValue(AnimatorModifierComponent modifierComponent)
        {
            modifierComponent.SetParameterTrigger(Parameter);
        }
    }
}