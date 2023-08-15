using System.Collections;
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
    public ArrowSystem arrow;
    public AudioSource source;

    private Vector3 moveInput;
    private Vector2 move;
    private InputAction movement;
    private InputAction attack;
    private InputAction run;
    private InputAction aiming;
    private InputAction shooting;
    private InputAction pickup;
    private InputAction inventoryKey;
    private InputAction pauseMenu;

    private float speed = 4f;
    [SerializeField] private float hitRange;

    public HealthSystem playerHealth;

    private bool canShoot = true;
    private bool isRunning = false;
    public bool isDead = false;
    private bool isAiming = false;
    public bool isInventoryActive { get; set; } = false;
    public bool isPauseMenuActive { get; set; } = false;
    public bool isGameObj { get; set; } = false;
    public bool isFinalFigurineCollected = false;

    private float shootCooldown = 0.5f;

    public Camera cam;
    public GameObject handArrow;
    public GameObject arrowPrefab;
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
        movement = inputs.actions["Movement"];
        attack = inputs.actions["Attack"];
        run = inputs.actions["Run"];
        aiming = inputs.actions["Aiming"];
        shooting = inputs.actions["Shooting"];
        pickup = inputs.actions["Pickup"];
        inventoryKey = inputs.actions["Inventory"];
        pauseMenu = inputs.actions["PauseMenu"];


        camTranform = Camera.main.transform;
        
        knifePrefab.SetActive(false);

        arrowAmountText.text = "Ammo: " + GameData.arrow.ToString();

        spawner = Spawner.instance;

    }

    // Update is called once per frame
    void Update()
    {
        PlayerBehaviour();
        HealthResponse();
        arrowAmountText.text = "Ammo: " + GameData.arrow.ToString();
        
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
                //source.loop = true;
            }
            else
            {
                animator.SetBool("run", false);
                speed = 3f;
                isRunning = false;
                //source.loop = false;
            }

            if (attack.IsPressed())
            {
                int attackAnim = Animator.StringToHash("PunchAttack");
                animator.CrossFade(attackAnim, 0.08f);
                knifePrefab.SetActive(true);
                speed = 0.0f;
                characterController.Move(Vector3.zero);
                Invoke(nameof(KnifeActivator), 1.5f);
                source.PlayOneShot(AudioManager.instance.audioClip[3]);

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

            if(pauseMenu.IsPressed())
            {
                isPauseMenuActive = !isPauseMenuActive;
                GameManager.Instance.TogglePauseMenu(isPauseMenuActive);
            }


            characterController.Move(moveInput * speed * Time.deltaTime);

            RaycastHit hit;

            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 2f))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                {
                    Vector3 newPosition = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                    characterController.Move(newPosition - transform.position);
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
            Invoke(nameof(LoseScreenLoader), 2.5f);
        }

        healthBar.fillAmount = GameData.health / 100;
    }


    public void LoseScreenLoader()
    {
        GameManager.Instance.LoseScreenEnabler();
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
            source.PlayOneShot(AudioManager.instance.audioClip[1]);
            arrow.ReduceArrowAmount();
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
                //isGameObj = true;
                EnemySpawnerManager.Instance.isEnemySpanwer1 = true;
            }
        }

        if(other.gameObject.tag == "E1")
        {
            if (gameObject != null)
            {
                spawner.WandererSpawner();
                //isGameObj = true;
                EnemySpawnerManager.Instance.isEnemySpanwer1 = false;
            }
        }
        
        if(other.gameObject.tag == "E2")
        {
            if (gameObject != null)
            {
                spawner.WandererSpawner();
                //isGameObj = true;
                EnemySpawnerManager.Instance.isEnemySpanwer2 = true;
            }
        }
        
        if(other.gameObject.tag == "E3")
        {
            if (gameObject != null)
            {
                spawner.WandererSpawner();
                //isGameObj = true;
                EnemySpawnerManager.Instance.isEnemySpanwer2 = false;
            }
        }
        
        if(other.gameObject.tag == "E4")
        {
            if (gameObject != null)
            {
                spawner.WandererSpawner();
                EnemySpawnerManager.Instance.isEnemySpanwer3 = true;
            }
        }
        
        if(other.gameObject.tag == "E5")
        {
            if (gameObject != null)
            {
                spawner.WandererSpawner();
                EnemySpawnerManager.Instance.isEnemySpanwer4 = true;
            }
        }
        
        if(other.gameObject.tag == "E6")
        {
            if (gameObject != null)
            {
                spawner.WandererSpawner();
                EnemySpawnerManager.Instance.isEnemySpanwer4 = false;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Figurine"))
        {
            if (gameObject != null)
            {
                Spawner.instance.figurineIndex++;
                GameData.figurineAmount = Spawner.instance.figurineIndex;
                if (Spawner.instance.figurineIndex < Spawner.instance.figurines.Length)
                {
                    Destroy(hit.gameObject);
                    Spawner.instance.FigurineHandler();
                    source.PlayOneShot(AudioManager.instance.audioClip[13]);
                }

                if (Spawner.instance.figurineIndex >= 4)
                {
                    Destroy(hit.gameObject);
                    source.PlayOneShot(AudioManager.instance.audioClip[13]);
                }
            }
        }

        if(hit.gameObject.CompareTag("FF"))
        {
            if(gameObject != null)
            {
                isFinalFigurineCollected = true;
                Destroy(hit.gameObject);
            }
        }

        if (hit.gameObject.CompareTag("Ammo"))
        {
            if(pickup.IsPressed())
            {
                InventoryManager.instance.AddItem(logItem);
                source.PlayOneShot(AudioManager.instance.audioClip[4]);
                Destroy(hit.gameObject);
            }
        }
        
        if(hit.gameObject.CompareTag("Health"))
        {
            if(pickup.IsPressed())
            {
                InventoryManager.instance.AddItem(plantItem);
                source.PlayOneShot(AudioManager.instance.audioClip[2]);
                Destroy(hit.gameObject);
            }
        }
        
        if(hit.gameObject.CompareTag("Meat"))
        {
            InventoryManager.instance.AddItem(meatItem);
            source.PlayOneShot(AudioManager.instance.audioClip[2]); 
            Destroy(hit.gameObject);
        }
    }
}