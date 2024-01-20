using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    public void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    public void SetCameraTarget(Transform target)
    {
        virtualCamera.Follow = target;
        virtualCamera.LookAt = target;
    }
}
