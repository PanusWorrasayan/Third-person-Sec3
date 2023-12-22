using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private float playerspeed = 5f;
    [SerializeField] private Camera followCamera;


    [SerializeField] private float rotationSpeed = 10f;
    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -13f;

    public bool groundedPlayer;
    [SerializeField] private float jumpHeight = 2.5f;

    public Animator animator;

    public static PlayerCon instance;

    public void Awake()
    {
        instance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (Victory.Instance.isWinner) {

            case true:
            animator.SetBool("Victory", Victory.Instance.isWinner);
            break;

            case false:
            Movment();
            break; 
        }
       
    }
    void Movment()
    {
        groundedPlayer = characterController.isGrounded;
        if (characterController.isGrounded && playerVelocity.y < -2f) {
            playerVelocity.y = -1f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0,followCamera.transform.eulerAngles.y,0) *
                                                new Vector3 (horizontalInput,0,verticalInput);
        Vector3 movementDirection = movementInput.normalized;   

        characterController.Move(movementDirection * playerspeed * Time.deltaTime);

        if (movementDirection != Vector3.zero) { 
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
             transform.rotation = Quaternion.Slerp(transform.rotation,desiredRotation, rotationSpeed * Time.deltaTime);
        }

        if(Input.GetButton("Jump") && groundedPlayer){
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityValue);
            animator.SetTrigger("Jumping");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        animator.SetFloat("Speed",Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("Ground", characterController.isGrounded);
        
    }
}
