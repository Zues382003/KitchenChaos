using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance { get; private set; }
    
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause,
    }

    
    private PlayerInputAction _playerInputAction;

    private void Awake()
    {
        Instance = this;
        
        _playerInputAction = new PlayerInputAction();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            _playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
        
        _playerInputAction.Player.Enable();
        
        _playerInputAction.Player.Interact.performed += Interact_performed;
        _playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
        _playerInputAction.Player.Pause.performed += Pause_performed;


    }

    private void OnDestroy()
    {
        _playerInputAction.Player.Interact.performed -= Interact_performed;
        _playerInputAction.Player.InteractAlternate.performed -= InteractAlternate_performed;
        _playerInputAction.Player.Pause.performed -= Pause_performed;
        
        _playerInputAction.Dispose();
    }

    void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }
    
    void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }
    
    void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    
    public Vector2 GetMomentVectorNormalized()
    {
        Vector2 inputVector = _playerInputAction.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
                case Binding.Move_Up:
                    return _playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return _playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return _playerInputAction.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return _playerInputAction.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return _playerInputAction.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return _playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return _playerInputAction.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return _playerInputAction.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternate:
                return _playerInputAction.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return _playerInputAction.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBingding(Binding binding, Action onActionRebound)
    {
        _playerInputAction.Player.Disable();

        InputAction inputAction;
        int bindingIndex;
        
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = _playerInputAction.Player.Move;
                bindingIndex = 1;
                break;
            
            case Binding.Move_Down:
                inputAction = _playerInputAction.Player.Move;
                bindingIndex = 2;
                break;
            
            case Binding.Move_Left:
                inputAction = _playerInputAction.Player.Move;
                bindingIndex = 3;
                break;
            
            case Binding.Move_Right:
                inputAction = _playerInputAction.Player.Move;
                bindingIndex = 4;
                break;
            
            case Binding.Interact:
                inputAction = _playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;
            
            case Binding.InteractAlternate:
                inputAction = _playerInputAction.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            
            case Binding.Pause:
                inputAction = _playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = _playerInputAction.Player.Interact;
                bindingIndex = 1;
                break;
            
            case Binding.Gamepad_InteractAlternate:
                inputAction = _playerInputAction.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            
            case Binding.Gamepad_Pause:
                inputAction = _playerInputAction.Player.Pause;
                bindingIndex = 1;
                break;
        }
        
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete((callBack =>
            {
                callBack.Dispose();
                _playerInputAction.Player.Enable();
                onActionRebound();
                
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, _playerInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
                
                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })).Start();

        
    }
}
