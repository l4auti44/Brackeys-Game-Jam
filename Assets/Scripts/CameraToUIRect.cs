using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToUIRect : MonoBehaviour
{
    public Camera targetCamera;
    public RectTransform uiRect;

    void LateUpdate()
    {
        Vector3[] corners = new Vector3[4];
        uiRect.GetWorldCorners(corners);

        Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 topRight = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        float x = bottomLeft.x / Screen.width;
        float y = bottomLeft.y / Screen.height;
        float width = (topRight.x - bottomLeft.x) / Screen.width;
        float height = (topRight.y - bottomLeft.y) / Screen.height;

        targetCamera.rect = new Rect(x, y, width, height);
    }
}
