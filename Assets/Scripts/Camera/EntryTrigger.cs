using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EntryTrigger : MonoBehaviour
{
public CameraManager cameraManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && (cameraManager.IsOutsideConfinerLeft() || cameraManager.IsOutsideRightConfinerLeft()))
        {
            cameraManager.SwitchToMainCamera();
        }
    }
}

