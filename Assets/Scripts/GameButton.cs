using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isClick = false, OneClick = true;
    public GameObject TargetObject = null;
    public string Message;

    void Update()
    {
        if (!OneClick && isClick && TargetObject != null) TargetObject.SendMessage(Message);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isClick)
        {
            if (OneClick && TargetObject != null) TargetObject.SendMessage(Message);
            isClick = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClick = false;
    }
}
