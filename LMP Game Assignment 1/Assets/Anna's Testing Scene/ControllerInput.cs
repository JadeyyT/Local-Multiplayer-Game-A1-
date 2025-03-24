using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerInput : MonoBehaviour
{
    private SamuraiControls controls;

    public bool isSlashing;
    public bool isDodgingLeft;
    public bool isDodgingRight;
    public bool isClapping;

    [Header("Player Input Device")]
    public InputDevice playerDevice;

    void Awake()
    {
        controls = new SamuraiControls();

        if (playerDevice != null)
        {
            controls.devices = new[] { playerDevice };
            Debug.Log("ControllerInput assigned to: " + playerDevice.name);
        }
        else
        {
            Debug.LogWarning("No playerDevice assigned in ControllerInput on " + gameObject.name);
        }

        // Bind input actions
        controls.GamepadControls.Slash.performed += ctx => isSlashing = true;
        controls.GamepadControls.Slash.canceled += ctx => isSlashing = false;

        controls.GamepadControls.DodgeLeft.performed += ctx => isDodgingLeft = true;
        controls.GamepadControls.DodgeLeft.canceled += ctx => isDodgingLeft = false;

        controls.GamepadControls.DodgeRight.performed += ctx => isDodgingRight = true;
        controls.GamepadControls.DodgeRight.canceled += ctx => isDodgingRight = false;

        controls.GamepadControls.Clap.performed += ctx => isClapping = true;
        controls.GamepadControls.Clap.canceled += ctx => isClapping = false;
    }

    void OnEnable()
    {
        if (controls != null)
        {
            controls.Enable();
        }
    }

    void OnDisable()
    {
        if (controls != null)
        {
            controls.Disable();
        }
    }
}
