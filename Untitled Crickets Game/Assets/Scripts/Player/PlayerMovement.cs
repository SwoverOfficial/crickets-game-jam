using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [SerializeField] private TeleportPlayer teleportPlayerScript;

    public float speed = 12f;
    private float gravity = -9.81f;
    public float gravityScale = 1f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    private bool isGrounded;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //GROUND CHECK
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; //Value of -2 forces player to be on the ground

        //PLAYER INPUT
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //CALCULATING MOVEMENT
        Vector3 move = transform.right * x + transform.forward * z;

        //MOVEMENT APPLIED WITH SPEED
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButton("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * (gravity * gravityScale));

        //GRAVITY
        velocity.y += (gravity * gravityScale) * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
