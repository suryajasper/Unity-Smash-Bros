using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float speed;
    private float moveInput;
    private Rigidbody2D rb;
    private bool isRight;
    private int knockedtimes;
    [HideInInspector] public GameObject launchedBullet;
    [HideInInspector] public bool spacecontrols;
    private bool isGrounded;
    [HideInInspector] public float checkRadius = 0F;
    private Rigidbody2D rb2;
    [HideInInspector] public bool knockedBack;
    [HideInInspector] public float knockSpeed;
    private int extraJumps;
    private KeyCode left, right, jump, attack, down, longAttack, shield;
    private int direction;
    private int lastScore;
    [HideInInspector] public int score;
    private int bulletnum = 0;
    [HideInInspector] public bool downB = false;
    private bool firstRun;
    private float startCoolDown;
    private float startShieldCoolDown;
    private float shieldCoolDown;
    private bool shieldOn;
   
    [HideInInspector] public bool shouldShield;
    public float startSpeed;
    public Animator animator;
    public GameObject otherplayer;
    public GameObject shieldObject;
    public int knockback;
    public int jumpforce;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public Text scoreField;
    public int extraJumpsValue;
    public GameObject bullet;
    public float coolDownTime;
    [Header("Controls")]
    [Space]
    public string rightVal;
    public string leftVal;
    public string jumpVal;
    public string attackVal;
    public string downVal;
    public string longAttackVal;
    public string shieldVal;

    void Start()
    {
        startShieldCoolDown = Time.time;
        shieldCoolDown = Time.time;
        startCoolDown = Time.time;
        score = 0;
        lastScore = 0;
        if (leftVal.Equals("left"))
            left = KeyCode.LeftArrow;
        if (leftVal.Equals("a"))
            left = KeyCode.A;
        if (rightVal.Equals("right"))
            right = KeyCode.RightArrow;
        if (rightVal.Equals("d"))
            right = KeyCode.D;
        if (jumpVal.Equals("up"))
            jump = KeyCode.UpArrow;
        if (jumpVal.Equals("w"))
            jump = KeyCode.W;
        if (attackVal.Equals("lshift"))
            attack = KeyCode.LeftShift;
        if (attackVal.Equals("rshift"))
            attack = KeyCode.RightShift;
        if (longAttackVal.Equals("lalt"))
            longAttack = KeyCode.LeftAlt;
        if (longAttackVal.Equals("ralt"))
            longAttack = KeyCode.RightAlt;
        if (downVal.Equals("down"))
            down = KeyCode.DownArrow;
        if (downVal.Equals("s"))
            down = KeyCode.S;
        if (shieldVal.Equals("q"))
            shield = KeyCode.Q;
        if (shieldVal.Equals("/"))
            shield = KeyCode.Slash;

        direction = 1;
        //Physics2D.IgnoreLayerCollision(8,8);
        speed = startSpeed;
        rb = GetComponent<Rigidbody2D>();
        rb2 = otherplayer.GetComponent<Rigidbody2D>();
        moveInput = 0;
    }
    void Update()
    {
        if (Input.GetKey(down)) {
            rb.velocity = new Vector2(0F, -25F);
            if (Input.GetKey(attack)) {
                animator.SetBool("downB", true);
                if ((transform.position.y - otherplayer.transform.position.y <= 0.5) &&
                (Mathf.Abs(transform.position.x - otherplayer.transform.position.x) <= 1) && 
                    (transform.position.y - otherplayer.transform.position.y > 0) && (otherplayer.GetComponent<PlayerController>().shouldShield == false))
                    score += 8;
            }
        }
        if (score > lastScore) {
            if (score > 1000)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            otherplayer.GetComponent<Rigidbody2D>().mass -= 0.01F;
            scoreField.text = score + "%";
            lastScore = score;
        }
        if ((Mathf.Abs(transform.position.x - otherplayer.transform.position.x) + Mathf.Abs(transform.position.y - otherplayer.transform.position.y) <= 1.5) && Input.GetKeyDown(attack) && (otherplayer.GetComponent<PlayerController>().shouldShield == false)) {
            animator.SetBool("isAttacking", true);
            otherplayer.GetComponent<PlayerController>().knockedBack = true;
            if (moveInput == 0F) {
                otherplayer.GetComponent<PlayerController>().knockSpeed = direction * 400;
                score += 5;
            } else {
                otherplayer.GetComponent<PlayerController>().knockSpeed = moveInput * 600;
                score += 7;
            }
        }
        if (Input.GetKey(shield) && (Time.time - shieldCoolDown > 3))
        {
            if (!shieldOn)
                startShieldCoolDown = Time.time;
            if (Time.time - startShieldCoolDown <= 3)
            {
                shouldShield = true;
                shieldObject.SetActive(true);
                shieldOn = true;
            }
            else
            {
                shieldObject.SetActive(false);
                shouldShield = false;
            }

        }
        if (Input.GetKeyUp(shield))
        {
            shieldCoolDown = Time.time;
            shieldOn = false;
            shieldObject.SetActive(false);
            shouldShield = false;
        }
        if (Input.GetKeyDown(longAttack) && (Time.time - startCoolDown > coolDownTime))
        {
            bulletnum++;
            Instantiate(bullet);
            bullet.GetComponent<bulletScript>().direction = direction;
            bullet.GetComponent<bulletScript>().launchedObject = gameObject;
            bullet.transform.position = transform.position;
            Vector3 scale = bullet.transform.localScale;
            scale.x *= direction*-1;
            bullet.transform.localScale = scale;
            startCoolDown = Time.time;
        }
        animator.SetBool("isAttacking", false);
        if (knockedBack) {
            rb.AddForce(new Vector2(knockSpeed, 0));
            knockedtimes++;
            if (knockedtimes >= knockback) {
                knockedBack = false;
                knockedtimes = 0;
            }
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetKey(left) || Input.GetKey(right))
            speed += 0.01F;
        if (Input.GetKeyDown(left))
            moveInput = -1;
        if (Input.GetKeyDown(right))
            moveInput = 1;
        if (Input.GetKeyUp(left) || Input.GetKeyUp(right)){
            moveInput = 0;
            speed = startSpeed;
        }
        animator.SetFloat("speed", Mathf.Abs(moveInput));
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (isGrounded) {
            animator.SetBool("isJumping", false);
            extraJumps = extraJumpsValue;
            animator.SetBool("downB", false);
        }
        if (Input.GetKeyDown(jump) && extraJumps > 0) {
            animator.SetBool("isJumping", true);
            rb.velocity = Vector2.up * jumpforce;
            extraJumps--;
            if ((Input.GetKey(attack)) && (otherplayer.transform.position.y - transform.position.y <= 0.5) &&
             (Mathf.Abs(transform.position.x - otherplayer.transform.position.x) <= 1) && (transform.position.y - otherplayer.transform.position.y > 0))
                score += 15;
        } else if (Input.GetKeyDown(jump) && extraJumps == 0 && isGrounded == true) {
            animator.SetBool("isJumping", true);
            rb.velocity = Vector2.up * jumpforce;
        }
    }
    void FixedUpdate()
    {
        if (isRight && moveInput > 0)
            Flip();
        else if (!isRight && moveInput < 0)
            Flip();
    }
    void Flip()
    {
        direction *= -1;
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
