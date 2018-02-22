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
    bool leftDash, rightDash;
    public bool downbtn = false;
    bool leftUpDash, rightUpDash, leftIdleDash, rightIdleDash;

    bool parryAttack = false;
    bool parryButton, jumpButton, dashButton = false;
    float parryTimer = 0;
    bool facingleft = false;
    bool facingright = true;
    float parryCooldown = 0.3f;
    public Collider2D attackTrigger;
    public Renderer attackVisual;

    [SerializeField]
    GameObject healthBar, manaBar;
    [SerializeField]
    MainGame mainGame;

    [SerializeField]
    ScreenShake screenShake;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        movementSpeed = 5;
        jumpHeight = 5;
        dashCountdown = 7.0f;
        mana = 0;
        health = 100;
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

        //Crouch animation
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //screenShake.ShakeCamera(1.0f, 0.3f, 0.95f);
            screenShake.ShakeCamera();
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

        //dash up
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            Debug.Log("dash up");
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movementSpeed * 1.5f);
            dashCountdown--;
            invincible = true;
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
    }

    //Dash movement
    void Dash()
    {
        if (leftDash)
        {
            //Left
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed * 10, GetComponent<Rigidbody2D>().velocity.y);
            dashCountdown--;
            Debug.Log("LeftDash" + movementSpeed);
            leftDash = false;
        }
        if (rightDash)
        {
            //Right
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed * 10, GetComponent<Rigidbody2D>().velocity.y);
            dashCountdown--;
            Debug.Log("RightDash" + movementSpeed);
            rightDash = false;
        }
        if (leftUpDash)
        {
            Debug.Log("dash upright");
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movementSpeed * 2);
            dashCountdown--;
            invincible = true;
            leftUpDash = false;
        }
        if (rightUpDash)
        {
            Debug.Log("dash upright");
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movementSpeed * 2);
            dashCountdown--;
            invincible = true;
            rightUpDash = false;
        }
        if (leftIdleDash)
        {
            Debug.Log("dash left idle");
            dashCountdown--;
            invincible = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed * 1.5f, GetComponent<Rigidbody2D>().velocity.y);
            leftIdleDash = false;
        }
        if (rightIdleDash)
        {
            Debug.Log("dash right idle");
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

        //Move Left
        if ((mainGame.direction.x < 0 || Input.GetKey(KeyCode.LeftArrow)) && !downbtn)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed, GetComponent<Rigidbody2D>().velocity.y);
            Debug.Log("Left" + movementSpeed);
            animator.SetInteger("States", 1);
            facingright = false;
            facingleft = true;
        }
        //Move Left with dash
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            leftDash = true;
            Dash();
            invincible = true;
        }

        //Move Right
        if ((mainGame.direction.x > 0 || Input.GetKey(KeyCode.RightArrow)) && !downbtn)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, GetComponent<Rigidbody2D>().velocity.y);
            facingright = true;
            facingleft = false;
            Debug.Log("Right" + movementSpeed);
            animator.SetInteger("States", 2);
        }
        //Move Right with dash
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            rightDash = true;
            Dash();
            invincible = true;
        }

        //dash upright
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            rightUpDash = true;
            Dash();
        }

        //dash upleft
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow) && dashCountdown > 0)
        {
            leftUpDash = true;
            Dash();
        }

        //dash left while idle
        if (facingleft == true && (Input.GetKey(KeyCode.LeftShift)||dashButton) && dashCountdown > 0)
        {
            leftIdleDash = true;
            dashButton = false;
            Dash();
        }
        //dash right while idle
        if (facingright == true && (Input.GetKey(KeyCode.LeftShift) || dashButton) && dashCountdown > 0)
        {
            rightIdleDash = true;
            dashButton = false;
            Dash();
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
