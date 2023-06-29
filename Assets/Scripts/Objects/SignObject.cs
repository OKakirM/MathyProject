using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignObject : MonoBehaviour
{
    public Animator anim;
    public Animator dialogueAnim;
    public AudioSource blipSound;
    private bool isActivated;

    private void Update()
    {
        anim.SetBool("isActivated", isActivated);

        if (isActivated)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.L))
            {
                dialogueAnim.SetBool("isActivated", isActivated);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 7)
        {
            if (!blipSound.isPlaying)
            {
                blipSound.Play();
            }
            isActivated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 7)
        {
            isActivated = false;
            dialogueAnim.SetBool("isActivated", isActivated);
        }
    }
}
