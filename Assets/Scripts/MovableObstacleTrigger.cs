using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObstacleTrigger : MonoBehaviour
{
    public MovableObstacle[] moveableObstacle; 
    private void Start()
    {
        moveableObstacle = GetComponentsInChildren<MovableObstacle>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foreach (var item in moveableObstacle)
            {
                if(item != null)
                    item.Trigger();
            }
        }
    }
}
