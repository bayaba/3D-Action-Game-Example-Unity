using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public bool isClick = false;
    Vector2 origin, buttonDis;
    RectTransform Rtransform;

    public float Degree;

    public GameObject TargetObject = null;
    public string Message;

    void Start()
    {
        Rtransform = GetComponent<RectTransform>();
        origin = Rtransform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isClick)
        {
            buttonDis = eventData.position - origin;
            isClick = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.position = origin;
        isClick = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Rtransform.position = eventData.position - buttonDis;
        Vector2 dir = (Vector2)Rtransform.position - origin;
        Degree = (Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 360f) % 360f;

        if (TargetObject != null)
        {
            TargetObject.SendMessage(Message, Degree);
        }
    }
}
