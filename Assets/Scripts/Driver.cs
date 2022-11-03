using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator)/*,typeof(Rigidbody)*/)]
public class Driver : MonoBehaviour
{
    public static Driver instance;
    public Animator animator;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

}
