using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{

    private float inputDirection;       //X value of our MoveVector
    private float verticalVelocity;     //Y value of our MoveVector

    public float speed = 5.0f;
    public float gravity = 30.0f;
    public float jumpForce = 10.0f;
    private bool secondJumpAvail = false;
    private bool wallJumpAvail = false;

    private Vector3 moveVector;
    private Vector3 lastMotion;
    private CharacterController controller;

    void Start()
    {

        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        IsControllerGrounded();
        moveVector = Vector3.zero;
        inputDirection = Input.GetAxis("Horizontal") * speed;
        moveVector.x = inputDirection;
        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            if (verticalVelocity > 0)
            {
                verticalVelocity = -5;
            }
        }

        //Jump/dobule jump/isGrounded check
        if (IsControllerGrounded())
        {
            verticalVelocity = 0;
            wallJumpAvail = false;

            //Regular jump
            if (Input.GetKey(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                secondJumpAvail = true;
            }
        }

        else
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {

                //Double jump
                if (secondJumpAvail)
                {
                    verticalVelocity = jumpForce;
                    secondJumpAvail = false;
                }
            }

            verticalVelocity -= gravity * Time.deltaTime;

            if (wallJumpAvail)
            {
                moveVector.x = lastMotion.x;
            }
        }

        moveVector.y = verticalVelocity;
        controller.Move(moveVector * Time.deltaTime);
        lastMotion = moveVector;
    }

    private bool IsControllerGrounded()
    {

        Vector3 leftRayStart;
        Vector3 rightRayStart;

        leftRayStart = controller.bounds.center;
        rightRayStart = controller.bounds.center;

        leftRayStart.x -= controller.bounds.extents.x;
        rightRayStart.x += controller.bounds.extents.x;

        Debug.DrawRay(leftRayStart, Vector3.down, Color.red);
        Debug.DrawRay(rightRayStart, Vector3.down, Color.green);

        if (Physics.Raycast(leftRayStart, Vector3.down, (controller.height / 2) + 0.2f))
        {
            return true;
        }

        if (Physics.Raycast(rightRayStart, Vector3.down, (controller.height / 2) + 0.2f))
        {
            return true;
        }

        return false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        //Wall jump
        if (controller.collisionFlags == CollisionFlags.Sides)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 2.0f);
                moveVector = hit.normal * speed;
                verticalVelocity = jumpForce;
                secondJumpAvail = true;
                wallJumpAvail = true;
            }
        }

        //Collectables
        switch (hit.gameObject.tag)
        {
            case "Coin":
                Destroy(hit.gameObject);
                break;
            default:
                break;
        }
    }
}
