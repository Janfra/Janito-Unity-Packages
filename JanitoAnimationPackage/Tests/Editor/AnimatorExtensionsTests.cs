using System.Collections;
using NUnit.Framework;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.TestTools;

namespace Janito.Animations.Tests.Editor
{
    public class AnimatorExtensionsTests
    {
        private Animator _animator;
        private AnimatorController _animatorController;
        private AnimatorParameterHasher _animatorParameterHasher;

        [SetUp]
        public void SetUp()
        {
            // Do not assign animator controller here, as it will be assigned in the test method after adding parameters to it, otherwise the parameters will not be recognized by the animator.
            _animator = new GameObject("TempGameObject").AddComponent<Animator>();
            _animatorController = AnimatorTestHelpers.CreateMockAnimatorController();
            _animatorParameterHasher = ScriptableObject.CreateInstance<AnimatorParameterHasher>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_animator.gameObject);
            Object.DestroyImmediate(_animatorController);
            ScriptableObject.DestroyImmediate(_animatorParameterHasher);
        }

        [Test]
        public void SetFloat_ShouldSetAnimatorFloatParameter()
        {
            const string parameterName = "TestFloatParameter";
            const AnimatorControllerParameterType parameterType = AnimatorControllerParameterType.Float;
            _animatorController.AddParameter(parameterName, parameterType);
            _animatorParameterHasher.Initialise(parameterName, parameterType);
            _animator.runtimeAnimatorController = _animatorController;
            const float expectedValue = 3.14f;

            _animator.SetFloat(_animatorParameterHasher, expectedValue);

            Assert.That(_animator.GetFloat(parameterName), Is.EqualTo(expectedValue));
        }

        [Test]
        public void SetBool_ShouldSetAnimatorBoolParameter()
        {
            const string parameterName = "TestBoolParameter";
            const AnimatorControllerParameterType parameterType = AnimatorControllerParameterType.Bool;
            _animatorController.AddParameter(parameterName, parameterType);
            _animatorParameterHasher.Initialise(parameterName, parameterType);
            _animator.runtimeAnimatorController = _animatorController;
            const bool expectedValue = true;

            _animator.SetBool(_animatorParameterHasher, expectedValue);

            Assert.That(_animator.GetBool(parameterName), Is.EqualTo(expectedValue));
        }

        [Test]
        public void SetInteger_ShouldSetAnimatorIntegerParameter()
        {
            const string parameterName = "TestIntegerParameter";
            const AnimatorControllerParameterType parameterType = AnimatorControllerParameterType.Int;
            _animatorController.AddParameter(parameterName, parameterType);
            _animatorParameterHasher.Initialise(parameterName, parameterType);
            _animator.runtimeAnimatorController = _animatorController;
            const int expectedValue = 42;
            _animator.SetInteger(_animatorParameterHasher, expectedValue);
            Assert.That(_animator.GetInteger(parameterName), Is.EqualTo(expectedValue));
        }

        [Test]
        public void SetTrigger_ShouldSetAnimatorTriggerParameter()
        {
            const string parameterName = "TestTriggerParameter";
            const AnimatorControllerParameterType parameterType = AnimatorControllerParameterType.Trigger;
            _animatorController.AddParameter(parameterName, parameterType);
            _animatorParameterHasher.Initialise(parameterName, parameterType);
            _animator.runtimeAnimatorController = _animatorController;

            _animator.SetTrigger(_animatorParameterHasher);

            Assert.That(_animator.GetBool(parameterName), Is.True);
        }

        [Test]
        public void ResetTrigger_ShouldResetAnimatorTriggerParameter()
        {
            const string parameterName = "TestTriggerParameter";
            const AnimatorControllerParameterType parameterType = AnimatorControllerParameterType.Trigger;
            _animatorController.AddParameter(parameterName, parameterType);
            _animatorParameterHasher.Initialise(parameterName, parameterType);
            _animator.runtimeAnimatorController = _animatorController;
            _animator.SetTrigger(parameterName);

            // Verify that the trigger is set before resetting it
            Assert.That(_animator.GetBool(parameterName), Is.True);

            _animator.ResetTrigger(_animatorParameterHasher);

            Assert.That(_animator.GetBool(parameterName), Is.False);
        }

        [Test]
        public void GetFloat_ShouldReturnAnimatorFloatParameterValue()
        {
            const string parameterName = "TestFloatParameter";
            const AnimatorControllerParameterType parameterType = AnimatorControllerParameterType.Float;
            _animatorController.AddParameter(parameterName, parameterType);
            _animatorParameterHasher.Initialise(parameterName, parameterType);
            _animator.runtimeAnimatorController = _animatorController;
            const float expectedValue = 2.71f;
            _animator.SetFloat(parameterName, expectedValue);

            float actualValue = _animator.GetFloat(_animatorParameterHasher);
            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }
    }
}
