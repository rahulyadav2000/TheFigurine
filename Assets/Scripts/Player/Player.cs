using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerInput inputs;
    public Animator animator;
    public CharacterController characterController;
    public Image healthBar;
    public TMP_Text arrowAmountText;

    public LayerMask ground;
    private RaycastHit hitInfo;

    [SerializeField] private int figurineCount;
    private bool isFigCollected = false; 

    public ArrowSystem arrow;

    private Vector3 moveInput;
    private Vector2 move;
    private InputAction movement;
    private InputAction attack;
    private InputAction run;
    private InputAction aiming;
    private InputAction shooting;
    private InputAction pickup;
    private InputAction inventoryKey;

    private float speed = 4f;
    [SerializeField] private float hitRange;

    public HealthSystem playerHealth;

    private bool canShoot = true;
    private bool isRunning = false;
    public bool isDead = false;
    private bool isAiming = false;
    public bool isInventoryActive { get; set; } = false;
    public bool isGameObj { get; set; } = false;

    private float shootCooldown = 0.5f;

    public Camera cam;
    public GameObject handArrow;
    public GameObject arrowPrefab;
    //public GameObject loseSceneScreen;
    public GameObject knifePrefab;

    public Transform arrowPos;
    private Transform camTranform;

    private Spawner spawner;

    public Item logItem;
    public Item plantItem;
    public Item meatItem;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //loseSceneScreen.SetActive(false);
        movement = inputs.actions["Movement"];
        attack = inputs.actions["Attack"];
        run = inputs.actions["Run"];
        aiming = inputs.actions["Aiming"];
        shooting = inputs.actions["Shooting"];
        pickup = inputs.actions["Pickup"];
        inventoryKey = inputs.actions["Inventory"];


        camTranform = Camera.main.transform;
        
        knifePrefab.SetActive(false);

        arrowAmountText.text = "Ammo: " + GameData.arrow.ToString();

        spawner = Spawner.instance;

        figurineCount = GameData.figurineAmount;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerBehaviour();
        HealthResponse();
        arrowAmountText.text = "Ammo: " + GameData.arrow.ToString();
        

        if(isFigCollected)
        {
            figurineCount++;
            GameData.figurineAmount = figurineCount;
            isFigCollected = false;
        }
    }

    private void LateUpdate()
    {
        
    }

    void PlayerBehaviour()
    {
        if (gameObject != null)
        {
            move = movement.ReadValue<Vector2>();
            moveInput = new Vector3(move.x, 0, move.y);
            moveInput = moveInput.x * camTranform.right.normalized + moveInput.z * camTranform.forward.normalized;
            moveInput.y = 0;

            if (run.IsPressed() && move.y > 0.6f)
            {
                isRunning = true;
                animator.SetBool("run", isRunning);
                speed = 6f;
            }
            else
            {
                animator.SetBool("run", false);
                speed = 3f;
            }

            if (attack.IsPressed())
            {
                int attackAnim = Animator.StringToHash("PunchAttack");
                animator.CrossFade(attackAnim, 0.08f);
                knifePrefab.SetActive(true);
                speed = 0.0f;
                characterController.Move(Vector3.zero);
                Invoke(nameof(KnifeActivator), 1.5f);
            }
          

            if (aiming.IsPressed() && arrow.GetArrowAmount() > 0)
            {
                animator.SetBool("aiming", true);
                isAiming = true;
            }
            else
            {
                animator.SetBool("aiming", false);
            }

            if (shooting.IsPressed() && isAiming)
            {
                animator.SetBool("shooting", true);
                Invoke(nameof(ShootArrow), 0.5f);
            }
            else
            {
                animator.SetBool("shooting", false);
            }

            if (inventoryKey.IsPressed())
            {
                isInventoryActive = !isInventoryActive;
                GameManager.Instance.ToggleInventorySystem(isInventoryActive);
            }
    

            characterController.Move(moveInput * speed * Time.deltaTime);
            //transform.position += Physics.gravity * Time.deltaTime; 

            if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 1.8f, ground))
            {
                if (Physics.OverlapSphere(transform.position, 0.5f, ground))
                {
                    transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * 1f, 5 * Time.deltaTime);
                    transform.position += Physics.gravity * Time.deltaTime;
                }
            }

            Quaternion targetRotaion = Quaternion.Euler(0, camTranform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotaion, 6.5f * Time.deltaTime);

            animator.SetFloat("moveX", move.x);
            animator.SetFloat("moveZ", move.y);
        }
    }

    void HealthResponse()
    {
        if(playerHealth.GetHealth() <= 0)
        {
            animator.SetBool("dead", true);
            isDead = true;
        }

        healthBar.fillAmount = GameData.health / 100;
    }

    public void KnifeActivator()
    {
        knifePrefab.SetActive(false);
    }


    private void ShootArrow()
    {
        if(!canShoot || arrow.GetArrowAmount() <= 0)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(arrowPos.transform.position, arrowPos.transform.forward, out hit, hitRange))
        {
            GameObject arrowSpawn = GameObject.Instantiate(arrowPrefab, arrowPos.position, arrowPos.rotation) as GameObject;
            arrowSpawn.GetComponent<Arrow>().setTarget(hit.point);
            arrow.ReduceArrowAmount();
            //GameData.arrow = arrow.GetArrowAmount();
            Debug.Log("Arrow Amount: " + arrow.GetArrowAmount());
            Destroy(arrowSpawn, 1.5f);

        }

        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(arrowPos.transform.position, arrowPos.transform.up * hitRange);
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "EnemySpawner")
        {
            if(gameObject != null)
            {
                spawner.WandererSpawner();
                isGameObj = true;
                EnemySpawnerManager.Instance.isEnemySpanwer1 = true;
            }
        }
        if(other.gameObject.tag == "E1")
        {
            if (gameObject != null)
            {
                spawner.WandererSpawner();
                isGameObj = true;
                EnemySpawnerManager.Instance.isEnemySpanwer1 = false;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag("Figurine"))
        {
            Debug.Log("Workinggg");
            Destroy(hit.gameObject);
            isFigCollected = true;
        }

        if(hit.gameObject.CompareTag("Ammo"))
        {
            if(pickup.IsPressed())
            {
                InventoryManager.instance.AddItem(logItem);
                //InventoryManager.instance.UpdatingItem();
                Destroy(hit.gameObject);
            }
        }
        
        if(hit.gameObject.CompareTag("Health"))
        {
            if(pickup.IsPressed())
            {
                InventoryManager.instance.AddItem(plantItem);
                //InventoryManager.instance.UpdatingItem();
                Destroy(hit.gameObject);
            }
        }
        
        if(hit.gameObject.CompareTag("Meat"))
        {
            InventoryManager.instance.AddItem(meatItem);
            //InventoryManager.instance.UpdatingItem();
            Destroy(hit.gameObject);
        }
    }
}