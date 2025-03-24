using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetup : MonoBehaviour
{
    public ControllerInput player1ControllerInput;
    public ControllerInput player2ControllerInput;

    void Start()
    {
        
        var devices = InputSystem.devices;

        
        if (devices.Count > 0 && devices[0] is Gamepad)
        {
            player1ControllerInput.playerDevice = devices[0];
            Debug.Log("Player 1 assigned to device: " + devices[0].name);
        }

        
        if (devices.Count > 1 && devices[1] is Gamepad)
        {
            player2ControllerInput.playerDevice = devices[1];
            Debug.Log("Player 2 assigned to device: " + devices[1].name);
        }
    }
}