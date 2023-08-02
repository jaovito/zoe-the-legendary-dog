using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowScript : MonoBehaviour
{
  public GameObject arrow;
  Vector2 direction;
  public float force;

  public GameObject pointPrefab;
  public List<GameObject> points;
  public int numberOfPoints;
  public GameObject bowGraphics;
  public SpriteRenderer rightHandWeapon;

  private PlayerInputActions controls;
  private CharacterController2D playerController;
  private PlayerMovement playerMovement;

  private bool isShooted = false;


  void Awake()
  {
    controls = new PlayerInputActions();
    controls.Player.ShootArrow.performed += ctx => Shoot();

    GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

    playerController = player[0].GetComponent<CharacterController2D>();
    playerMovement = player[0].GetComponent<PlayerMovement>();
  }


  void FixedUpdate()
  {
    Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    Vector2 bowPos = transform.position;
    direction = MousePos - bowPos;

    FaceMouse();
    OnAim();

    if (playerMovement.isAim && direction.x < -0.1f && playerController.m_FacingRight)
    {
      playerController.Flip();
    }
    else if (playerMovement.isAim && direction.x > 0.1f && !playerController.m_FacingRight)
    {
      playerController.Flip();
    }
  }

  void OnAim()
  {
    float aimHolded = Input.GetAxisRaw("Fire2");

    if (aimHolded >= 0.1f && GlobalState.Instance.arrowCount > 0)
    { 
      if (points.ToArray().Length <= 0)
        CreatePoints();

      MovePointsToAim();
    }
    else if (aimHolded < 0.1f)
    {
      DeletePoints();
    }
  }

  void CreatePoints()
  {
    points.AddRange(new GameObject[numberOfPoints]);

    for (int i = 0; i < numberOfPoints; i++)
    {
      points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity);
    }
  }

  void MovePointsToAim()
  {
        rightHandWeapon.enabled = false;
      bowGraphics.SetActive(true);
      
      for (int i = 0; i < points.ToArray().Length; i++)
      {
        points[i].transform.position = PointPosition(i * 0.1f);
      }
      playerMovement.isAim = true;
      playerController.Move(0, false, false);
  }

  void DeletePoints()
  {
    rightHandWeapon.enabled = true;
      bowGraphics.SetActive(false);
      for (int i = 0; i < points.ToArray().Length; i++)
      {
        Destroy(points[i].gameObject);
        points.Remove(points[i]);
      }
      playerMovement.isAim = false;
  }

  void FaceMouse()
  {
    transform.right = direction;
  }

  Vector2 PointPosition(float t)
  {
    Vector2 currentPointPos = (Vector2)transform.position + (direction.normalized * force * t) + 0.5f * Physics2D.gravity * (t * t);

    return currentPointPos;
  }

  void Shoot()
  {
    if (!isShooted && GlobalState.Instance.arrowCount > 0 && playerMovement.isAim)
    {
      GameObject arrowsIn = Instantiate(arrow, transform.position, transform.rotation);

      arrowsIn.GetComponent<Rigidbody2D>().velocity = transform.right * force;
      GlobalState.Instance.ChangeArrowCount(GlobalState.Instance.arrowCount - 1);

      isShooted = true;
      Invoke("ResetShoot", 0.5f);
    }
  }

  void ResetShoot()
  {
    isShooted = false; ;
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
