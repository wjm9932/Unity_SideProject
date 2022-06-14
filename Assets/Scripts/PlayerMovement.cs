using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animatorPlayer;
    private PlayerInput inputPlayer;
    // Start is called before the first frame update
    void Start()
    {
        animatorPlayer = GetComponent<Animator>();    
        inputPlayer = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation(inputPlayer.moveInput);
    }

    void UpdateAnimation(Vector2 input)
    {
        animatorPlayer.SetFloat("Horizontal", input.x);
        animatorPlayer.SetFloat("Vertical", input.y);
    }
}
