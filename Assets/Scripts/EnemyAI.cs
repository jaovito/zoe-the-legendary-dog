using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
  public Transform target;
  public float speed = 200f;
  public float nextWaipointDistance = 1.3f;
  public Transform enemyGFX;
  public float detectTargetRange = 2f;
  public LayerMask playerLayer;

  public bool detectedPlayer = false;
  Path path;
  int currentWayPoint = 0;
  bool reachedEndOfPath = false;

  Seeker seeker;
  Rigidbody2D rb;

  // Start is called before the first frame update
  void Start()
  {
    seeker = GetComponent<Seeker>();
    rb = GetComponent<Rigidbody2D>();

    InvokeRepeating("UpdatePath", 0f, .5f);
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    if (path == null)
      return;

    if (currentWayPoint >= path.vectorPath.Count)
    {
      reachedEndOfPath = true;
      return;
    }
    else
    {
      reachedEndOfPath = false;
    }

    Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
    Vector2 force = direction * speed * Time.deltaTime;

    rb.AddForce(force);

    float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

    if (distance < nextWaipointDistance)
    {
      currentWayPoint++;
    }

    if (force.x >= 0.01f)
    {
      enemyGFX.localScale = new Vector3(-3f, 3f, 3f);
    }
    else if (force.x <= -0.01f)
    {
      enemyGFX.localScale = new Vector3(3f, 3f, 3f);
    }
  }

  void OnPathComplete(Path p)
  {
    if (!p.error)
    {
      path = p;
      currentWayPoint = 0;
    }
  }

  void UpdatePath()
  {
    DetectPlayer();
    if (seeker.IsDone() && detectedPlayer)
      seeker.StartPath(rb.position, target.position, OnPathComplete);
  }

  void DetectPlayer()
  {
    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, detectTargetRange, playerLayer);

    foreach (Collider2D enemy in hitEnemies)
    {
      detectedPlayer = true;
    }
  }

  void OnDrawGizmosSelected()
  {
    if (target == null)
      return;

    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, detectTargetRange);
  }
}
