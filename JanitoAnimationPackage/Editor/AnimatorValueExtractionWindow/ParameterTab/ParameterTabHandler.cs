using Janito.EditorExtras;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Janito.Animations.Editor
{
    public sealed class ParameterTabHandler
    {
        private const string ToolbarSearchName = "ToolbarSearchField";
        private const string ToolbarSelectAllButton = "ToolbarSelectAllButton";
        private const string ToolbarUnselectAllButton = "ToolbarUnselectAllButton";
        private const string ParameterBoxName = "ParameterBox";

        private readonly VisualTreeAsset m_ParameterAsset;
        private readonly ToolbarSearchField m_ToolbarSearchField;
        private readonly ToolbarButton m_ToolbarSelectButton;
        private readonly ToolbarButton m_ToolbarUnselectButton;
        private readonly GroupBox m_ParameterBox;

        private List<ParameterOptionHandler> m_ParameterHandlers = new();
        private List<ParameterOptionHandler> m_SelectedParameters = new();

        public ParameterTabHandler(VisualElement root, VisualTreeAsset parameterAsset)
        {
            ValidationLibrary.SetAndValidateReference(ref m_ToolbarSearchField, root.Q<ToolbarSearchField>(ToolbarSearchName));
            ValidationLibrary.SetAndValidateReference(ref m_ToolbarSelectButton, root.Q<ToolbarButton>(ToolbarSelectAllButton));
            ValidationLibrary.SetAndValidateReference(ref m_ToolbarUnselectButton, root.Q<ToolbarButton>(ToolbarUnselectAllButton));
            ValidationLibrary.SetAndValidateReference(ref m_ParameterBox, root.Q<GroupBox>(ParameterBoxName));
            ValidationLibrary.SetAndValidateReference(ref m_ParameterAsset, parameterAsset);

            SetupEvents();
        }
        
        public void GenerateAt(string path)
        {
            if (m_SelectedParameters.Count == 0) return;

            foreach (var handler in m_SelectedParameters)
            {
                AnimatorParameterHasher parameterHasher = ScriptableObject.CreateInstance<AnimatorParameterHasher>();
                parameterHasher.Initialise(handler.Parameter.name, handler.Parameter.type);

                // Override any assets with the same name
                string assetPath = path + $"/{handler.Parameter.name}_AnimParamHash.asset";
                AssetDatabase.CreateAsset(parameterHasher, assetPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void Clear()
        {
            RemoveParameterList();
        }

        public void AddParameterListFromAnimator(AnimatorController animatorController)
        {
            RemoveParameterList();

            foreach (var parameter in animatorController.parameters)
            {
                ParameterOptionHandler handler = AddParameter(parameter);
                handler.OnParameterSelectionChanged = HandleParameterSelectionChange;
            }
        }

        private void SetupEvents()
        {
            m_ToolbarSelectButton.RegisterCallback<ClickEvent>(OnSelectAllParameters);
            m_ToolbarUnselectButton.RegisterCallback<ClickEvent>(OnUnselectAllParameters);
            m_ToolbarSearchField.RegisterValueChangedCallback(OnSearchChanged);
        }

        private void OnSearchChanged(ChangeEvent<string> evt)
        {
            if (m_ParameterHandlers.Count == 0) return;

            string searchLowercased = evt.newValue.ToLower();
            foreach (var handler in m_ParameterHandlers)
            {
                string nameLowercased = handler.Parameter.name.ToLower();
                if (!nameLowercased.Contains(searchLowercased))
                {
                    if (!handler.Root.ClassListContains(USSClassNames.Hidden))
                    {
                        handler.Root.AddToClassList(USSClassNames.Hidden);
                    }
                }
                else if (handler.Root.ClassListContains(USSClassNames.Hidden))
                {
                    handler.Root.RemoveFromClassList(USSClassNames.Hidden);
                }
            }
        }

        private void OnUnselectAllParameters(ClickEvent evt)
        {
            SetAllParametersSelectionTo(false);
        }

        private void OnSelectAllParameters(ClickEvent evt)
        {
            SetAllParametersSelectionTo(true);
        }

        private void SetAllParametersSelectionTo(bool isSelected)
        {
            foreach (var handler in m_ParameterHandlers)
            {
                handler.SetSelectionTo(isSelected);
            }
        }

        private ParameterOptionHandler AddParameter(AnimatorControllerParameter parameter)
        {
            ParameterOptionHandler handler = new(m_ParameterAsset.Instantiate(), parameter);
            m_ParameterBox.Add(handler.Root);
            m_ParameterHandlers.Add(handler);
            return handler; 
        }

        private void RemoveParameterList()
        {
            foreach (var handler in m_ParameterHandlers)
            {
                handler.OnClear();
            }

            m_ParameterHandlers.Clear();
            m_ParameterBox.Clear();
        }

        private void HandleParameterSelectionChange(ParameterOptionHandler handler, bool isSelected)
        {
            if (isSelected) 
            {
                if (m_SelectedParameters.Contains(handler))
                {
                    Debug.LogWarning($"Parameter is already selected and is being selected again, ignoring it.");
                    return;
                }

                m_SelectedParameters.Add(handler);
            }
            else
            {
                m_SelectedParameters.Remove(handler);
            }
        }
    }
}
