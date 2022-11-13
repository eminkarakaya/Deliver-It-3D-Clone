using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Barrier : MovableObstacle
{
    [SerializeField] Light red, yellow, green;
    private void Start()
    {
        DOTween.Init(null, true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Trigger();
        }
    }
    public override void Trigger()
    {
        IEnumerator ChangeLight()
        {
            this.GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(1f);
            red.gameObject.SetActive(false);
            yield return new WaitForSeconds(.25f);
            yellow.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            yellow.gameObject.SetActive(false);
            yield return new WaitForSeconds(.25f);
            green.gameObject.SetActive(true);

            transform.DOLocalRotate(new Vector3(transform.rotation.x, transform.rotation.y, 90), 2f);
        }
        StartCoroutine(ChangeLight());
    }
}
