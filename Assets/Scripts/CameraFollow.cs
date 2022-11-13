using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    [SerializeField] GameObject followObject;
    [SerializeField] Vector3 offset;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        followObject = Motorcycle.instance.transform.GetChild(3).gameObject;
    }
    private void Update()
    {
        if (!Motorcycle.instance.isFinish)
        {
            Follow();
        }
    }
    void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, followObject.transform.position + offset, .1f);
    }
    public void Finish(Transform camPlace , Transform lookAt)
    {
        transform.DOMove(camPlace.position, .5f).OnComplete(()=> transform.DOLookAt(OfficeManager.instance.currentOffice.middleOfOffice.position, .5f));
    }
}
