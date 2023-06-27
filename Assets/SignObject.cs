using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignObject : MonoBehaviour
{
    public Animator anim;
    private bool isActivated;

    private void Update()
    {
        anim.SetBool("isActivated", isActivated);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 7)
        {
            isActivated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 7)
        {
            isActivated = false;
        }
    }
}
