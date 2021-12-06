using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public int score = 0;
    
    public int ammo = 4;

    public static int level;
    public AudioSource audioSource;
    public AudioSource musicSource;

    public float speed = 3.0f;
    bool levelComplete = false;
    bool gameIsOver = false;

    public int maxHealth = 5;

    public GameObject projectilePrefab;

    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip victoryFanfare;
    public AudioClip losingSong;

    public GameObject hurtParticles;
    public GameObject healthParticles;

    public float timeInvincible = 2.0f;

    public int health { get { return currentHealth; }}
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        UIHealthBar.instance.SetScore(score);
        UIHealthBar.instance.SetAmmo(ammo);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal,vertical);

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if(levelComplete == true)
            {
                level = 1;
                SceneManager.LoadScene("LevelTwo");
            }
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if ( character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

        if(Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
        
        if(Input.GetKeyDown(KeyCode.R) && gameIsOver == true)
        {
            if(level == 1 && score == 4)
            {
                level = 0;
                SceneManager.LoadScene("Main");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
            {
                return;
            }

            isInvincible = true;
            invincibleTimer = timeInvincible;
            GameObject projectileObject = Instantiate(hurtParticles, rigidbody2d.position, Quaternion.identity);
            PlaySound(hitSound);
        }
        else if(amount > 0)
        {
            GameObject projectileObject = Instantiate(healthParticles, rigidbody2d.position, Quaternion.identity);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth/ (float)maxHealth);
        if(currentHealth == 0)
        {
            UIHealthBar.instance.loseMessage();
            gameIsOver = true;
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
            musicSource.Stop();
            PlaySound(losingSong);
        }
    }
    void Launch()
    {
        if(ammo > 0)
        {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(throwSound);
        ammo = ammo - 1;
        UIHealthBar.instance.SetAmmo(ammo);
        }
    }
    public void ChangeAmmo(int a)
    {
        ammo = ammo + a;
        UIHealthBar.instance.SetAmmo(ammo);
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void increaseScore()
    {
        score = score + 1;
        UIHealthBar.instance.SetScore(score);
        if(score == 4 && level == 1)
        {
            UIHealthBar.instance.winMessage();
            gameIsOver = true;
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
            musicSource.Stop();
            PlaySound(victoryFanfare);
        }
        else if(score == 4 && level != 1)
        {
            UIHealthBar.instance.levelMessage();
            musicSource.Stop();
            PlaySound(victoryFanfare);
            levelComplete = true;
        }
    }
}
