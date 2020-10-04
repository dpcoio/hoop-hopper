using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonSoundEvent : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public AudioSource audioSource;
    public AudioClip hoverClip;
    public AudioClip selectClip;

    public void OnPointerEnter(PointerEventData ped)
    {
        audioSource.clip = hoverClip;
        audioSource.Play();
    }

    public void OnPointerDown(PointerEventData ped)
    {
        audioSource.clip = selectClip;
        audioSource.Play();
    }
}