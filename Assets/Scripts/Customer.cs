using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    public int giftCount;
    [SerializeField] private GameObject happyImage;
    [SerializeField] private GameObject sadImage;
    [SerializeField] private Transform sadPos;
    private void Update()
    {
        if (Vector3.Distance(transform.position, sadPos.position) <= .5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        animator = GetComponentInChildren<Animator>();
        if (other.gameObject.tag == "Player")
            TakeGift();
    }
    public void TakeGift()
    {
        Debug.Log(Collect.instance.GetGiftCount() + " " + giftCount);
        if (Collect.instance.GetGiftCount() < giftCount)
        {
            sadImage.SetActive(true);
            animator.SetTrigger("Sad");
            Move();
        }
        else
        {
            happyImage.SetActive(true);
            animator.SetTrigger("Dance");
            Destroy(gameObject, 10);
        }
        Collect.instance.Deliver(giftCount);
    }
    public void Move()
    {
        sadPos.transform.SetParent(null);
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(sadPos.position);
    }
}
