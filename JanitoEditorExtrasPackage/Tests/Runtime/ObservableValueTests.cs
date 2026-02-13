using Janito.EditorExtras.Observables;
using NUnit.Framework;

namespace Janito.EditorExtras.Tests.Runtime
{
    public class ObservableValueTests
    {
        [Test]
        public void ObservableValue_ConstructorArgumentBecomesValue()
        {
            int argumentValue = 10;

            var observableUnderTest = new ObservableValue<int>(argumentValue);

            Assert.That(observableUnderTest.Value, Is.EqualTo(argumentValue));
        }

        [Test]
        public void Value_ValueSetChangesValue()
        {
            int originalValue = 10;
            int newValue = 5;
            var observableUnderTest = new ObservableValue<int>(originalValue);

            observableUnderTest.Value = newValue;

            Assert.That(observableUnderTest.Value, Is.EqualTo(newValue));
        }

        [Test]
        public void Value_ValueSetInvokesEvent()
        {
            int? callbackValue = null;
            int newValue = 10;
            var observableUnderTest = new ObservableValue<int>();
            void ValueChangedCallback(int value) { callbackValue = value; }
            observableUnderTest.OnValueChanged += ValueChangedCallback;

            observableUnderTest.Value = newValue;

            Assert.That(callbackValue, Is.Not.Null.And.EqualTo(newValue));
        }

        [Test]
        public void ChangeWithoutNotify_ChangesValue()
        {
            int originalValue = 10;
            int newValue = 5;
            var observableUnderTest = new ObservableValue<int>(originalValue);

            observableUnderTest.ChangeWithoutNotify(newValue);

            Assert.That(observableUnderTest.Value, Is.EqualTo(newValue));
        }

        [Test]
        public void ChangeWithoutNotify_DoesNotInvokeEvent()
        {
            int? callbackValue = null;
            int newValue = 10;
            var observableUnderTest = new ObservableValue<int>();
            void ValueChangedCallback(int value) { callbackValue = value; }
            observableUnderTest.OnValueChanged += ValueChangedCallback;

            observableUnderTest.ChangeWithoutNotify(newValue);

            Assert.That(callbackValue, Is.Null);
        }
    }
}
