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
    [SerializeField] private float speed;
    [SerializeField] private Vector3 dir;
    public override void Trigger()
    {
        Move();
    }
    public void Move()
    {
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
