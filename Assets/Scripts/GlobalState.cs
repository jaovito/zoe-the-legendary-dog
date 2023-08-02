using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance 
    {
        get; 
        private set; 
    }

    public float playerHealth = 100f;
    public float maxHealth = 100f;
    public float swordDamage = 20f;
    public float arrowDamage = 20f;
    public int arrowCount = 0;
    public SpriteRenderer playerSprite;
    public Rigidbody2D playerRigidBody;
    public CharacterController2D characterController;
    public Animator playerAnimator;

    public float impulseForce = 2f;
    public GameObject player;

    private Color initialColor;
    public HealthBar healthBar;
    public bool isAim = false;

    public ArrowCounter arrowCounterText;

    private void Awake()
    {
        initialColor = playerSprite.color;
        playerHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            // destroy
        }
    }

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        playerSprite.color = new Color(1, 0, 0, 1);
        playerRigidBody.AddForce(new Vector2(characterController.m_FacingRight ? -impulseForce * 2 : impulseForce * 2, impulseForce), ForceMode2D.Impulse);
        playerAnimator.SetTrigger("Hurted");

        Invoke("ResetPlayerColor", 0.2f);
        healthBar.SetHealth(playerHealth -= damage);

        if (playerHealth <= 0)
        {
            PlayerDie();
        }
    }

    private void ResetPlayerColor()
    {
        playerSprite.color = initialColor;
    }

    public void PlayerDie()
    {
        Destroy(player);
    }

    public void ChangeArrowCount(int count)
    {
        arrowCount = count;
        arrowCounterText.SetArrowCounter(count);
    }
}
