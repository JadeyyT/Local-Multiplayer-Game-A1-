using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetup : MonoBehaviour
{
    public ControllerInput player1ControllerInput;
    public ControllerInput player2ControllerInput;

    void Start()
    {
        // Log all connected devices for debugging
        var devices = InputSystem.devices;
        foreach (var device in devices)
        {
            Debug.Log("Detected device: " + device.name);
        }

        // Assign first gamepad to Player 1
        if (devices.Count > 0 && devices[0] is Gamepad gamepad1)
        {
            player1ControllerInput.playerDevice = gamepad1;
            Debug.Log("Player 1 assigned to: " + gamepad1.name);
        }
        else
        {
            Debug.LogWarning("No gamepad found for Player 1");
        }

        // Assign second gamepad to Player 2
        if (devices.Count > 1 && devices[1] is Gamepad gamepad2)
        {
            player2ControllerInput.playerDevice = gamepad2;
            Debug.Log("Player 2 assigned to: " + gamepad2.name);
        }
        else
        {
            Debug.LogWarning("No gamepad found for Player 2");
        }
    }
}
