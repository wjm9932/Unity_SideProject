using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 moveInput { get; private set; }
    public bool isJump { get; private set; }
    public bool isFire { get; private set; }
    public bool isReload { get; private set; }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isJump = Input.GetButton("Jump");
        isFire = Input.GetButton("Fire1");
        isReload = Input.GetButton("Reload");
    }
}
