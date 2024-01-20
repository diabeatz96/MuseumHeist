using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using TMPro; // Import the TextMeshPro namespace

public class SwitchCameras : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;
    public CinemachineVirtualCamera playerCamera; // The player's CinemachineVirtualCamera component
    public MeshRenderer tvScreen; // The MeshRenderer of the TV screen
    public TextMeshProUGUI cameraNameText; // The TextMeshPro component
    private int currentCameraIndex;
    private RenderTexture renderTexture;
    private bool isInCameraMode = false; // Add this line

    // Start is called before the first frame update
    void Start()
    {
        currentCameraIndex = 0;
        renderTexture = new RenderTexture(1920, 1080, 16); // Create a new RenderTexture
        tvScreen.material.mainTexture = renderTexture; // Set the RenderTexture as the main texture of the TV screen

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].Priority = i == currentCameraIndex ? 1 : 0;
        }

        // Display the name of the current camera
        cameraNameText.text = cameras[currentCameraIndex].name;
        playerCamera = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame    
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (isInCameraMode)
            {
                ExitCamera();
            }
            else
            {
                EnterCameraMode();
            }
        }
        else if (isInCameraMode && Keyboard.current.qKey.wasPressedThisFrame)
        {
            SwitchCamera(-1);
        }
        else if (isInCameraMode && Keyboard.current.wKey.wasPressedThisFrame)
        {
            SwitchCamera(1);
        }
    }

    void EnterCameraMode()
    {
        isInCameraMode = true;
        // Optionally, you can switch to the first security camera here
        SwitchCamera(0);
    }

        public void ExitCamera()
    {
        isInCameraMode = false;
        // Set the priority of the current camera to 0
        cameras[currentCameraIndex].Priority = 0;

        // Set the priority of the player camera to 1
        playerCamera.Priority = 1;

        // Update the TextMeshPro text to display the name of the current camera
        cameraNameText.text = playerCamera.name;
    }

    public void SwitchCamera(int direction)
    {
        cameras[currentCameraIndex].Priority = 0;
        currentCameraIndex += direction;
        if (currentCameraIndex < 0) currentCameraIndex = cameras.Length - 1;
        if (currentCameraIndex >= cameras.Length) currentCameraIndex = 0;
        cameras[currentCameraIndex].Priority = 1;

        // Lower the priority of the player camera if the current camera is not the player camera
        playerCamera.Priority = cameras[currentCameraIndex] == playerCamera ? 1 : 0;

        // Update the TextMeshPro text to display the name of the current camera
        cameraNameText.text = cameras[currentCameraIndex].name;
    }

}