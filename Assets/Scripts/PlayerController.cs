using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    private int desiredLane = 1;

    public float laneDistance = 4;

    public float jumpForce;
    public float Gravity = -20;

    private bool isSliding = false;
    public Animator animator;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        animator = GetComponent<Animator>();
        if (!PlayerManager.isGameStarted)
        {
            return;
        }
        animator.SetBool("isGameStarted", true);
        direction.z = forwardSpeed;

        direction.y += Gravity * Time.deltaTime;

        if (controller.isGrounded)
        {
            if (SwipeManager.swipeUp)
            {
                Jump();
                isSliding = true;
                animator.SetBool("isJumping", true);

            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
            {
                animator.SetBool("isJumping", false);
                isSliding = false;
            }
        }
      



        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
            {
                desiredLane = 2;
            }
        }
        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
            {
                desiredLane = 0;
            }
        }

        if (SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        } 
        else
        {
            if (desiredLane == 2) {
                targetPosition += Vector3.right * laneDistance;
            }
        }

        //transform.position = Vector3.Lerp(transform.position, targetPosition, 30);
        //controller.center = controller.center;
        if (transform.position == targetPosition)
        {
            return;
        }
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
        {
            controller.Move(moveDir);
        }
        else
        {
            controller.Move(diff);
        }

    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
        {
            return;
        }
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {

            PlayerManager.gameOverFlag = true;
            FindObjectOfType<AudioManager>().PlaySound("lost");
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        Animator animator;
        animator = GetComponent<Animator>();
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        animator.SetBool("isSliding", true);
        yield return new WaitForSeconds(1f);
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        animator.SetBool("isSliding", false);
        isSliding = false;
    }

}
