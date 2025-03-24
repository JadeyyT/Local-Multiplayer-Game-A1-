using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetup : MonoBehaviour
{
    [Header("Player Input Settings")]
    [SerializeField] private PlayerInput _player1Input;
    [SerializeField] private PlayerInput _player2Input;

    [Header("Control Schemes")]
    [SerializeField] private string _keyboardControlScheme = "Keyboard";
    [SerializeField] private string _gamepadControlScheme = "Gamepad";

    private void Start()
    {
        ConfigurePlayerInputs();
    }

    private void ConfigurePlayerInputs()
    {
        var gamepads = Gamepad.all;

        if (gamepads.Count > 0)
        {
            _player1Input.SwitchCurrentControlScheme(_gamepadControlScheme, gamepads[0]);
        }
        else
        {
            _player1Input.SwitchCurrentControlScheme(_keyboardControlScheme, Keyboard.current);
        }

        if (gamepads.Count > 1)
        {
            _player2Input.SwitchCurrentControlScheme(_gamepadControlScheme, gamepads[1]);
        }
        else
        {
            _player2Input.SwitchCurrentControlScheme(_keyboardControlScheme, Keyboard.current);
        }
    }
}