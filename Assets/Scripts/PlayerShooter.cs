﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    public Transform leftHand;

    private PlayerInput input;
    private Vector3 aimTarget;
    private Camera camera;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimaion();
        UpdateAimTarget();

        if (input.isFire == true)
        {
            Shot();
        }
        if (input.isReload == true)
        {
            Reload();
        }
    }

    void Shot()
    {
        gun.Fire(aimTarget);
    }

    void UpdateAimTarget()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hit, gun.fireDistance) == true)
        {
            aimTarget = hit.point;
            if (Physics.Linecast(gun.fireTransform.position, aimTarget, out hit) == true)
            {
                aimTarget = hit.point;
            }
        }
        else
        {
            //aimTarget = ray.direction * gun.fireDistance + camera.transform.position;
            aimTarget = camera.transform.position + camera.transform.forward * gun.fireDistance;
            //aimTarget = gun.fireTransform.position + camera.transform.forward * gun.fireDistance;
        }
    }

    void Reload()
    {
        if (gun.Reload() == true)
        {
            animator.SetTrigger("Reload");
        }
    }

    private void OnAnimatorIK()
    {
        if (gun.state != Gun.State.Reload)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
        }
    }

    void UpdateAnimaion()
    {
        float angle = camera.transform.eulerAngles.x;

        if (angle >= 270f)
        {
            angle -= 360f;
        }

        angle = angle / -180f + 0.5f;

        animator.SetFloat("Angle", angle);
    }
}
