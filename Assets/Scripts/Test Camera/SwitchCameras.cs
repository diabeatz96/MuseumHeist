using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class SwitchCameras : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;
    private int currentCameraIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentCameraIndex = 0;
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].Priority = i == currentCameraIndex ? 1 : 0;
        }
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
    }
}