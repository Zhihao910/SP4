using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public float health = 100;
    float totalMana = 100;
    //float mana = 100;
    public float mana = 0;
    float dashCountdown;
    float regainDash = 0;

    bool leftDash, rightDash;

    bool parryAttack = false;
    float parryTimer = 0;
    float parryCooldown = 0.3f;
    public Collider2D attackTrigger;

    [SerializeField]
    GameObject healthBar, manaBar;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        movementSpeed = 5;
        jumpHeight = 5;
        dashCountdown = 10;
    }

    void FixedUpdate()
    {
        touchedGround = Physics2D.OverlapCircle(checkGround.position, groundCheckRadius, isGround);
    }

    // Update is called once per frame
    void Update()
    {
        var Horizontal = Input.GetAxis("Horizontal");
        
        if (touchedGround)
        {
            doubleJump = false;
        }
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && touchedGround)
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)||Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetInteger("States", 3);
        }
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !touchedGround && !doubleJump)
        {
            Jump();
            doubleJump = true;
            animator.SetInteger("States", 4);
        }

        //Move Left without dash
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed, GetComponent<Rigidbody2D>().velocity.y);

            Debug.Log("Left" + movementSpeed);
            animator.SetInteger("States", 1);
        }
        //Move Right without dash
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, GetComponent<Rigidbody2D>().velocity.y);

            Debug.Log("Right" + movementSpeed);
            animator.SetInteger("States", 2);
        }
        //Move Left with dash
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            leftDash = true;
            Dash();
        }
        //Move Right with dash
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftShift) && dashCountdown > 0)
        {
            rightDash = true;
            Dash();
        }

        healthBar.transform.localScale = new Vector3(health / totalHealth, 1, 1);
        manaBar.transform.localScale = new Vector3(mana / totalMana, 1, 1);

        //health -= 1;
        if (health <= 0)
        {
            SceneManager.LoadScene("GameOver");
            health = 0;
        }

        //if (Input.GetKeyDown(KeyCode.D))
        //    mana -= 10;
        //if (mana <= 0)
        //    mana = 0;
        //if (mana >= 100)
        //    mana = totalMana;
        //mana += Time.deltaTime * 3;
        
        if (Input.GetKeyDown(KeyCode.D) && !parryAttack)
        {
            parryAttack = true;
            attackTrigger.enabled = true;
            parryTimer = parryCooldown;
            mana -= 10;
            Debug.Log("Attack");
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
                attackTrigger.enabled = false;
            }
        }

        if (mana <= 0)
            mana = 0;
        if (mana >= 100)
            mana = totalMana;

        if (dashCountdown == 0)
        {
            regainDash += Time.deltaTime;
        }
        if (regainDash >= 5)
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
        }
        else if (rightDash)
        {
            //Right
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed * 10, GetComponent<Rigidbody2D>().velocity.y);
            dashCountdown--;
            Debug.Log("RightDash" + movementSpeed);
        }
    }
}
