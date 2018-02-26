using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    private Animator animator;

    public float movementSpeed;
    public float jumpHeight;

    public Transform checkGround;
    public float groundCheckRadius;
    public LayerMask isGround;
    private bool touchedGround;
    private bool doubleJump;
    float totalHealth = 100;
    public float health;
    float totalMana = 100;
    public float mana;
    float dashCountdown;
    float regainDash = 0;
    public bool invincible = false;
    public bool invincible2 = false;
    float invinciblelifetime = 0;
    bool leftDash, rightDash, leftUpDash, rightUpDash, leftIdleDash, rightIdleDash;
    bool downbtn = false;

    bool parryAttack = false;
    bool parryButton, jumpButton, dashButton = false;
    float parryTimer = 0;
    bool facingleft = false;
    bool facingright = true;
    float parryCooldown = 0.3f;
    public Collider2D attackTrigger;
    public Renderer attackVisual;

    Vector3 lastPosition, currentPosition;

    [SerializeField]
    GameObject healthBar, manaBar;
    [SerializeField]
    MainGame mainGame;

    public ScreenShake screenShake;
    public Dictionary<int, double> _keys = new Dictionary<int, double>();

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        movementSpeed = 5;
        jumpHeight = 5;
        dashCountdown = 7.0f;
        mana = 0;
        health = 100;

        for (int i = 1; i < 5; ++i)
        {
            _keys.Add(i, -1.0);

            // 1 - Up
            // 2 - Right
            // 3 - Down
            // 4 - Left
        }
    }

    void FixedUpdate()
    {
        touchedGround = Physics2D.OverlapCircle(checkGround.position, groundCheckRadius, isGround);
    }

    // Update is called once per frame
    void Update()
    {
        if (touchedGround)
        {
            doubleJump = false;
            animator.SetInteger("States", 3);
            currentPosition = transform.position;
        }
        if (invincible2 == true)
        {
            invinciblelifetime += Time.deltaTime;
        }
        if (invinciblelifetime >= 5.0f)
        {
            Debug.Log("INVINCIBILITY GONE");
            invincible2 = false;
            invinciblelifetime = 0;
        }
        
        // For Dictionary
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _keys[1] = 0.3f;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _keys[2] = 0.3f;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _keys[3] = 0.3f;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _keys[4] = 0.3f;
        }

        //Crouch animation
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //screenShake.ShakeCamera(1.0f, 0.3f, 0.95f);
            //screenShake.ShakeCamera();
            downbtn = true;
        }

        //Crouch animation
        if (Input.GetKey(KeyCode.DownArrow))
        {
            downbtn = true;
            if (downbtn)
            {
                animator.SetInteger("States", 5);
            }
        }
        else
        {
            downbtn = false;
            animator.SetInteger("States", 3);
        }

        //change animation to idle state      
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetInteger("States", 3);
        }

        healthBar.transform.localScale = new Vector3(health / totalHealth, 1, 1);
        manaBar.transform.localScale = new Vector3(mana / totalMana, 1, 1);

        //health -= 1;
        if (health <= 0)
        {
            SceneManager.LoadScene("GameOver");
            health = 0;
        }

        Movement();
        Jump();
        ParryAttack();
        UpdateKeys();

        if (mana <= 0)
            mana = 0;
        if (mana >= 100)
            mana = totalMana;

        if (dashCountdown == 0)
        {
            regainDash += Time.deltaTime;
            invincible = false;
        }
        if (regainDash >= 2)
        {
            dashCountdown = 5;
            regainDash = 0;
        }

        //Player Fall off screen, Reset back to previous position
        lastPosition = currentPosition;
        if (!touchedGround && transform.position.y <= -5)
        {
            transform.position = lastPosition;
            health -= 5;
        }
    }

    //Dash movement
    void Dash()
    {
        if (leftDash)
        {
            //Left
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed * 10, GetComponent<Rigidbody2D>().velocity.y);
            dashCountdown--;
            //Debug.Log("LeftDash" + movementSpeed);
            leftDash = false;
            invincible = true;
        }
        if (rightDash)
        {
            //Right
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed * 10, GetComponent<Rigidbody2D>().velocity.y);
            dashCountdown--;
           // Debug.Log("RightDash" + movementSpeed);
            rightDash = false;
            invincible = true;
        }
        if (leftUpDash)
        {
            //Debug.Log("dash upright");
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movementSpeed * 2);
            dashCountdown--;
            invincible = true;
            leftUpDash = false;
        }
        if (rightUpDash)
        {
            //Debug.Log("dash upright");
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movementSpeed * 2);
            dashCountdown--;
            invincible = true;
            rightUpDash = false;
        }
        if (leftIdleDash)
        {
            //Debug.Log("dash left idle");
            dashCountdown--;
            invincible = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed * 1.5f, GetComponent<Rigidbody2D>().velocity.y);
            leftIdleDash = false;
        }
        if (rightIdleDash)
        {
            //Debug.Log("dash right idle");
            dashCountdown--;
            invincible = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed * 1.5f, GetComponent<Rigidbody2D>().velocity.y);
            rightIdleDash = false;
        }
    }

    //Movement
    void Movement()
    {
        //if (mainGame.direction.x > 0)
        //{
        //    //go right
        //}
        //if (mainGame.direction.x < 0)
        //{
        //    //go left
        //} 
        //dash upleft
         if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow) && dashCountdown > 0)
        {
            leftUpDash = true;
            Dash();
        }
        //dash upright
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            rightUpDash = true;
            Dash();
        }
        //dash up
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            Debug.Log("dash up");
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movementSpeed * 1.5f);
            dashCountdown--;
            invincible = true;
        }
        //Move Left with dash
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            leftDash = true;
            Dash();
            invincible = true;
        }
        //Move Right with dash
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow) && dashCountdown > 0)
        {
            rightDash = true;
            Dash();
            invincible = true;
        }

        //dash right while idle
        else if (facingright == true && (Input.GetKey(KeyCode.LeftShift) || dashButton) && dashCountdown > 0)
        {
            rightIdleDash = true;
            dashButton = false;
            Dash();
        }

        //dash left while idle
        else if (facingleft == true && (Input.GetKey(KeyCode.LeftShift) || dashButton) && dashCountdown > 0)
        {
            leftIdleDash = true;
            dashButton = false;
            Dash();
        }
        //Move Left
        else if ((mainGame.direction.x < 0 || Input.GetKey(KeyCode.LeftArrow)) && !downbtn)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed, GetComponent<Rigidbody2D>().velocity.y);
            //Debug.Log("Left" + movementSpeed);
            animator.SetInteger("States", 1);
            facingright = false;
            facingleft = true;
        }
        //Move Right
        else if ((mainGame.direction.x > 0 || Input.GetKey(KeyCode.RightArrow)) && !downbtn)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, GetComponent<Rigidbody2D>().velocity.y);
            facingright = true;
            facingleft = false;
            //Debug.Log("Right" + movementSpeed);
            animator.SetInteger("States", 2);
        }
    }

    //Jump movement
    void Jump()
    {
        //Jump
        if ((Input.GetKeyDown(KeyCode.Space) || jumpButton) && touchedGround && !downbtn)
        {

            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
            jumpButton = false;
            animator.SetInteger("States", 4);
        }
        //Double Jump
        if ((Input.GetKeyDown(KeyCode.Space) || jumpButton) && !touchedGround && !doubleJump)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
            jumpButton = false;
            doubleJump = true;
        }

    }

    void ParryAttack()
    {
        if ((Input.GetKeyDown(KeyCode.D) || parryButton) && !parryAttack)
        {
            parryAttack = true;
            parryButton = false;
            attackTrigger.enabled = true;
            attackVisual.enabled = true;
            parryTimer = parryCooldown;
            Debug.Log("Attack");
            animator.SetInteger("States", 6);
        }

        if (parryAttack)
        {
            if (parryTimer > 0)
            {
                parryTimer -= Time.deltaTime;
            }
            else
            {
                parryAttack = false;
                parryButton = false;
                attackTrigger.enabled = false;
                attackVisual.enabled = false;
            }
        }
    }

    void UpdateKeys()
    {
        for (int i = 1; i < 5; ++i)
        {
            if (_keys[i] > 0.0)
                _keys[i] -= Time.deltaTime;
            else
                _keys[i] = -1.0;
        }
    }


#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_ANDROID
    public void ParryButton()
    {
        parryButton = true;
    }

    public void JumpButton()
    {
        jumpButton = true;
    }

    public void DashButton()
    {
        dashButton = true;
    }
#endif
}
