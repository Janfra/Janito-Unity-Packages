using Janito.EditorExtras;
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Janito.Animations.Editor
{
    public sealed class ParameterOptionHandler
    {
        private const string ParameterButtonName = "ParameterButton";
        private const string IsGeneratedToggleName = "IsGeneratedToggle";
        private const string ParameterNameLabelName = "ParameterNameLabel";
        private const string ParameterTypeLabelName = "ParameterTypeLabel";

        public Action<ParameterOptionHandler, bool> OnParameterSelectionChanged = delegate { };

        public readonly VisualElement Root;
        public readonly AnimatorControllerParameter Parameter;
        public readonly SerializedProperty ParameterProperty;

        private readonly Button m_ParameterButton;
        private readonly Toggle m_IsGeneratedToggle;
        private readonly Label m_ParameterNameLabel;
        private readonly Label m_ParameterTypeLabel;

        public ParameterOptionHandler(VisualElement root, AnimatorControllerParameter parameter)
        {
            Root = root;
            Parameter = parameter;

            ValidationLibrary.SetAndValidateReference(ref m_ParameterButton, Root.Q<Button>(ParameterButtonName));
            ValidationLibrary.SetAndValidateReference(ref m_IsGeneratedToggle, Root.Q<Toggle>(IsGeneratedToggleName));
            ValidationLibrary.SetAndValidateReference(ref m_ParameterNameLabel, Root.Q<Label>(ParameterNameLabelName));
            ValidationLibrary.SetAndValidateReference(ref m_ParameterTypeLabel, Root.Q<Label>(ParameterTypeLabelName));

            SetParameterLabels();
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

        private void SetParameterLabels()
        {
            m_ParameterNameLabel.text = Parameter.name;
            m_ParameterTypeLabel.text = Parameter.type.ToString();
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
