using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool isGift;
    Rigidbody rb;
    public Transform top;
    public int index;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Barrier")
        {
            Collect.instance.FallGifts(index);
            Fall();
            //Destroy(this);
        }
    }
    public void Fall()
    {
        rb.isKinematic = false;
        rb.AddForce(Motorcycle.instance.GetCrushDir() / 2);
        this.transform.SetParent(null);
    }
    public void ActivateMoney()
    {
        isGift = false;
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
