using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BossTrigger : MonoBehaviour
{
    public GameObject boss;
    public GameObject dirt;
    public  float timer;
    void OnTriggerEnter2D(Collider2D other)
    {
            boss.SetActive(true);
        if (timer == 0)
        {
            dirt.SetActive(true);
            Destroy(this);
        }
        else
        {
            timer =- Time.deltaTime;
        }
        
    }

}
