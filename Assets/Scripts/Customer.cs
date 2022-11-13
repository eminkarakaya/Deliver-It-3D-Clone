using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    //AudioSource audioSource;
    Animator animator;
    public int giftCount;
    [SerializeField] private GameObject happyImage;
    [SerializeField] private GameObject sadImage;
    [SerializeField] private Vector3 sadPos;
    private void Start()
    {
        sadPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 10);
        //audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, sadPos) <= .5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        animator = GetComponentInChildren<Animator>();
        if (other.gameObject.tag == "Player")
        {
            TakeGift();
            //audioSource.Play();
        }
    }
    public void TakeGift()
    {
        if (Collect.instance.GetGiftCount() < giftCount)
        {
            sadImage.SetActive(true);
            animator.SetTrigger("Sad");
        }
        else
        {
            happyImage.SetActive(true);
            animator.SetTrigger("Dance");
        }
        //Destroy(gameObject, 10);
        Collect.instance.Deliver(giftCount);
    }

}
