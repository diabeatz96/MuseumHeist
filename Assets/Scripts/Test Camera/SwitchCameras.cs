using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using TMPro; // Import the TextMeshPro namespace

public class SwitchCameras : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;
    public Camera mainCamera; // The main Camera component
    public Camera tvCamera; // The second Camera component that's a child of the main camera
    public MeshRenderer tvScreen; // The MeshRenderer of the TV screen
    public TextMeshProUGUI cameraNameText; // The TextMeshPro component
    private int currentCameraIndex;
    private RenderTexture renderTexture;

    // Start is called before the first frame update
    void Start()
    {
        currentCameraIndex = 0;
        renderTexture = new RenderTexture(1920, 1080, 16); // Create a new RenderTexture
        tvScreen.material.mainTexture = renderTexture; // Set the RenderTexture as the main texture of the TV screen
        tvCamera.targetTexture = renderTexture; // Set the second Camera to output to the RenderTexture

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].Priority = i == currentCameraIndex ? 1 : 0;
        }

        // Display the name of the current camera
        cameraNameText.text = cameras[currentCameraIndex].name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            SwitchCamera(-1);
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            SwitchCamera(1);
        }
    }

    void SwitchCamera(int direction)
    {
        cameras[currentCameraIndex].Priority = 0;
        currentCameraIndex += direction;
        if (currentCameraIndex < 0) currentCameraIndex = cameras.Length - 1;
        if (currentCameraIndex >= cameras.Length) currentCameraIndex = 0;
        cameras[currentCameraIndex].Priority = 1;

        // Update the TextMeshPro text to display the name of the current camera
        cameraNameText.text = cameras[currentCameraIndex].name;
    }
}