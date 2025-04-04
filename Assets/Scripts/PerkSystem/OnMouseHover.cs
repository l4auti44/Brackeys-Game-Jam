using UnityEngine.EventSystems;
using UnityEngine;

public class OnMouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 offset;
    private bool mouse_over = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        transform.position += offset;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        transform.position -= offset;
    }
}
