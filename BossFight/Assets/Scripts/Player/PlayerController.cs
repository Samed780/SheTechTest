using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    //references
    private CharacterController characterController;
    [SerializeField] private Transform cam;
    [SerializeField] private Animator animator;

    //movement speed
    [SerializeField] float speed = 5f;

    //movement rotation smoothing
    [SerializeField] float rotationSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    //jumping
    [SerializeField] float jumpForce = 5f;
    Vector3 jumpVelocity;
    float gravityModifier;

    //animation
    int isWalkingHash, isRunningHash, isJumpingHash;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Jump();
        
    }

    void Move()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (move.magnitude > 0.1f)
        {

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = 10f;
                animator.SetBool(isRunningHash, true);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 5f;
                animator.SetBool(isRunningHash, false);
            }

            float angle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, rotationAngle, 0);

            Vector3 direction = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;
            characterController.Move(direction.normalized * Time.deltaTime * speed);

            animator.SetBool(isWalkingHash, true);

        }

        else
            animator.SetBool(isWalkingHash, false);

    }

    void Jump()
    {
        gravityModifier += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            gravityModifier = -0.5f;

            if (Input.GetButtonDown("Jump"))
            {
                gravityModifier = jumpForce;
                animator.SetBool(isJumpingHash, true);
            }
            else
                animator.SetBool(isJumpingHash, false);
        }

        jumpVelocity.y = gravityModifier;

        characterController.Move(jumpVelocity * Time.deltaTime);
    }

}
