using UnityEngine;

namespace Janito.Animations
{
    public static class AnimatorExtensions
    {
        public static void SetFloat(this Animator animator, AnimatorParameterHasher parameter, float value)
        {
            animator.SetFloat(parameter.ID, value);
        }   

        public static void SetInteger(this Animator animator, AnimatorParameterHasher parameter, int value)
        {
            animator.SetInteger(parameter.ID, value);
        }

        public static void SetBool(this Animator animator, AnimatorParameterHasher parameter, bool value)
        {
            animator.SetBool(parameter.ID, value);
        }

        public static void SetTrigger(this Animator animator, AnimatorParameterHasher parameter)
        {
            animator.SetTrigger(parameter.ID);
        }

        public static float GetFloat(this Animator animator, AnimatorParameterHasher parameter)
        {
            return animator.GetFloat(parameter.ID);
        }

        public static int GetInteger(this Animator animator, AnimatorParameterHasher parameter)
        {
            return animator.GetInteger(parameter.ID);
        }

        public static bool GetBool(this Animator animator, AnimatorParameterHasher parameter)
        {
            return animator.GetBool(parameter.ID);
        }
    }
}
