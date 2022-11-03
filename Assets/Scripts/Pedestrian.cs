using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Pedestrian : MovableObstacle
{
    NavMeshAgent agent;
    Animator animator;
    [SerializeField] private Transform destination;
    public override void Trigger()
    {
        Move();
    }
    public void Move()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("Move");
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.position);
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, destination.position) <= .5f)
        {
            Destroy(this.gameObject);
        }
    }   
}
