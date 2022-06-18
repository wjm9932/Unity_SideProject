using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animatorPlayer;
    private PlayerInput inputPlayer;
    private CharacterController characterController;
    private Camera camera;

    [SerializeField] private float speed;
    [SerializeField] private float playerGravity;
    [SerializeField] private float refVelocity;
    [SerializeField] private float turnSmoothVelocity;
    [SerializeField] private float smoothTime;
    [SerializeField] private float turnSmoothTime;

    // Start is called before the first frame update
    void Start()
    {
        animatorPlayer = GetComponent<Animator>();
        inputPlayer = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        camera = Camera.main;

        speed = 6f;
        playerGravity = 0f;
        smoothTime = 0.1f;
        turnSmoothTime = 0.1f;
    }

    private void FixedUpdate()
    {
        if(GetCurrentVelocity() > 0.2f)
        {
            
        }
        Rotate();
        Move(inputPlayer.moveInput);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation(inputPlayer.moveInput);
    }

    void Move(Vector2 playerMoveInput)
    {
        float targetSpeed = playerMoveInput.magnitude * speed;

        Vector3 moveDir = Vector3.Normalize(transform.forward * playerMoveInput.y + transform.right * playerMoveInput.x);

        targetSpeed = Mathf.SmoothDamp(GetCurrentVelocity(), targetSpeed, ref refVelocity, smoothTime);

        playerGravity += Physics.gravity.y * Time.deltaTime;

        var velocity = targetSpeed * moveDir + Vector3.up * playerGravity;

        characterController.Move(velocity * Time.deltaTime);

        if (characterController.isGrounded == true)
        {
            playerGravity = 0f;
        }
    }

    void Rotate()
    {
        var targetRotation = camera.transform.eulerAngles.y;

        targetRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

        transform.eulerAngles = Vector3.up * targetRotation;
    }

    void UpdateAnimation(Vector2 input)
    {
        animatorPlayer.SetFloat("Horizontal", input.x);
        animatorPlayer.SetFloat("Vertical", input.y);
    }

    float GetCurrentVelocity()
    {
        return new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;
    }
}
