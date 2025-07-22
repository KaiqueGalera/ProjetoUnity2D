using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRightTrigger : MonoBehaviour
{   
    public CameraManager  cameraManager; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !cameraManager.IsOutsideRightConfinerLeft())
        {
            cameraManager.SwitchToOutsideRightCamera();
        }
    }
}
