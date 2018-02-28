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

    [SerializeField]
    BossHealth bosshealth;

    public Transform checkGround;
    public float groundCheckRadius;
    public LayerMask isGround;
    private bool touchedGround;
    private bool doubleJump;
    float totalMana = 100;
    public float mana;
    float dashCountdown;
    float regainDash = 0;
    public bool invincible = false;
    public bool invincible2 = false;
    public float invinciblelifetime = 0;
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
    // THIS ISN'T EVEN MY FINAL FORM
    public static bool _crescendo = false;
    // actually it is


    private int maxHeartAmount = 5;
    public int startHeart = 3;
    public int currHeart;
    private int maxHeart;
    private int healthPerHeart = 2;

    public Image[] healthImage;
    public Sprite[] healthSprite;

    Vector3 lastPosition, currentPosition;

    [SerializeField]
    GameObject manaBar;
    [SerializeField]
    MainGame mainGame;

    public ScreenShake screenShake;
    public Dictionary<int, double> _keys = new Dictionary<int, double>();

    //I mean i could just add a tag for score but uhh idk
    [SerializeField]
    Score playerScore;

    // Use this for initialization
    void Start()
    {
        //bosshealth = GetComponent<BossHealth>();
        //bosshealth.health = 100.0f;
        animator = this.GetComponent<Animator>();
        movementSpeed = 5;
        jumpHeight = 5;
        dashCountdown = 7.0f;
        mana = 0;

        currHeart = startHeart * healthPerHeart;
        maxHeart = maxHeartAmount * healthPerHeart;
        checkHealth();

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
        updateHealth();
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
            _keys[1] = 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _keys[2] = 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _keys[3] = 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _keys[4] = 0.5f;
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

        manaBar.transform.localScale = new Vector3(mana / totalMana, 1, 1);
        Movement();
        Jump();
        ParryAttack();
        UpdateKeys();

        // MANA DRAIN
        if (!_crescendo)
        {
            mana -= 0.02f;
        }
        else
            mana -= 0.5f;
        // cause like... its actually a sound/music bar thing
        // and uhh.. sound energy is lost to surrounding, amirite?
        // I'm not a scientist. This is a game.

        if (mana <= 0)
        {
            mana = 0;
            _crescendo = false;
        }

        if (mana >= 100 && !_crescendo)
        {
            mana = totalMana;
            //mana = 0;
            _crescendo = true;

            // Add base 5000 score
            playerScore.AddScore(5000.0f);
            // Increase multiplier by 0.5f
            playerScore.AddMultiplier(0.5f);

            // True, trigger drop state, trigger QTE
            // Clear all projectiles on screen
        }

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

        //Player Fall off screen,takes dmg, Reset back to previous position
        lastPosition = currentPosition;
        if (!touchedGround && transform.position.y <= -5)
        {
            transform.position = lastPosition;
            takeDamage(3);
        }
        removeAllProjectile();
        //ForHighScoreTesting
        //if (Input.GetKey(KeyCode.PageDown))
        //    takeDamage(50);
        //if (Input.GetKey(KeyCode.End))
        //    playerScore.ClearAllScores();
    }

    //Dash movement
    void Dash()
    {
        if (leftDash)
        {
            //Left
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed * 10, GetComponent<Rigidbody2D>().velocity.y);
            dashCountdown--;
            leftDash = false;
            invincible = true;
        }
        if (rightDash)
        {
            //Right
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed * 10, GetComponent<Rigidbody2D>().velocity.y);
            dashCountdown--;
            rightDash = false;
            invincible = true;
        }
        if (leftUpDash)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movementSpeed * 2);
            dashCountdown--;
            invincible = true;
            leftUpDash = false;
        }
        if (rightUpDash)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, movementSpeed * 2);
            dashCountdown--;
            invincible = true;
            rightUpDash = false;
        }
        if (leftIdleDash)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-movementSpeed * 1.5f, GetComponent<Rigidbody2D>().velocity.y);
            dashCountdown--;
            invincible = true;
            leftIdleDash = false;
        }
        if (rightIdleDash)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed * 1.5f, GetComponent<Rigidbody2D>().velocity.y);
            dashCountdown--;
            invincible = true;
            rightIdleDash = false;
        }

    }

    //Movement
    void Movement()
    {
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
            playerScore.AddScore(100);
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

    //Total heart player have
    void checkHealth()
    {
        for (int i = 0; i < maxHeartAmount; ++i)
        {
            if (startHeart <= i)
                healthImage[i].enabled = false;
            else
                healthImage[i].enabled = true;
        }
        updateHealth();
    }

    void updateHealth()
    {
        bool empty = false;
        int i = 0;
        foreach (Image image in healthImage)
        {
            if (empty)
            {
                image.sprite = healthSprite[0];
            }
            else
            {
                ++i;
                if (currHeart >= i * healthPerHeart)
                    image.sprite = healthSprite[healthSprite.Length - 1];
                else
                {
                    int currHeartHealth = healthPerHeart - (healthPerHeart * i - currHeart);
                    int healthPerImage = healthPerHeart / (healthSprite.Length - 1);
                    int imageIndex = currHeartHealth / healthPerImage;
                    image.sprite = healthSprite[imageIndex];
                    empty = true;
                }
            }
        }
    }

    //Player gets damage
    public void takeDamage(int amount)
    {
        currHeart -= amount;
        currHeart = Mathf.Clamp(currHeart, 0, startHeart * healthPerHeart);

        // If hit, reset multiplier
        playerScore.ResetMultiplier();

        //No more heart, Gameover
        if (currHeart <= 0)
        {
            //PlayerPrefs.SetFloat("bosshealth", bosshealth.health);
            playerScore.SaveScore();
            print(playerScore.GetCurrScore());
            SceneManager.LoadScene("GameOver");
        }
        updateHealth();
    }

    //Player regen health
    public void regenHealth(int amount)
    {
        currHeart += amount;
        currHeart = Mathf.Clamp(currHeart, 0, startHeart * healthPerHeart);
        updateHealth();
    }

    //Increase heart for player
    public void addHealth(int amount)
    {
        startHeart += amount;
        startHeart = Mathf.Clamp(startHeart, 0, maxHeartAmount);

        maxHeart = maxHeartAmount * healthPerHeart;

        checkHealth();
    }

    //ForBlankPowerupTesting
    public void removeAllProjectile()
    {
        GameObject[] gameObjects;
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            gameObjects = GameObject.FindGameObjectsWithTag("Projectile");
            for(int i=0;i<gameObjects.Length;i++)
            {
                Destroy(gameObjects[i]);
            }
        }
    }
}
