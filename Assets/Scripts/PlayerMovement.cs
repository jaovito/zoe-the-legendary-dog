using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public CharacterController2D controller;
  float horizontalMove = 0f;
  public float runSpeed = 0f;
  private Animator animator;
  bool jump = false;
  bool crouch = false;
  public bool activeJump = true;

  public LayerMask groundLayer;
  public bool isAim = false;

  // Start is called before the first frame update
  void Start()
  {
    animator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
    if (Input.GetButtonDown("Jump"))
    {
      jump = true;
      activeJump = true;
      animator.SetBool("isJumping", true);
    }

    if (Input.GetButtonDown("Crouch"))
    {
      crouch = true;
    }
    else if (Input.GetButtonUp("Crouch"))
    {
      crouch = false;
    }
  }

  public void OnCrouching(bool isCrouching)
  {
    animator.SetBool("isCrouching", isCrouching);
  }

  void FixedUpdate()
  {
    if (!isAim)
    {
      controller.Move(horizontalMove * Time.deltaTime, crouch, jump);

      if (horizontalMove != 0f)
      {
        animator.SetBool("isWalking", true);
      }
      else
      {
        animator.SetBool("isWalking", false);
      }
    } else if (isAim)
    {
        animator.SetBool("isWalking", false);
    }
    jump = false;
  }

  void OnCollisionEnter2D(Collision2D col)
  {
    if ((groundLayer.value & (1 << col.transform.gameObject.layer)) > 0)
    {
      OnLanding();
    }
  }

  public void OnLanding()
  {
    animator.SetBool("isJumping", false);
    activeJump = false;
    Debug.Log("encostou");
  }
}
