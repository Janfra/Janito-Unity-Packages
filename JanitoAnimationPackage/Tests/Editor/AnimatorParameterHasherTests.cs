using NUnit.Framework;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.TestTools;

namespace Janito.Animations.Tests.Editor
{
    public class AnimatorParameterHasherTests
    {
        private AnimatorParameterHasher _animatorParameterHasherUnderTest;
        private AnimatorController _mockAnimatorController;

        /// <summary>
        /// Used when parameter name is not relevant for test but a valid name must be provided for initialisation.
        /// </summary>
        private const string _placeholderName = "TestParameter";

        /// <summary>
        /// Used when parameter type is not relevant for test but must be provided for initialisation.
        /// </summary>
        private const AnimatorControllerParameterType _placeholderType = AnimatorControllerParameterType.Bool;

        [SetUp]
        public void CreateAnimatorControllerAndAnimatorParameterHasherUnderTest()
        {
            _mockAnimatorController = AnimatorTestHelpers.CreateMockAnimatorController();
            _animatorParameterHasherUnderTest = ScriptableObject.CreateInstance<AnimatorParameterHasher>();
        }

        [TearDown]
        public void DestroyAnimatorControllerAndAnimatorParameterHasherUnderTest()
        {
            Object.DestroyImmediate(_mockAnimatorController);
            ScriptableObject.DestroyImmediate(_animatorParameterHasherUnderTest);
        }

        [Test]
        public void HasParameter_ShouldReturnTrue_WhenParameterExistsInAnimator()
        {
            const string testParameterName = "TestParameter";
             // Type is not relevant for this test
            _animatorParameterHasherUnderTest.Initialise(testParameterName, _placeholderType);
            // Create a temporary Animator with the test parameter
            Animator animator = new GameObject("TempGameObject").AddComponent<Animator>();
            _mockAnimatorController.AddParameter(testParameterName, _placeholderType);
            animator.runtimeAnimatorController = _mockAnimatorController;

            bool hasParameter = _animatorParameterHasherUnderTest.HasParameter(animator);

            Object.DestroyImmediate(animator.gameObject);
            Assert.That(hasParameter, Is.True);
        }
    }
}
