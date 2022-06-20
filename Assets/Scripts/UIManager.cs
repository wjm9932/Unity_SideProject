using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<UIManager>();

            return instance;
        }
    }

    [SerializeField] private Crosshair crosshair;

    public void UpdateCrossHairPosition(Vector3 pos)
    {
        crosshair.UpdatePosition(pos);
    }
}
