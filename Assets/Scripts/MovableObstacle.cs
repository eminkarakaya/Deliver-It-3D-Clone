using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObstacle : MonoBehaviour
{
    [SerializeField] protected float time;
    [SerializeField] protected Transform destination;
    //private void Start()
    //{
    //    destination.SetParent(null);
    //}
    public abstract void Trigger();
}
