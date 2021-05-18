using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutImg : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        InputManager.instance.TutRate++;
        InputManager.instance.ShowTutImg.sprite = InputManager.instance.TutImg[InputManager.instance.TutRate];

        if (InputManager.instance.TutRate + 1 == InputManager.instance.TutImg.Count) 
        {
            InputManager.instance.ShowTutImg.gameObject.SetActive(false);
            InputManager.instance.bIsTut = false;
            InputManager.instance.TutRate = 0;
        }
    }
}
