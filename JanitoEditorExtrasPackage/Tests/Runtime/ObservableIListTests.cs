using Janito.EditorExtras.Observables;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Janito.EditorExtras.Tests.Runtime
{
    public class ObservableIListTests
    {
        protected ObservableIList<List<int>, int> GetNewEmptyObservableList() => new(new());
        protected ObservableIList<List<int>, int> GetObservableListFromList(List<int> list) => new(list);

        [Test]
        public void ObservableIList_ConstructorInitialisesBaseClass()
        {
            var observableUnderTest = GetNewEmptyObservableList();

            Assert.That(observableUnderTest.ReadOnlyCollection, Is.Not.Null);
        }

        [Test]
        public void ObservableIList_ConstructorNullArgumentIsValid()
        {
            var observableUnderTest = new ObservableIList<List<int>, int>(null);

            Assert.That(observableUnderTest.ReadOnlyCollection, Is.Not.Null);
        }

        [Test]
        public void ObservableIList_ConstructorArgumentBecomesList()
        {
            var populatedList = new List<int>() { 1, 2 };

            var observableUnderTest = GetObservableListFromList(populatedList);

            Assert.That(observableUnderTest.ReadOnlyList, Is.EqualTo(populatedList));
            Assert.That(observableUnderTest.ReadOnlyCollection, Is.EqualTo(populatedList));
        } 

        [Test]
        public void ReadOnlyList_PropertyReturnsReadOnlyInstance()
        {
            var observableUnderTest = GetNewEmptyObservableList();

            Assert.That(observableUnderTest.ReadOnlyList, Is.Not.Null);
        }

        [Test]
        public void Index_IndexModificationsAreApplied()
        {
            int appliedValue = 4;
            var populatedList = new List<int> { 1, 2, 3 };
            var observableUnderTest = GetObservableListFromList(populatedList);

            observableUnderTest[0] = appliedValue;

            Assert.That(observableUnderTest[0], Is.EqualTo(appliedValue));
        }

        [Test]
        public void Index_CanPerformIndexAccess()
        {
            var targetValue = 5;
            var populatedList = new List<int> { targetValue };
            var observableUnderTest = GetObservableListFromList(populatedList);

            var value = observableUnderTest[0];

            Assert.That(value, Is.EqualTo(targetValue));
        }

        [Test]
        public void Index_IndexSetInvokesAddEvent()
        {
            int? callbackValue = null;
            var list = new List<int>() { 0 };
            var observableUnderTest = GetObservableListFromList(list);
            void SetCallbackValue(int value) { callbackValue = value; }
            observableUnderTest.OnItemAdded += SetCallbackValue;

            observableUnderTest[0] = 1;

            Assert.That(callbackValue, Is.EqualTo(1));
        }

        [Test]
        public void Index_IndexSetInvokesRemoveEvent()
        {
            int? callbackValue = null;
            int initialValue = 5;
            var list = new List<int>() { initialValue };
            var observableUnderTest = GetObservableListFromList(list);
            void SetCallbackValue(int value) { callbackValue = value; }
            observableUnderTest.OnItemRemoved += SetCallbackValue;

            observableUnderTest[0] = 2;

            Assert.That(callbackValue, Is.EqualTo(initialValue));
        }
    }
}
