using Janito.EditorExtras;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

namespace Janito.Prototyping.Input
{
    // TODO: Add getting bind by group?
    // TODO: Add option to ignore conflicting bindings

    /// <summary>
    /// Wrapper for Unity's <c>RebindOperation</c> to expose the settings in inspector, the operation state through <c>UnityEvents</c>, and easily toggle the operation in between active and inactive.
    /// </summary>
    /// <remarks>
    /// It contains some additional information retrieved from the binding result, or from the targeted binding. It contain the binding name during its lifetime. Can easily be adapted into an UI rebind button by connecting <c>ToggleRebind</c> to the <c>Button</c> <c>OnClick</c> event.
    /// </remarks>
    public class RebindToggler : MonoBehaviour
    {
        public event UnityAction OnRebindStarted { add => _onRebindStarted.AddListener(value); remove => _onRebindStarted.RemoveListener(value); }
        public event UnityAction OnRebindCompleted { add => _onRebindCompleted.AddListener(value); remove => _onRebindCompleted.RemoveListener(value); }
        public event UnityAction OnRebindCanceled { add => _onRebindCanceled.AddListener(value); remove => _onRebindCanceled.RemoveListener(value); }

        /// <summary>
        /// Input action reference of the target input action to rebind. This is required for the rebind operation to work.
        /// </summary>
        [Header("Dependencies")]
        [SerializeField] 
        private InputActionReference _targetAction;

        /// <summary>
        /// Optional input action asset container to search for the target input action. If not set, it will fallback to the default project-wide input system actions asset.
        /// </summary>
        [Header("Optional")]
        [SerializeField] 
        private InputActionAsset _actionAssetContainer;

        /// <summary>
        /// Defines which controls to exclude from the rebind operation. This is useful to avoid rebinding to certain controls that are not desired, such as mouse or touch inputs.
        /// </summary>
        [Header("Configuration")]
        [SerializeField] 
        private string[] _excludedControls;

        /// <summary>
        /// Index of the binding to target for the rebind operation. If set to -1, it will not target any specific binding.
        /// </summary>
        [SerializeField]
        [Tooltip("Which index of the input action to target. Set to -1 for none.")]
        private int _targetBindingIndex = -1;

        /// <summary>
        /// Name of the binding that is currently targeted or has been rebound. This is read-only and updated automatically when the rebind operation completes or when the target binding index changes.
        /// </summary>
        [SerializeField]
        [ReadOnly]
        private string _bindDisplayName;
        [SerializeField]
        private bool _supressMatchingBinding = true;
        [SerializeField]
        private bool _supressActionEventNotifications = true;
        [SerializeField] 
        private float _matchDelay;

        [Header("Events")]
        [Space(5.0f)]
        [SerializeField]
        private UnityEvent _onRebindStarted;
        [SerializeField]
        private UnityEvent _onRebindCompleted;
        [SerializeField]
        private UnityEvent _onRebindCanceled;

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] 
        private bool _isLogging;
#endif

        public string BindDisplayName => _bindDisplayName;
        public InputActionReference TargetAction => _targetAction;
        public bool IsActive => _rebindingOperation != null && _rebindingOperation.started;

        private RebindingOperation _rebindingOperation;

        private void Start()
        {
            if (TryGetInputAction(out InputAction action) && IsTargetingIndex(action))
            {
                _bindDisplayName = GetFormattedNameFromBinding(action.bindings[_targetBindingIndex]);
            }
        }

        /// <summary>
        /// Switches the rebind operation between active and inactive. When active, the next valid input performed becomes the new binding for the selected target input action.
        /// </summary>
        [Button("Toggle Rebind", ButtonExecutionModes.PlayMode, "Starts or stops input listening. When enabled, the next valid input performed becomes the new binding for the selected target input action.")]
        public void ToggleRebind()
        {
            if (_rebindingOperation != null)
            {
                _rebindingOperation.Cancel();
                return;
            }

            if (!TryGetInputAction(out InputAction action))
            {
                return;
            }

            action.Disable();
            _rebindingOperation = action.PerformInteractiveRebinding();
            foreach (var excluded in _excludedControls)
            {
                _rebindingOperation.WithControlsExcluding(excluded);
            }

            if (IsTargetingIndex(action))
            {
                _rebindingOperation.WithTargetBinding(_targetBindingIndex);
                TryLogToEditor($"Targeting action bound to: {GetFormattedNameFromBinding(action.bindings[_targetBindingIndex])} - Path: {action.bindings[_targetBindingIndex].path}");
            }

            if (_matchDelay > 0) 
            {
                _rebindingOperation.OnMatchWaitForAnother(_matchDelay);
            }

            if (_supressMatchingBinding)
            {
                _rebindingOperation.WithMatchingEventsBeingSuppressed(_supressMatchingBinding);
                
                // Only set event notifications is supress is happening as otherwise it has no effect.
                _rebindingOperation.WithActionEventNotificationsBeingSuppressed(_supressActionEventNotifications); 
            }

            _rebindingOperation.OnCancel(OnCancel);
            _rebindingOperation.OnComplete(OnRebound);
            _rebindingOperation.Start();
            _onRebindStarted.Invoke();
        }

        private bool TryGetInputAction(out InputAction action)
        {
            var assetContainer = _actionAssetContainer ? _actionAssetContainer : InputSystem.actions;
            if (assetContainer == null || _targetAction == null)
            {
                action = null;
                return false;
            }

            action = assetContainer.FindAction(_targetAction.name);
            return action != null;
        }

        private void OnRebound(RebindingOperation completedOperation)
        {
            completedOperation.action.Enable();
            _bindDisplayName = completedOperation.selectedControl.displayName;
            TryLogToEditor($"Rebound {completedOperation.action.name} to {_bindDisplayName}");
            _onRebindCompleted.Invoke();
            DisposeBindingOperation();
        }

        private void OnCancel(RebindingOperation cancelledOperation)
        {
            TryLogToEditor($"Rebind canceled for {cancelledOperation.action.name}");
            _onRebindCanceled.Invoke();
            DisposeBindingOperation();
        }

        private bool IsTargetingIndex(InputAction action)
        {
            return _targetBindingIndex >= 0 && _targetBindingIndex < action.bindings.Count;
        }

        private string GetInputBindingNameForDevice(InputAction action, InputControl control)
        {
            var deviceName = control.device.displayName;
            var bindings = action.bindings;

            foreach (var binding in bindings)
            {
                if (binding.isPartOfComposite) { continue; }
                if (binding.path.Contains(deviceName))
                {
                    return GetFormattedNameFromBinding(binding);
                }
                else if (binding.groups.Contains(deviceName)) 
                {
                    return GetFormattedNameFromBinding(binding);
                }
                else if (binding.isComposite && deviceName == "Keyboard" && binding.path.Contains("Dpad"))
                {
                    return GetFormattedNameFromBinding(binding);
                }
            }

            return string.Empty;
        }

        private string GetFormattedNameFromBinding(InputBinding binding)
        {
            var result = binding.isComposite ? binding.name : binding.ToDisplayString();
            foreach (var interaction in binding.interactions)
            {
                result = result.Replace(interaction.ToString(), string.Empty);
            }

            result = result.Trim();
            return result;
        }

        private void DisposeBindingOperation()
        {
            _rebindingOperation.Dispose();
            _rebindingOperation = null;
        }

        private void OnValidate()
        {
            _matchDelay = Mathf.Max(0, _matchDelay);

            TryGetInputAction(out InputAction action);
            if (action != null)
            {
                _targetBindingIndex = Mathf.Clamp(_targetBindingIndex, -1, action.bindings.Count);
            }
            else
            {
                _targetBindingIndex = Mathf.Max(-1, _targetBindingIndex);
            }

            // Update display in editor of targeted binding input
            if (action != null && IsTargetingIndex(action))
            {
                _bindDisplayName = GetFormattedNameFromBinding(action.bindings[_targetBindingIndex]);
            }
            else if (_targetBindingIndex == -1)
            {
                _bindDisplayName = string.Empty;
            }
        }

        /// <summary>
        /// Convinience method only log in editor mode, prefix the type of the source of the log, and append the message.
        /// </summary>
        /// <param name="message">Message to log</param>
        [Conditional("UNITY_EDITOR")]
        [HideInCallstack]
        private void TryLogToEditor(string message) 
        {
            if (!_isLogging) return;

            this.LogWarningInDevelopment(message);
        }
    }
}
