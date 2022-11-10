using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Barrier : MovableObstacle
{
    [SerializeField] Light red, yellow, green;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Trigger();
        }
    }
    public override void Trigger()
    {
        IEnumerator qwe()
        {
            yield return new WaitForSeconds(1f);
            red.gameObject.SetActive(false);
            yield return new WaitForSeconds(.25f);
            yellow.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            yellow.gameObject.SetActive(false);
            yield return new WaitForSeconds(.25f);
            green.gameObject.SetActive(true);

            transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, 90), 2f);
        }
        StartCoroutine(qwe());
    }
}
