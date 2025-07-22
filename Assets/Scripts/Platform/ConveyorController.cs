using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{   
    public  float           conveyorSpeed;

    private void OnCollisionStay2D(Collision2D col)
    {
        Rigidbody2D rB  = col.collider.GetComponent<Rigidbody2D>();

        if (rB != null && rB.position.y > gameObject.transform.position.y)
        {   
            Vector2 conveyorForce = transform.right * conveyorSpeed; // Direção da movimentação

            rB.AddForce(conveyorForce, ForceMode2D.Force);
        }
    }
}
