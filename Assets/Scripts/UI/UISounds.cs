using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISounds : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{

     public AudioSource source;
     public AudioClip selectionSound;
    public AudioClip clickSound;
    private Selectable but;

    private void Start()
    {
        but = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(but && but.interactable)
        source.PlayOneShot(selectionSound, 0.1f);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (but && but.interactable)
            source.PlayOneShot(clickSound, 0.2f);
    }
}
