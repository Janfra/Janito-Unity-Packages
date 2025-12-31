using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Janito.Animations.Editor
{
    public sealed class ParameterOptionHandler
    {
        private const string ParameterButtonName = "ParameterButton";
        private const string IsGeneratedToggleName = "IsGeneratedToggle";
        private const string ParameterLabelName = "ParameterLabel";

        public Action<ParameterOptionHandler, bool> OnParameterSelectionChanged = delegate { };

        public readonly VisualElement Root;
        public readonly AnimatorControllerParameter Parameter;

        private readonly Button m_ParameterButton;
        private readonly Toggle m_IsGeneratedToggle;
        private readonly Label m_ParameterLabel;

        public ParameterOptionHandler(VisualElement root, AnimatorControllerParameter parameter)
        {
            Root = root;
            Parameter = parameter;

            m_ParameterButton = Root.Q<Button>(ParameterButtonName);
            Debug.Assert(m_ParameterButton != null);
            m_IsGeneratedToggle = Root.Q<Toggle>(IsGeneratedToggleName);
            Debug.Assert(m_IsGeneratedToggle != null);
            m_ParameterLabel = Root.Q<Label>(ParameterLabelName);
            Debug.Assert(m_ParameterLabel != null);

            SetParameterName();
            SetupEvents();
        }

        public void SetSelectionTo(bool isSelected)
        {
            if (isSelected == m_IsGeneratedToggle.value) return;

            m_IsGeneratedToggle.value = isSelected;
            UpdateSelectionState();
        }

        public void OnClear()
        {
            OnParameterSelectionChanged = null;
        }

        private void SetParameterName()
        {
            m_ParameterLabel.text = Parameter.name;
        }

        private void SetupEvents()
        {
            m_ParameterButton.RegisterCallback<ClickEvent>(OnParameterClicked);
            m_IsGeneratedToggle.RegisterCallback<ClickEvent>(OnParameterToggled);
        }

        private void OnParameterClicked(ClickEvent evt)
        {
            m_IsGeneratedToggle.value = !m_IsGeneratedToggle.value;
            UpdateSelectionState();
        }

        private void OnParameterToggled(ClickEvent evt)
        {
            evt.StopPropagation();
            UpdateSelectionState();
        }

        private void UpdateSelectionState()
        {
            OnParameterSelectionChanged.Invoke(this, m_IsGeneratedToggle.value);
        }
    }
}
