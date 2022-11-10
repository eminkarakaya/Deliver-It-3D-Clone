using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyAnimation : MonoBehaviour
{
    private Transform targetTransform;
    private void Start()
    {
    }
    public void Trigger()
    {
        targetTransform = OfficeManager.instance.currentOffice.moneyImage.transform;
        this.transform.DOMove(targetTransform.position, 2f).OnComplete(()=>Destroy(this.gameObject));
    }
}
