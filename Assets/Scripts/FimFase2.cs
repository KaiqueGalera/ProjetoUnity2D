using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FimFase2 : MonoBehaviour
{
    private NPC             _NPC;
    private AudioController _AudioController;
    public  Collider2D col;
    public  bool            isTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        _NPC             = FindAnyObjectByType(typeof(NPC)) as NPC;
        _AudioController = FindAnyObjectByType(typeof(AudioController)) as AudioController;
        col              = GetComponent<BoxCollider2D>();
        col.enabled      = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if(_NPC.isNPC2Q == true && _NPC.isNPC3Q == true && _NPC.isNPC4Q == true && _NPC.isNPC5Q == true && _NPC.isNPC6Q == true)
        // {
        //     col.enabled = true;
        //     Debug.Log("Ativando fim2");
        // }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && isTrigger == false)
        {
            _AudioController.ChangeScene("ResultadosJogadorTeste 1", true, _AudioController.gamePlayMusic);
            isTrigger = true;
            _AudioController.isFaseDocDone = true;
        }
    }
}
