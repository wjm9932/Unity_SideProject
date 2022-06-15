using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPosition : MonoBehaviour
{
    public Transform rightHand;

    private void LateUpdate()
    {
        transform.position = rightHand.position;
        transform.rotation = rightHand.rotation;
    }
}
