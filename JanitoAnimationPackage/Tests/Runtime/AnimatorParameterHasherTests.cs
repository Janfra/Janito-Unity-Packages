using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Janito.Animations.Tests.Runtime
{
    public class AnimatorParameterHasherTests
    {
        private AnimatorParameterHasher _animatorParameterHasherUnderTest;

        /// <summary>
        /// Used when parameter name is not relevant for test but a valid name must be provided for initialisation.
        /// </summary>
        private const string _placeholderName = "TestParameter";
        
        /// <summary>
        /// Used when parameter type is not relevant for test but must be provided for initialisation.
        /// </summary>
        private const AnimatorControllerParameterType _placeholderType = AnimatorControllerParameterType.Bool;

        [SetUp]
        public void CreateParameterHasherUnderTest()
        {
            _animatorParameterHasherUnderTest = ScriptableObject.CreateInstance<AnimatorParameterHasher>();
        }

        [TearDown]
        public void DestroyParameterHasherUnderTest()
        {
            ScriptableObject.DestroyImmediate(_animatorParameterHasherUnderTest);
        }

        [Test]
        public void ID_ShouldMatchAnimatorStringHashResult()
        {
            const string testParameterName = "TestParameter";
            int expectedHash = Animator.StringToHash(testParameterName);

            _animatorParameterHasherUnderTest.Initialise(testParameterName, _placeholderType);

            Assert.That(_animatorParameterHasherUnderTest.ID, Is.EqualTo(expectedHash));
        }

        [Test]
        public void Type_ShouldMatchAnimatorControllerParameterType()
        {
            AnimatorControllerParameterType expectedType = AnimatorControllerParameterType.Bool;

            _animatorParameterHasherUnderTest.Initialise(_placeholderName, expectedType);

            Assert.That(_animatorParameterHasherUnderTest.Type, Is.EqualTo(expectedType));
        }

        [Test]
        public void ReadableParameterName_ShouldMatchInitialisedParameterName()
        {
            const string expectedParameterName = "TestParameter";

            _animatorParameterHasherUnderTest.Initialise(expectedParameterName, _placeholderType);
            
            Assert.That(_animatorParameterHasherUnderTest.ReadableParameterName, Is.EqualTo(expectedParameterName));
        }

        [Test]
        public void Initialise_ShouldThrowArgumentException_WhenParameterNameIsNullOrEmpty()
        {
            const AnimatorControllerParameterType placeholderType = AnimatorControllerParameterType.Bool; // Type is not relevant for this test

            Assert.Throws<System.ArgumentException>(() => _animatorParameterHasherUnderTest.Initialise(null, placeholderType));
            Assert.Throws<System.ArgumentException>(() => _animatorParameterHasherUnderTest.Initialise(string.Empty, placeholderType));
        }

        [Test]
        public void Initialise_ShouldThrowArgumentException_WhenParameterNameIsWhitespace()
        {
            const string emptyParameterName = "   ";

            Assert.Throws<System.ArgumentException>(() => _animatorParameterHasherUnderTest.Initialise(emptyParameterName, _placeholderType));
        }

        [Test]
        public void Initialise_ShouldIgnoreReinitialisation_WhenAlreadyInitialised()
        {
            const string initialParameterName = "InitialParameter";
            const AnimatorControllerParameterType initialType = AnimatorControllerParameterType.Float;
            _animatorParameterHasherUnderTest.Initialise(initialParameterName, initialType);

            // Attempt to reinitialise with different values
            const string newParameterName = "NewParameter";
            const AnimatorControllerParameterType newType = AnimatorControllerParameterType.Int;
            _animatorParameterHasherUnderTest.Initialise(newParameterName, newType);

            // Verify that the original values are retained
            Assert.That(_animatorParameterHasherUnderTest.ReadableParameterName, Is.EqualTo(initialParameterName));
            Assert.That(_animatorParameterHasherUnderTest.Type, Is.EqualTo(initialType));
        }

        [Test]
        public void HasParameter_ShouldReturnFalse_WhenAnimatorDoesNotContainParameter()
        {
            const string testParameterName = "NonExistentParameter";
            _animatorParameterHasherUnderTest.Initialise(testParameterName, _placeholderType);
            // Create a temporary GameObject with an Animator component without any parameters
            Animator animator = new GameObject("TempGameObject").AddComponent<Animator>();

            // Ensure the animator does not contain the parameter
            bool result = _animatorParameterHasherUnderTest.HasParameter(animator);

            // Clean up the temporary GameObject
            Object.DestroyImmediate(animator.gameObject);
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_ShouldReturnFalse_WhenContainNullID()
        {
            // Use unitialised parameter hasher that has no ID
            bool result = _animatorParameterHasherUnderTest.IsValid;

            Assert.That(result, Is.Not.True);
        }

        [Test]
        public void IsValid_ShouldReturnTrue_WhenContainID()
        {
            const string validName = "TestParameter";
            _animatorParameterHasherUnderTest.Initialise(validName, _placeholderType);

            bool result = _animatorParameterHasherUnderTest.IsValid;

            Assert.That(result, Is.True);
        }
    }
}