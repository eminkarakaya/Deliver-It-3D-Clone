using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject followObject;
    [SerializeField] Vector3 offset;
    void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, followObject.transform.position + offset, .1f);
    }
    private void Update()
    {
        Follow();
    }
}
