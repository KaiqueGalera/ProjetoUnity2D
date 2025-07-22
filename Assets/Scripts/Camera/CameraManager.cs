using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera outsideLeftConfinerCamera;
    public CinemachineVirtualCamera outsideRightConfinerCamera;

    private bool isOutsideConfinerLeft = false;
    private bool isOutsideConfinerRight = false;

    public void SwitchToOutsideLeftCamera()
    {
        isOutsideConfinerLeft = true;
        isOutsideConfinerRight = false;
        outsideLeftConfinerCamera.Priority = 10;
        mainCamera.Priority = 5;
        outsideRightConfinerCamera.Priority = 5;
    }
    public void SwitchToOutsideRightCamera()
    {
        isOutsideConfinerRight = true;
        isOutsideConfinerLeft = false;
        outsideRightConfinerCamera.Priority = 10;
        mainCamera.Priority = 5;
        outsideLeftConfinerCamera.Priority = 5;
    }

    public void SwitchToMainCamera()
    {
        isOutsideConfinerLeft = false;
        isOutsideConfinerRight = false;
        mainCamera.Priority = 10;
        outsideLeftConfinerCamera.Priority = 5;
        outsideRightConfinerCamera.Priority = 5;
    }

    public bool IsOutsideConfinerLeft()
    {
        return isOutsideConfinerLeft;
    }
    public bool IsOutsideRightConfinerLeft()
    {
        return isOutsideConfinerRight;
    }
}

