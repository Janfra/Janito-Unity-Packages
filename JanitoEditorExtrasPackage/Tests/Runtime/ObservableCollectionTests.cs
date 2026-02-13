using Janito.EditorExtras.Observables;
using NUnit.Framework;
using System.Collections.Generic;

namespace Janito.EditorExtras.Tests.Runtime
{
    public class ObservableCollectionTests
    {
        /// <summary>
        /// Create a new empty observable collection to test.
        /// </summary>
        /// <returns>A new observable collection of generic <c>List<></c> type of <c>int</c>.</returns>
        public ObservableCollection<List<int>, int> GetNewEmptyCollectionToTest() => new(new());

        /// <summary>
        /// Create a new observable collection initiliased to provided list to test.
        /// </summary>
        /// <param name="list">List contained in the resulting observable collection.</param>
        /// <returns>A new observable collection containing provided list.</returns>
        public ObservableCollection<List<int>, int> GetCollectionFromListToTest(List<int> list) => new(list);

        [Test]
        public void ObservableCollection_NullArgumentIsValidForConstructor()
        {
            using var observableUnderTest = new ObservableCollection<List<int>, int>(null);

            // No direct access is provided to the internal collection, instead we use the Read Only property exposed as it is a reference to the internal collection as a Read Only.
            Assert.That(observableUnderTest.ReadOnlyCollection, Is.Not.Null);
        }

        [Test]
        public void ObservableCollection_ConstructorArgumentBecomesCollection()
        {
            var populatedList = new List<int>()
            {
                1,
                2,
                3
            };

            using var observableUnderTest = GetCollectionFromListToTest(populatedList);

            Assert.That(observableUnderTest.ReadOnlyCollection, Is.Not.Null);
            Assert.That(observableUnderTest.Count, Is.EqualTo(populatedList.Count));
            Assert.That(observableUnderTest.ReadOnlyCollection, Is.EqualTo(populatedList));
        }

        [Test]
        public void Add_AddsItemSuccessfully()
        {
            // Arrange
            var addedValue = 100;
            using var observableUnderTest = GetNewEmptyCollectionToTest();

            // Act
            observableUnderTest.Add(addedValue);
        
            // Assert
            Assert.That(observableUnderTest.Count, Is.EqualTo(1));
            Assert.That(observableUnderTest, Does.Contain(addedValue));
        }

        [Test]
        public void Remove_RemovesItemSuccessfully()
        {
            // Arrange
            int target = 10;
            var populatedList = new List<int>()
            {
                target - 1,
                target,
                target + 1
            };
            using var observableUnderTest = GetCollectionFromListToTest(populatedList);

            // Act
            var result = observableUnderTest.Remove(target);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(observableUnderTest.Count, Is.EqualTo(2));
            Assert.That(observableUnderTest, Does.Not.Contains(target));
        }

        [Test]
        public void Remove_CannotRemoveItemMissingFromList()
        {
            // Arrange
            int target = 10;
            var populatedList = new List<int>()
            {
                target - 1,
                target + 1
            };
           using var observableUnderTest = GetCollectionFromListToTest(populatedList);

            // Act
            var result = observableUnderTest.Remove(target);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(observableUnderTest.Count, Is.EqualTo(2));
            Assert.That(observableUnderTest, Does.Not.Contains(target));
        }

        [Test]
        public void Add_InvokesEventOnAdd()
        {
            // Arrange
            int? callbackValue = null;
            int addedValue = 1;
            using var observableUnderTest = GetNewEmptyCollectionToTest();
            void SetCallbackValue(int value) => callbackValue = value;
            observableUnderTest.OnItemAdded += SetCallbackValue;

            // Act
            observableUnderTest.Add(addedValue);

            // Assert
            Assert.That(callbackValue, Is.Not.Null.And.EqualTo(addedValue));
        }

        [Test]
        public void Remove_InvokesEventOnSuccessfulRemove()
        {
            // Arrange
            int? callbackValue = null;
            int targetValue = 1;
            var populatedList = new List<int>()
            {
                targetValue
            };
            using var observableUnderTest = GetCollectionFromListToTest(populatedList);
            void SetCallbackValue(int value) => callbackValue = value;
            observableUnderTest.OnItemRemoved += SetCallbackValue;

            // Act
            observableUnderTest.Remove(targetValue);

            // Assert
            Assert.That(callbackValue, Is.Not.Null.And.EqualTo(targetValue));  
        }

        [Test]
        public void Remove_DoesNotInvokeEventOnFailedRemove()
        {
            // Arrange
            int? callbackValue = null;
            int targetValue = 1;
            using var observableUnderTest = GetNewEmptyCollectionToTest();
            void SetCallbackValue(int value) => callbackValue = value;
            observableUnderTest.OnItemRemoved += SetCallbackValue;

            // Act
            observableUnderTest.Remove(targetValue);

            // Assert
            Assert.That(callbackValue, Is.Null);
        }

        [Test]
        public void Clear_EmptiesList()
        {
            // Arrange
            var populatedList = new List<int>()
            {
                1,
                2
            };
            using var observableUnderTest = GetCollectionFromListToTest(populatedList);

            // Act
            observableUnderTest.Clear();

            // Assert
            Assert.That(observableUnderTest, Is.Empty);
            Assert.That(observableUnderTest.Count, Is.EqualTo(0));
        }

        [Test]
        public void Clear_InvokesEventOnSuccessfulClear()
        {
            // Arrange
            bool wasInvoked = false;
            var populatedList = new List<int>()
            {
                1,
                2
            };
            using var observableUnderTest = GetCollectionFromListToTest(populatedList);
            void SetCallbackValue() => wasInvoked = true;
            observableUnderTest.OnCleared += SetCallbackValue;

            // Act
            observableUnderTest.Clear();

            // Assert
            Assert.That(wasInvoked, Is.True);
        }

        [Test]
        public void Clear_DoesNotInvokeEventOnNoChange()
        {
            // Arrange
            bool wasInvoked = false;
            using var observableUnderTest = GetNewEmptyCollectionToTest();
            void SetCallbackValue() => wasInvoked = true;
            observableUnderTest.OnCleared += SetCallbackValue;

            // Act
            observableUnderTest.Clear();

            // Assert
            Assert.That(wasInvoked, Is.False);
        }
    }
}
