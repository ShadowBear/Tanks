using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TankVirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    private Image bgImage;
    private Image joystickImage;
    private Vector3 inputVector;

    // Use this for initialization

    void Awake()
    {
#if Unity_STANDALONE || UNITY_WEBPLAYER
        this.enabled = false;
#else
        this.enabled = true;
#endif
    }

    void Start()
    {
        bgImage = GetComponent<Image>();
        //joystickImage = GetComponentInChildren<Image>();
        joystickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImage.rectTransform.sizeDelta.y);
            inputVector = new Vector3((pos.x * 2) - 1, 0, (pos.y * 2) - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            //Debug.Log(inputVector);

            //Joystick Movement
            joystickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImage.rectTransform.sizeDelta.x / 2.5f), inputVector.z * (bgImage.rectTransform.sizeDelta.y / 2.5f));
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }
    
    public float Horizontal()
    {
        if (inputVector.x != 0) return inputVector.x * -1;
        else return Input.GetAxis("Horizontal1");
    }

    public float Vertical()
    {
        if (inputVector.z != 0) return inputVector.z;
        else return Input.GetAxis("Vertical1");
    }

}
