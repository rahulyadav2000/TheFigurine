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

    private float speed = 4f;
    [SerializeField] private float hitRange;

    public HealthSystem enemyHealth;
    public HealthSystem playerHealth;

    private bool canShoot = true;
    private bool isRunning = false;
    private bool isDead = false;


    private float shootCooldown = 0.5f;

    public Camera cam;
    public GameObject handArrow;
    public GameObject arrowPrefab;
    public GameObject loseSceneScreen;
    public Transform arrowPos;
    private Transform camTranform;

    private Spawner spawner;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        loseSceneScreen.SetActive(false);
        movement = inputs.actions["Movement"];
        attack = inputs.actions["Attack"];
        run = inputs.actions["Run"];
        aiming = inputs.actions["Aiming"];
        shooting = inputs.actions["Shooting"];

        camTranform = Camera.main.transform;
        
        
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
        Invoke("TimeManager", 2.5f);

        if(isFigCollected)
        {
            figurineCount++;
            GameData.figurineAmount = figurineCount;
            isFigCollected = false;
        }
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
                speed = 0.0f;
                characterController.Move(Vector3.zero);
            }

            if (aiming.IsPressed() && arrow.GetArrowAmount() > 0)
            {
                animator.SetBool("aiming", true);
            }
            else
            {
                animator.SetBool("aiming", false);
            }

            if (shooting.IsPressed())
            {
                animator.SetBool("shooting", true);
                Invoke("ShootArrow", 0.5f);
            }
            else
            {
                animator.SetBool("shooting", false);
            }

            characterController.Move(moveInput * speed * Time.deltaTime);

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

    public void TimeManager()
    {
        if(isDead)
        {
            Time.timeScale = 0.0f;
            loseSceneScreen.SetActive(true);
        }
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
        if(other.gameObject.tag == "Ammo")
        {
            Debug.Log("Working!");
            arrow.IncreaseArrowAmount(10);
        }

        if(other.gameObject.tag == "EnemySpawner")
        {
            spawner.WandererSpawner();
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
    }
}