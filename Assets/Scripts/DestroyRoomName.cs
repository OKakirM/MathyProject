using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRoomName : MonoBehaviour
{
    private float timeToDestroy = 2.5f;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timeToDestroy)
        {
            Destroy(transform.gameObject);
        }
    }
}
