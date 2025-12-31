using Janito.Animations.Editor;
using Janito.EditorExtras;
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
    private AnimatorController m_AnimatorController;
    private ParameterTabHandler m_ParameterTabHandler;
    private string m_DestinationPath = string.Empty;

    [MenuItem("Tools/Janito/AnimatorValueExtractionWindow")]
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
    }

    private void OnPathSelection(ClickEvent evt)
    {
        string path = EditorUtility.OpenFolderPanel("Select Destination", "Assets", string.Empty);

        if (!string.IsNullOrEmpty(path)) 
        {
            if (path.StartsWith(Application.dataPath))
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
                m_PathLabel.text = path;
                m_DestinationPath = path;
            }
        }
    }

    private void OnConfirm(ClickEvent evt)
    {
        if (m_AnimatorController == null || string.IsNullOrEmpty(m_DestinationPath)) return;

        m_ParameterTabHandler.GenerateAt(m_DestinationPath);
    }

    private void OnAnimatorSet(ChangeEvent<Object> evt)
    {
        if (evt.newValue is AnimatorController runtime)
        {
            m_AnimatorController = runtime;
            m_ConfirmButton.SetEnabled(true);
            m_ParameterTabHandler.AddParameterListFromAnimator(m_AnimatorController);
        }
        else
        {
            Clear();
        }
    }

    private void Clear()
    {
        m_ConfirmButton.SetEnabled(false);
        m_ParameterTabHandler.Clear();
    }
}
