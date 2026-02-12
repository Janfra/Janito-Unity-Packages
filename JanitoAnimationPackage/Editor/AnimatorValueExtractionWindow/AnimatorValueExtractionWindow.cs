using Janito.Animations.Editor;
using Janito.EditorExtras;
using Janito.EditorExtras.Editor;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class AnimatorValueExtractionWindow : EditorWindow
{
    private const string AnimatorFieldName = "AnimatorField";
    private const string ConfirmButtonName = "ConfirmButton";
    private const string PathLabelName = "PathLabel";
    private const string PathSelectionButtonName = "PathSelectionButton";
    private const string ParameterTabName = "ParameterTab";

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    [SerializeField]
    private VisualTreeAsset m_ParameterAsset = default;

    // Visual variables
    private VisualElement m_Root;
    private ObjectField m_AnimatorField;
    private Button m_ConfirmButton;
    private Tab m_ParameterTab;
    private Label m_PathLabel;
    private Button m_PathSelectionButton;

    // Logic variables
    [SerializeField]
    private AnimatorController m_AnimatorController;
    [SerializeField]
    private string m_DestinationPath = "Assets";
    private ParameterTabHandler m_ParameterTabHandler;
    private SerializedObject m_SerializedAnimator;

    [MenuItem("Tools/Janito/Animator Value Extraction Window")]
    public static void ShowWindow()
    {
        AnimatorValueExtractionWindow wnd = GetWindow<AnimatorValueExtractionWindow>();
        wnd.titleContent = new GUIContent("Animator Value Extraction Window");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        m_Root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        m_Root.Add(labelFromUXML);

        FetchReferences();
        SetupEvents();
        SetupTabs();
        SetupReferences();
    }

    private void FetchReferences()
    {
        this.SetAndValidateReference(ref m_AnimatorField, m_Root.Q<ObjectField>(AnimatorFieldName));
        this.SetAndValidateReference(ref m_ConfirmButton, m_Root.Q<Button>(ConfirmButtonName));
        this.SetAndValidateReference(ref m_ParameterTab, m_Root.Q<Tab>(ParameterTabName));
        this.SetAndValidateReference(ref m_PathLabel, m_Root.Q<Label>(PathLabelName));
        this.SetAndValidateReference(ref m_PathSelectionButton, m_Root.Q<Button>(PathSelectionButtonName));
    }

    private void SetupTabs()
    {
        m_ParameterTabHandler = new(m_ParameterTab, m_ParameterAsset);
    }

    private void SetupEvents()
    {
        m_AnimatorField.RegisterValueChangedCallback(OnAnimatorSet);
        m_ConfirmButton.RegisterCallback<ClickEvent>(OnConfirm);
        m_PathSelectionButton.RegisterCallback<ClickEvent>(OnPathSelection);
        m_Root.RegisterCallback<DetachFromPanelEvent>(OnDetached);
    }

    private void SetupReferences()
    {
        m_PathLabel.text = m_DestinationPath;
        if (m_AnimatorController != null) 
        {
            m_AnimatorField.value = m_AnimatorController;
            SetSerializedAnimatorReference();
        } 
    }

    private void OnPathSelection(ClickEvent evt)
    {
        if (PathLibrary.TryGetProjectPathFromUser(out string destinationPath))
        {
            m_PathLabel.text = destinationPath;
            m_DestinationPath = destinationPath;
        }
    }

    private void OnConfirm(ClickEvent evt)
    {
        if (m_AnimatorController == null || string.IsNullOrEmpty(m_DestinationPath)) return;

        m_ParameterTabHandler.GenerateAt(m_DestinationPath);
    }

    private void OnAnimatorSet(ChangeEvent<Object> evt)
    {
        RemoveOldSerializedAnimatorReference();
        if (evt.newValue is AnimatorController runtime)
        {
            m_AnimatorController = runtime;
            SetSerializedAnimatorReference();
            m_ConfirmButton.SetEnabled(true);
            m_ParameterTabHandler.AddParameterListFromAnimator(m_AnimatorController);
        }
        else
        {
            Clear();
        }
    }

    private void RemoveOldSerializedAnimatorReference()
    {
        m_Root.Unbind();
        m_SerializedAnimator?.Dispose();
    }

    private void SetSerializedAnimatorReference()
    {
        if (m_SerializedAnimator != null)
        {
            RemoveOldSerializedAnimatorReference();
        }

        m_SerializedAnimator = new(m_AnimatorController);
        m_Root.TrackSerializedObjectValue(m_SerializedAnimator, CheckForRelevantChanges);
    }

    private void CheckForRelevantChanges(SerializedObject @object)
    {
        // For now assume the parameter list changed and readd the parameters
        m_ParameterTabHandler.AddParameterListFromAnimator(m_AnimatorController);
    }

    private void Clear()
    {
        m_ConfirmButton.SetEnabled(false);
        m_ParameterTabHandler.Clear();
    }

    private void OnDetached(DetachFromPanelEvent evt)
    {
        RemoveOldSerializedAnimatorReference();
    }
}
