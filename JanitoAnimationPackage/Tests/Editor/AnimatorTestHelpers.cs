using UnityEngine;
using UnityEditor.Animations;

namespace Janito.Animations.Tests.Editor
{
    public static class AnimatorTestHelpers
    {
        public static AnimatorController CreateMockAnimatorController()
        {
            AnimatorController animatorController = new AnimatorController();
            animatorController.name = "MockController";
            animatorController.AddLayer("Base Layer"); // Layer is a requirement for controller to be valid.
            return animatorController;
        }

        public static AnimatorController CreateMockAnimatorControllerWithParameter(string parameterName, AnimatorControllerParameterType type)
        {
            AnimatorController animatorController = CreateMockAnimatorController();
            animatorController.AddParameter(parameterName, type);
            return animatorController;
        }
    }
}
