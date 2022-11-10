using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Train : MovableObstacle
{
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;
    public override void Trigger()
    {
        Destroy(this.gameObject, 5f);
        audioSource = GetComponent<AudioSource>();
        AudioSource.PlayClipAtPoint(audioClip, this.transform.position);
        transform.DOMove(destination.position, time);
    }
    
}
