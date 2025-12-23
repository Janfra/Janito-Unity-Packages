using UnityEngine;

namespace Janito.Animations
{
    public static class AnimatorExtensions
    {
        public static void SetParameterFloat(this Animator animator, AnimatorParameterHasher parameter, float value)
        {
            animator.SetFloat(parameter.ID, value);
        }   

        public static void SetParameterInt(this Animator animator, AnimatorParameterHasher parameter, int value)
        {
            animator.SetInteger(parameter.ID, value);
        }

        public static void SetParameterBool(this Animator animator, AnimatorParameterHasher parameter, bool value)
        {
            animator.SetBool(parameter.ID, value);
        }

        public static void SetParameterTrigger(this Animator animator, AnimatorParameterHasher parameter)
        {
            animator.SetTrigger(parameter.ID);
        }
    }
}
