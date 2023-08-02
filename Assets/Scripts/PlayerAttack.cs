using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
  private Animator animator;
  public Transform attackPoint;
  public LayerMask enemyLayers;

  public float attackRange = 0.5f;
  public int attackDamage = 20;

  [Space][SerializeField]
  private PlayerMovement playerMovement;
  private PlayerInputActions controls;


  void Awake()
  {
    controls = new PlayerInputActions();
    controls.Player.SwordAttack.performed += ctx => Attack();
  }

  void Start()
  {
    animator = GetComponent<Animator>();
    playerMovement = GetComponent<PlayerMovement>();
  }

  public void Attack()
  {
    if (playerMovement.activeJump)
      return;

    animator.SetTrigger("Attack");

    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

    foreach (Collider2D enemy in hitEnemies)
    {
      enemy.GetComponent<EnemyScript>().TakeDamage(GlobalState.Instance.swordDamage);
    }
  }

  void OnDrawGizmosSelected()
  {
    if (attackPoint == null)
      return;

    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
  }

  private void OnEnable()
  {
    controls.Enable();
  }

  private void OnDisable()
  {
    controls.Disable();
  }
}
