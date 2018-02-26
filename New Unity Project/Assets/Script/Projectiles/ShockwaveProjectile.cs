using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveProjectile : Projectile
{
    [SerializeField]
    GameObject _shockwave;

    private GameObject _screenShake;
    private GameObject _feedback;

    // Number of waves to make
    int _waves = 5;
    // Speed of projectile
    int _speed = 5;
    // Tallest point of wave
    int _top = -1;
    // Affect scale of projectile, also used to count spawning up
    int _height = 0;
    // Needed to spawn two highest points in event of even
    bool _isEven = false;
    // Begin Spawning shockwaves when true
    bool _spawnShockwave = false;
    // Time counter
    double _spawnTime = 0.0;
    // Time between each wave
    double timeToSpawn;
    // Once beyond the top, start reducing height of wave
    bool _pastTop = false;

    // Use this for initialization
    protected void Start ()
    {
        _screenShake = GameObject.FindGameObjectWithTag("ScreenShake");
        _feedback = GameObject.FindGameObjectWithTag("Feedback");
        timeToSpawn = ((_speed * Time.deltaTime) / _shockwave.transform.localScale.x);
        base.Start();
    }
	
	// Update is called once per frame
	protected void Update ()
    {
        base.Update();

        // Means it hasnt been set at all
        if (_top == -1)
        {
            if (_waves == 0)
            {
                print("Number of waves is zero!");
            }

            if ((_waves % 2) == 1)
            {
                // Odd
                _top = (_waves / 2);
            }
            else
            {
                // Even
                _top = (_waves / 2) - 1;
                _isEven = true;
            }
        }

        if (hittarget)
        {
            float _multiplier = ((float)_waves * 0.2f);

            _feedback.GetComponent<Feedback>().CreateImage("ParryFail", gameObject.transform.position);
            _feedback.GetComponent<Feedback>().CreateAudio("Fail");

            // BWAAAAAAAAAAAAAAAH
            _screenShake.GetComponent<ScreenShake>().ShakeCamera(0.1f * _multiplier, 0.3f * _multiplier, 0.95f);

            _spawnShockwave = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (_spawnShockwave)
        {
            _spawnTime += Time.deltaTime;

            //print(_spawnTime);
            //print(timeToSpawn);
            //print(left.GetComponent<Projectile>().transform.localScale);

            if (_spawnTime > timeToSpawn)
            {
                Vector3 moveUp = new Vector3(0, _height);

                // movmeup * 0.33f ( 0.165)
                // += moveup (* 0.5f)

                GameObject left = Instantiate(_shockwave, transform.position + (moveUp * 0.33f), Quaternion.identity);
                left.GetComponent<Projectile>().SetDir(new Vector3(-1, 0));
                left.GetComponent<Projectile>().SetSpeed(_speed);
                left.GetComponent<Projectile>().transform.localScale += moveUp;
                left.GetComponent<BoxCollider2D>().enabled = true;

                GameObject right = Instantiate(_shockwave, transform.position + (moveUp * 0.33f), Quaternion.identity);
                right.GetComponent<Projectile>().SetDir(new Vector3(1, 0));
                right.GetComponent<Projectile>().SetSpeed(_speed);
                right.GetComponent<Projectile>().transform.localScale += moveUp;
                right.GetComponent<BoxCollider2D>().enabled = true;

                // This took way longer than i'd like
                if (!_pastTop)
                    ++_height;
                else
                    --_height;

                if (_height > _top)
                {
                    _pastTop = true;

                    if (_isEven)
                        _height -= 1;
                    else
                        _height -= 2;
                }


                _spawnTime = 0.0;
            }

            if (_height < 0)
            {
                // Done spawning
                print("poof!");
                Destroy(gameObject);
            }
        }
	}

    public void SetWaves(int waves)
    {
        _waves = waves;
    }
}
