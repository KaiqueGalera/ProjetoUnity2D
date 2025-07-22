using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [Header("Porta")]
    private Animator            doorAnimator;
    private BoxCollider2D       dCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = GetComponent<Animator>();
        dCollider2D   = GetComponent<BoxCollider2D>();
    }

    public void OpenDoor()
    {
        doorAnimator.SetTrigger("Open");
    }

    public void DestroyCollider()
    {
        dCollider2D.enabled = false;
    }
}
