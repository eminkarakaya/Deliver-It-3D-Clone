using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Collect : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;
    [SerializeField] float moneyMultiplier;
    public static Collect instance;
    [SerializeField] private Transform collectableItemPlace;
    [SerializeField] private List<Collectable> collectedItems;
    private Transform lastTransform;
    private float _collectablePlaceRotation = 0;
    public float collectablePlaceRotation
    {
        get => _collectablePlaceRotation;
        set
        {
            _collectablePlaceRotation = value;
            collectableItemPlace.transform.localRotation = Quaternion.Euler(collectablePlaceRotation * 2, 0, 0);
        }
    }
    private void Awake()
    {
        instance = this;
        lastTransform = collectableItemPlace;
    }
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        GiftIdleAnimation();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gift")
        {
            audioSource.PlayOneShot(audioClip);
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
        collectable.transform.SetParent(collectableItemPlace.transform);
        collectable.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
    public void FallGifts(int index)
    {
        for (int i = index; i < collectedItems.Count; i++)
        {               
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
    public int GetCollecteMoney()
    {
        return (int) (collectedItems.Count * moneyMultiplier);
    }
    public void ConvertMoney()
    {
        for (int i = 0; i < collectedItems.Count; i++)
        {
            collectedItems[i].ActivateMoney();
        }
    }
    public List<GameObject> CollectedMoneys()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < collectedItems.Count; i++)
        {
            GameObject [] moneys = collectedItems[i].GetComponentsInChildren<MeshRenderer>().Select(x=>x.gameObject).ToArray();
            for (int j = 0; j < moneys.Length; j++)
            {
                list.Add(moneys[j]);
            }
        }
        return list;
    }
    public IEnumerator GiftIdleAnimation()
    {
        yield return new WaitForSeconds(1f);
        collectableItemPlace.transform.parent.DOShakeRotation(.5f, 2, 3).SetLoops(-1, LoopType.Incremental);
    }
    public void GiftForwardAnim(float speed)
    {
        var offsetX = (1 / speed)*8;
        Vector3 offset = new Vector3(offsetX, 0, 0);
        collectableItemPlace.DORotate(collectableItemPlace.eulerAngles + offset, .1f);
    }
    public void GiftBackAnim(float speed)
    {
        var offsetX = (1 / speed) * 8;
        Vector3 offset = new Vector3(-offsetX, 0, 0);
        collectableItemPlace.DORotate(collectableItemPlace.eulerAngles + offset, .1f);
    }
   
    public void ResetPos()
    {
        collectablePlaceRotation = 0;
        collectedItems.Clear();
    }
}
