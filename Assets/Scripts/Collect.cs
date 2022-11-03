using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public static Collect instance;
    [SerializeField] private Transform collectableItemPlace;
    [SerializeField] private List<Collectable> collectedItems;
    private Transform lastTransform;
    private void Awake()
    {
        instance = this;
        lastTransform = collectableItemPlace;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gift")
        {
            CollectCollectable(other.gameObject.GetComponent<Collectable>());
        }
    }
    public void CollectCollectable(Collectable collectable)
    {
        if(collectedItems.Count == 0)
        {
            collectable.transform.position = collectableItemPlace.transform.position;
        }
        else
        {
            collectable.transform.position = collectedItems[collectedItems.Count - 1].top.position;
        }
        collectable.transform.forward = this.transform.forward;
        collectable.index = collectedItems.Count;
        collectable.tag = "Untagged";
        collectedItems.Add(collectable);
        collectable.GetComponent<Collider>().isTrigger = false;
        collectable.transform.SetParent(this.transform);
    }
    public void FallGifts(int index)
    {
        for (int i = index; i < collectedItems.Count; i++)
        {
               
            Debug.Log(collectedItems[i]);
            Debug.Log(i + " i");
            collectedItems[i].Fall();
        }
        for (int i = index; i < collectedItems.Count; i++)
        {
            collectedItems.Remove(collectedItems[i]);
            //Destroy(collectedGifts[i]);
        }
    }
    public float GetCollectedCount()
    {
        return collectedItems.Count;
    }
    List<Collectable> GetDelivered(int count)
    {
        List<Collectable> tempList = new List<Collectable>();
        var temp = 0;
        for (int i = 0; i < collectedItems.Count; i++)
        {
            
            if(collectedItems[i].isGift)
            {
                temp++;
                tempList.Add(collectedItems[i]);
            }
            if (temp == count)
                break;
        }
        return tempList;
    }
    public int GetGiftCount()
    {
        var temp = 0;
        for (int i = 0; i < collectedItems.Count; i++)
        {
            if (collectedItems[i].isGift)
                temp++;
        }
        return temp;
    }
    int GetMoneyCount()
    {
        var temp = 0;
        for (int i = 0; i < collectedItems.Count; i++)
        {
            if (!collectedItems[i].isGift)
                temp++;
        }
        return temp;
    }
    public void Deliver(int count)
    {
        var temp = 0;
        var list = GetDelivered(count);
        for (int i = 0; i < count; i++)
        {
            if (temp == list.Count)
                break;
            list[i].ActivateMoney();
            temp++;
        }
    }
}
