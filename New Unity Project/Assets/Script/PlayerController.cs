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
    bool leftDash, rightDash;
    bool downbtn = false;

    bool parryAttack = false;
    bool parryButton, jumpButton = false;
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

        //crouch animation
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            downbtn = true;
        }

        if (downbtn == true)
        {
            animator.SetInteger("States", 5);

        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            downbtn = false;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            animator.SetInteger("States", 3);
        }

        //Jump
        if ((Input.GetKeyDown(KeyCode.Space) || jumpButton) && touchedGround && !downbtn)
        {
            Jump();
            jumpButton = false;
            animator.SetInteger("States", 4);
        }
        //change animation        
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetInteger("States", 3);
        }
        //Jump
        if ((Input.GetKeyDown(KeyCode.Space) || jumpButton) && !touchedGround && !doubleJump)
        {
            Jump();
            jumpButton = false;
            doubleJump = true;
        }

        //Move Left without dash
        if (Input.GetKey(KeyCode.LeftArrow) && !downbtn)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed, GetComponent<Rigidbody2D>().velocity.y);
            Debug.Log("Left" + movementSpeed);
            animator.SetInteger("States", 1);
            facingright = false;
            facingleft = true;
        }
        //Move Right without dash
        if (Input.GetKey(KeyCode.RightArrow) && !downbtn)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, GetComponent<Rigidbody2D>().velocity.y);
            facingright = true;
            facingleft = false;
            Debug.Log("Right" + movementSpeed);
            animator.SetInteger("States", 2);
        }
        //dash up
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            Debug.Log("dash up");
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movementSpeed*1.5f);
            dashCountdown--;
            invincible = true;
        }
        //dash left while idle
       else if (facingleft == true && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            Debug.Log("dash left idle");
            dashCountdown--;
            invincible = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed * 1.5f, GetComponent<Rigidbody2D>().velocity.y);
        }
        //dash right while idle
        else if (facingright == true && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            Debug.Log("dash right idle");
            dashCountdown--;
            invincible = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed*1.5f, GetComponent<Rigidbody2D>().velocity.y);
        }

        //dash upright
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            Debug.Log("dash upright");
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movementSpeed * 2);
            dashCountdown--;
            invincible = true;
        }
        //Move Right with dash
        else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            rightDash = true;
            Dash();
            invincible = true;
        }
        //dash upleft
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow) && dashCountdown > 0)
        {
            Debug.Log("dash upleft");
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movementSpeed * 2);
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

        healthBar.transform.localScale = new Vector3(health / totalHealth, 1, 1);
        manaBar.transform.localScale = new Vector3(mana / totalMana, 1, 1);

        //health -= 1;
        if (health <= 0)
        {
            SceneManager.LoadScene("GameOver");
            health = 0;
        }
        
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

    //Jump movement
    void Jump()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
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

    public void ParryButton()
    {
        parryButton = true;
    }

    public void JumpButton()
    {
        jumpButton = true;
    }
}
