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

    //dodging
    float dodgetimer;
    [SerializeField] AnimationCurve dodgeCurve;

    //animation
    int isWalkingHash, isRunningHash, isJumpingHash;
    bool isDodging;

    //audio
    AudioSource audioSource;
    [SerializeField] AudioClip walkAudio, runAudio;
  


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        Keyframe dodge_LastFrame = dodgeCurve[dodgeCurve.length - 1];
        dodgetimer = dodge_LastFrame.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDodging) 
        {
            Move();
            
            Jump();
        }

        
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
                audioSource.clip = runAudio;
                audioSource.Play();
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
            audioSource.clip = walkAudio;
            audioSource.Play();

            if (Input.GetKeyDown(KeyCode.E))
                StartCoroutine(Dodge());

        }

        else
        {
            animator.SetBool(isWalkingHash, false);
            audioSource.Stop();
        }

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

    IEnumerator Dodge()
    {
        animator.SetTrigger("Dodge");
        isDodging = true;
        float timer = 0;

        characterController.center = new Vector3(0, 0.5f, 0);
        characterController.height = 1;

        gravityModifier += Physics.gravity.y * Time.deltaTime;

        while (timer < dodgetimer)
        {
            float speed = dodgeCurve.Evaluate(timer);
            Vector3 direction = (transform.forward * speed) + (Vector3.up * gravityModifier);
            characterController.Move(direction * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isDodging = false;

        characterController.center = new Vector3(0, 1f, 0);
        characterController.height = 2;
    }
}
