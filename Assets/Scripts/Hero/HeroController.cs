using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroController : MonoBehaviour, IDataPersistence
{

    private GameController      _GameController;
    private Alarmobot           _Alarmobot;
    private LifeController      _LifeController;
    private LifeController      _NPCControl;
    private EqBoss              _EqBoss;
    private BossController      _BossController;

    [Header("Movimentação")]
    private Rigidbody2D         heroRB2D;
    private Transform           heroTr;
    public  bool                isLookingL; // Está olhando para a esquerda ?
    public  float               heroSpeedX;
    public  float               heroSpeedY;
    public  float               controlHz; // Controle Horizontal (-1/0/1) 

    [Header("Pulo")]
    public  Transform           groundCheckR;
    public  Transform           groundCheckL;
    public  LayerMask           ground;
    public  LayerMask           platformBoss;
    public  float               jumpForce;
    public  bool                isGround;
    

    [Header("Controle de vida")]
    public  Color               invencibleColor;
    public  int                 currentHealth;
    public  int                 maxHealth = 100;
    public  bool                isInvencible = false;
    private HealthBarUI         _HealthBarUI;
    private SpriteRenderer      heroSr;

    [Header("SFX")]
    private AudioController     _AudioController;

    [Header("Controle do Dialogo")]
    private DialogControl       _DialogControl;

    [Header("Sistema Raycast")]
    public  InteractableObject  _IntObj;
    private float               hAxis;
    public  Transform           raycastRadius;
    public  LayerMask           interactable;
    
    [Header("Animação")]
    private Animator            heroAnimator;
    private bool                isRunAnimation;
    private bool                isAnimationComplete;

    [Header("Atacar")]
    public int                  heroDmg = 20;
    public Collider2D[]         swordHitbox;
    
    [Header("Atirar")]
    public  GameObject          shootPrefab;
    public  Transform           shootPosition;
    public  float               shootForceX, shootForceY;
    public  float               delayShoot;
    public  bool                isShot;

    [Header("KnockBack")]
    public bool isKnockback = false;
    private float knockbackTime = 0.5f; // Duração do empurrão

    [Header("Nado")]
    public bool isInWater = false;
    public float swimGravityScale = 0.3f;
    public float swimUpForce = 7f;
    public float maxSwimVelocityY = 3f;
    public Transform waterCheck;
    public LayerMask waterLayer;
    public float waterCheckRadius = 0.1f;


    // Start is called before the first frame update
    void Start()
    {   
        _AudioController  =   FindAnyObjectByType(typeof(AudioController)) as AudioController;
        _DialogControl    =   FindAnyObjectByType(typeof(DialogControl)) as DialogControl;
        _GameController   =   FindAnyObjectByType(typeof(GameController)) as GameController;
        _HealthBarUI      =   FindAnyObjectByType(typeof(HealthBarUI)) as HealthBarUI;
        _LifeController   =   FindAnyObjectByType(typeof(LifeController)) as LifeController;
        _Alarmobot        =   FindAnyObjectByType(typeof(Alarmobot)) as Alarmobot;
        _EqBoss           =   FindAnyObjectByType(typeof(EqBoss)) as EqBoss;
        _BossController   =   FindAnyObjectByType(typeof(BossController)) as BossController;

        heroRB2D          =   GetComponent<Rigidbody2D>();
        heroTr            =   GetComponent<Transform>();
        heroSr            =   GetComponent<SpriteRenderer>();
        heroAnimator      =   GetComponent<Animator>();

        foreach (GameObject visualAlert in _DialogControl.visualAlert)
        {
            visualAlert.SetActive(false);
        }

        foreach (Collider2D colliders in swordHitbox)
        {
            colliders.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        InteractionControll(); 

        if (GameStateManager.Instance.CurrentState == GameState.Playing)
        {
            HeroMov();
        }

        if (waterCheck != null && isInWater && GameStateManager.Instance.CurrentState == GameState.Playing)
        {
            // Reduz a gravidade enquanto estiver na água
            heroRB2D.gravityScale = swimGravityScale;

            // Se o jogador apertar o botão de pulo, aplica impulso para cima
            if (Input.GetButtonDown("Jump"))
            {
                heroRB2D.velocity = new Vector2(heroRB2D.velocity.x, 0); // Zera velocidade vertical para não acumular
                heroRB2D.AddForce(new Vector2(0, swimUpForce), ForceMode2D.Impulse);
            }

            // Limita a velocidade máxima de subida
            if (heroRB2D.velocity.y > maxSwimVelocityY)
            {
                heroRB2D.velocity = new Vector2(heroRB2D.velocity.x, maxSwimVelocityY);
            }
        }
        else
        {
            // Se estiver fora da água, volta a gravidade normal
            heroRB2D.gravityScale = 1.8f;
        }

        if (Input.GetButtonDown("Jump") && isGround && GameStateManager.Instance.CurrentState == GameState.Playing)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            print ("FlowMacroDescriptor");
            _LifeController.heroDeaths -= 1;

        }

        if (Input.GetButtonDown("Fire2") && GameStateManager.Instance.CurrentState == GameState.Playing && _GameController.shootAmm > 0 && isShot == false && isAnimationComplete == false)
        {
            Shoot();
        }
        if (Input.GetButtonDown("Fire1") && GameStateManager.Instance.CurrentState == GameState.Playing && isAnimationComplete == false)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        isGround = Physics2D.OverlapArea(groundCheckL.position, groundCheckR.position, ground); // Faz uma área através da posição de 2 transformadas para checar a layerMask fornecida (output true or false)
        if (waterCheck != null)
        {
            isInWater = Physics2D.OverlapCircle(waterCheck.position, waterCheckRadius, waterLayer);
        }
        else
        {
            isInWater = false;
        }    
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        isKnockback = true;
        heroRB2D.velocity = Vector2.zero;
        heroRB2D.AddForce(direction * force, ForceMode2D.Impulse);
        StartCoroutine(EndKnockback(knockbackTime));
    }

    IEnumerator EndKnockback(float duration)
    {
        yield return new WaitForSeconds(duration);
        isKnockback = false;
    }

    void HeroMov() // Responsável pela movimentação
    {
        if (isKnockback) return;
        
        controlHz = Input.GetAxisRaw("Horizontal"); // esq -1 / 0 / dir 1
       
        heroSpeedY = heroRB2D.velocity.y;
        heroRB2D.velocity = new Vector2(heroSpeedX * controlHz, heroSpeedY);
        
        
        if (controlHz != 0)
        {
            hAxis = controlHz;
        }

        if (isLookingL == true && controlHz > 0 )
        {
            Flip();
        } 
        else if (isLookingL == false && controlHz < 0 )
        {
            Flip();
        }

        if (controlHz != 0) {isRunAnimation = true;} else {isRunAnimation = false;} // Controle para animação da corrida
    }

    void Flip() // Altera o scale para trocar o lado do personagem
    {
        isLookingL = !isLookingL;

        float ScaleX = heroTr.localScale.x;

        heroTr.localScale = new Vector3(ScaleX * - 1, heroTr.localScale.y, heroTr.localScale.z);
        
        shootForceX *= -1;
    }

    void Jump()
    {       
            heroRB2D.velocity = new Vector2(heroRB2D.velocity.x, 0); // 0 a Vel no y para previnir pulo maior no caso de multiplos registros do Jump
            heroRB2D.AddForce(new Vector2(0, jumpForce));
            _AudioController.sJump.PlayOneShot(_AudioController.jump);

    }

    void InteractionControll()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastRadius.position, new Vector2(hAxis, 0), 0.40f, interactable); // Sistema usado para criar uma linha de hit (aula 39)
        Debug.DrawRay(raycastRadius.position, new Vector2(hAxis, 0) * 0.40f, Color.red);

        if(hit == true ) // Caso colida
        {
            if(_IntObj == null) // A variável seja vazia
            {
                _IntObj = hit.transform.gameObject.GetComponent<InteractableObject>(); // Atribui a variável _Interação (do tipo script interação), o script interação associado ao obj colidido
                
                
                switch(_IntObj.idInteractiveObj)
                {
                    case 0:
                    // if(isHitPorta == false)
                    // {
                    //     PortaController temp = hit.transform.gameObject.GetComponent<PortaController>(); // Obj temporário que armazena o script portacontroller
                    //     isHitPorta = true;
                    
                    //     Teletransporte(temp.saida); // Chama função e passa como argumento o script temporário, cujo contém somente uma variável "saída" do tipo Transform
                    // }
                        break;

                    case 1:
                        if (_IntObj.name == "NPC1")
                        {
                            _DialogControl.visualAlert[0].SetActive(true);
                        } 
                        else if(_IntObj.name == "NPC2")
                        {
                            _DialogControl.visualAlert[1].SetActive(true);
                        }
                        else if(_IntObj.name == "NPC3")
                        {
                            _DialogControl.visualAlert[2].SetActive(true);
                        }
                        else if(_IntObj.name == "NPC4")
                        {
                            _DialogControl.visualAlert[3].SetActive(true);
                        }
                        else if(_IntObj.name == "NPC5")
                        {
                            _DialogControl.visualAlert[4].SetActive(true);
                        }
                        else if(_IntObj.name == "NPC6")
                        {
                            _DialogControl.visualAlert[5].SetActive(true);
                        }
                        break;

                }
            }

        } else
        {
            _IntObj = null;

            foreach (GameObject visualAlert in _DialogControl.visualAlert)
            {
                visualAlert.SetActive(false);
            }
        }

        if (Input.GetButtonDown("Fire3") && _IntObj != null && _DialogControl.panelDialog.activeSelf == false && _DialogControl.panelQuestions.activeSelf == false && _IntObj.idInteractiveObj == 1)
        {   
            _DialogControl.gNPC = _IntObj.gameObject;
            _IntObj.gameObject.SendMessage("StartConversation", SendMessageOptions.DontRequireReceiver);
            
        } 
        else if (Input.GetButtonDown("Fire3") &&  _DialogControl.panelDialog.activeSelf == true && _IntObj != null)//  && _DialogControl.isTyping == false)
        {
            _DialogControl.NextLine();
        }
        else if (Input.GetButtonDown("Fire3") &&  _DialogControl.panelDialog.activeSelf == true && _EqBoss.isDialogOn == true)//  && _DialogControl.isTyping == false)
        {
            _DialogControl.NextLine();
        }
    }

    void Shoot()
    {   
        isShot = true;
        
        heroAnimator.SetTrigger("Attack2");
        
        StartCoroutine("waitToShot");
        
        _GameController.AmmManager(-1);

    }

    void ShootAnimator()
    {
        
        GameObject temp = Instantiate(shootPrefab);
        temp.transform.position = shootPosition.position;
        temp.GetComponent<Rigidbody2D>().AddForce(new Vector2(shootForceX, shootForceY));

        Destroy(temp, 1.0f);

    }
   
    void Attack()
    {
        isAnimationComplete = true;
        heroAnimator.SetTrigger("Attack1");
        _AudioController.sDmgHeroFx.PlayOneShot(_AudioController.dmgHeroFx); 
    }
  
    public void OnAttackComplete() // Função que é utilizada nos eventos da animação para assegurar que não tenha bug de repetir animação toda vez que pressionado
    {
        swordHitbox[0].enabled   = false;
        swordHitbox[1].enabled   = false;
        isAnimationComplete = false;
        if (_EqBoss != null)
        {
            _EqBoss.CanAttackAgain();
        }
    }
  
    public void EnableFirstCollider() // Função que é utilizada nos eventos da animação para assegurar que não tenha bug de repetir animação toda vez que pressionado
    {
        swordHitbox[0].enabled   = true;
    }
 
    public void EnableSecondCollider() // Função que é utilizada nos eventos da animação para assegurar que não tenha bug de repetir animação toda vez que pressionado
    {
        swordHitbox[1].enabled   = true;
    }

    public IEnumerator BeInvencible()
    {   
        isInvencible = true;

        heroSr.color = invencibleColor;
        
        StartCoroutine("BlinkInvencible");

        yield return new WaitForSeconds(_GameController.invulTime);

        heroSr.color = Color.white;

        isInvencible = false;

        heroSr.enabled = true;
        
        StopCoroutine("BlinkInvencible");
        
    }

    public IEnumerator BlinkInvencible() // Função assincrona para 
    {
        
        yield return new WaitForSeconds(_GameController.blinkTime);

        heroSr.enabled = !heroSr.enabled;
            
        StartCoroutine("BlinkInvencible");

    }
    
    public IEnumerator waitToShot()
    {
        yield return new WaitForSeconds(delayShoot);

        isShot = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "MouthTrap":
                _LifeController.HeroDmgControl(20);
                
                break;

            case "ExplosionTrap":
                _LifeController.HeroDmgControl(20);
                
                break;

            case "Platform":
                if (groundCheckR.position.y > col.transform.position.y)
                {   
                     transform.parent = col.transform;
                }
                
                break;

            case "Enemy":
                EnemyLifeControl _TempLifeController = col.gameObject.GetComponent<EnemyLifeControl>();

                switch (_TempLifeController.enemyName)
                {
                    case"Flymachine":
                        _LifeController.HeroDmgControl(10);

                        break;
                }

                break;

            case "Boss10":
                if(!_BossController.isDead)
                {
                    _LifeController.HeroDmgControl(_GameController.Attack1);
                    Debug.Log("Ataque 1");
                }
                break;

            // case "Boss11":
            //     _LifeController.HeroDmgControl(_GameController.Attack2);
            //     Debug.Log("Ataque 2");

            //     break;

            // case "Boss12":
            //     _LifeController.HeroDmgControl(_GameController.Attack3);
                
            //     break;

            // case "Boss13":
            //     _LifeController.HeroDmgControl(_GameController.Attack4);
                
            //     break;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Platform":
                transform.parent = null;
            break;
        }
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.gameObject.tag)
        {
            case "ShootG":
                _LifeController.HeroDmgControl(10);
                break;

            case"FlymachineElectricity":
                _LifeController.HeroDmgControl(25);
                break;

            case"Colectable":
                _GameController.AmmManager(1);
                Destroy(col.gameObject);
                break;
            case"speed_c":
                jumpForce += 15;
                Destroy(col.gameObject);
                break;
            case"AlarmTrigger":
                _Alarmobot.TriggerAlarm();
                break;
        }
    }

    public void LoadData(GameData data)
    {
        // transform.position = data.heroPosition;
    }
    
    public void SaveData(GameData data)
    {
        // if (gameObject != null)
        // {
        //     data.heroPosition = transform.position;
        // }
    }
    
    private void LateUpdate()
    {
        heroAnimator.SetBool("isRunning", isRunAnimation);
        heroAnimator.SetBool("isGround", isGround);
        heroAnimator.SetBool("isAnimationComplete", isAnimationComplete);
        heroAnimator.SetFloat("heroSpeedY", heroSpeedY);
    }
}
