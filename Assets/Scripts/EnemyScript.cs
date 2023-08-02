using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
  public float maxHealth = 200f;
  float currentHealth;

  private SpriteRenderer sprite;
  private Rigidbody2D rb;
  private Color initialColor;
  public CharacterController2D characterController;
  public float impulseForce = 2f;

  public LayerMask playerMask;
  public float enemyDamage = 20f;
  public float hitPlayerRange = 2f;
  public float attackCooldown = 1f;

  private bool attacked = false;
  public Animator enemyAnimator;
  private EnemyAI enemyAI;

  private float initialNextWaipointDistance = 0.1f;


  // Start is called before the first frame update
  void Start()
  {
    currentHealth = maxHealth;
    sprite = GetComponentInChildren<SpriteRenderer>();
    rb = GetComponent<Rigidbody2D>();
    initialColor = sprite.color;
    enemyAI = GetComponent<EnemyAI>();

    initialNextWaipointDistance = enemyAI.nextWaipointDistance;
  }

  void FixedUpdate()
  {

    Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, hitPlayerRange, playerMask);

    if (hitPlayer)
    {
      HitPlayer(enemyDamage);
    }
  }

  public void TakeDamage(float damage)
  {
    currentHealth -= damage;
    sprite.color = new Color(1, 0, 0, 1);
    rb.AddForce(new Vector2(characterController.m_FacingRight ? impulseForce : -impulseForce, impulseForce), ForceMode2D.Impulse);
    attacked = true;
    enemyAI.nextWaipointDistance = 3f;
    
    if (enemyAnimator != null)
      enemyAnimator.SetTrigger("Hurted");

    if (currentHealth <= 0)
    {
      Die();
    }

    enemyAI.detectedPlayer = true;
    Invoke("ResetEnemyColor", 0.2f);
    Invoke("CoolDown", attackCooldown);
  }

  public void HitPlayer(float damage)
  {
    Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, 2f, playerMask);

    if (hitPlayer && !attacked)
    {
      GlobalState.Instance.TakeDamage(damage);
      rb.AddForce(new Vector2(characterController.m_FacingRight ? impulseForce * 2f : -impulseForce * 2f, impulseForce), ForceMode2D.Impulse);
      attacked = true;
      enemyAI.nextWaipointDistance = 3f;

      Invoke("CoolDown", attackCooldown);
    }
  }

  private void CoolDown()
  {
    attacked = false;
    enemyAI.nextWaipointDistance = initialNextWaipointDistance;
  }

  void Die()
  {
    Destroy(gameObject);
  }

  public void ResetEnemyColor()
  {
    sprite.color = initialColor;
  }

  void OnDrawGizmosSelected()
  {
    if (transform == null)
      return;

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, hitPlayerRange);
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.gameObject.tag == "Arrow")
    {
     TakeDamage(GlobalState.Instance.arrowDamage);
    }
  }
}
