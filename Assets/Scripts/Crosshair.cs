using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image hitPoint;
    public Image aimPoint;

    private RectTransform crossHairRectTransform;
    private Camera camera;
    private Vector2 targetPoint;

    private void Start()
    {
        crossHairRectTransform = hitPoint.GetComponent<RectTransform>();
        camera = Camera.main;
    }


    public void UpdatePosition(Vector3 pos)
    {
        targetPoint = camera.WorldToScreenPoint(pos);
    }

    private void Update()
    {
        crossHairRectTransform.position = targetPoint;
    }
}
