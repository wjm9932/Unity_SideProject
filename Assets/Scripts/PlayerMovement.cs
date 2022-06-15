using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animatorPlayer;
    private PlayerInput inputPlayer;
    private CharacterController characterController;

    [SerializeField] private float speed;
    [SerializeField] private float playerGravity;
    [SerializeField] private float refVelocity;
    [SerializeField] private float smoothTime;


    // Start is called before the first frame update
    void Start()
    {
        animatorPlayer = GetComponent<Animator>();    
        inputPlayer = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();

        speed = 6f;
        playerGravity = 0f;
        smoothTime = 0.1f;
    }

    private void FixedUpdate()
    {
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
        Vector3 moveDir = new Vector3(playerMoveInput.x, 0, playerMoveInput.y).normalized;

        targetSpeed = Mathf.SmoothDamp(GetCurrentVelocity(), targetSpeed, ref refVelocity, smoothTime);

        playerGravity += Physics.gravity.y * Time.deltaTime;

        var velocity = targetSpeed * moveDir + Vector3.up * playerGravity;

        characterController.Move(velocity * Time.deltaTime);

        if(characterController.isGrounded == true)
        {
            playerGravity = 0f;
        }
    }

    void Rotate()
    {

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
