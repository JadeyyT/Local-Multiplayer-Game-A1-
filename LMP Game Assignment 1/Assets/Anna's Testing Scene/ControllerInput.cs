using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class ControllerInput : MonoBehaviour
{
    // 输入状态
    public bool IsSlashing { get; private set; }
    public bool IsDodgingLeft { get; private set; }
    public bool IsDodgingRight { get; private set; }
    public bool IsClapping { get; private set; }

    // 输入配置
    [Header("Keyboard Bindings")]
    [SerializeField] private KeyCode _keyboardSlash = KeyCode.Space;
    [SerializeField] private KeyCode _keyboardDodgeLeft = KeyCode.A;
    [SerializeField] private KeyCode _keyboardDodgeRight = KeyCode.D;
    [SerializeField] private KeyCode _keyboardClap = KeyCode.Q;

    // 内部状态
    private PlayerInput _playerInput;
    private bool _usingGamepad;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _usingGamepad = _playerInput.currentControlScheme == "Gamepad";

        // 初始化输入绑定
        SetupInputCallbacks();
    }

    private void SetupInputCallbacks()
    {
        var actions = _playerInput.actions;

        // 手柄输入绑定
        actions["Slash"].performed += _ => IsSlashing = true;
        actions["Slash"].canceled += _ => IsSlashing = false;

        actions["DodgeLeft"].performed += _ => IsDodgingLeft = true;
        actions["DodgeLeft"].canceled += _ => IsDodgingLeft = false;

        actions["DodgeRight"].performed += _ => IsDodgingRight = true;
        actions["DodgeRight"].canceled += _ => IsDodgingRight = false;

        actions["Clap"].performed += _ => IsClapping = true;
        actions["Clap"].canceled += _ => IsClapping = false;
    }

    private void Update()
    {
        // 统一处理键盘输入
        if (!_usingGamepad)
        {
            IsSlashing = Input.GetKey(_keyboardSlash);
            IsDodgingLeft = Input.GetKey(_keyboardDodgeLeft);
            IsDodgingRight = Input.GetKey(_keyboardDodgeRight);
            IsClapping = Input.GetKey(_keyboardClap);
        }
    }
}