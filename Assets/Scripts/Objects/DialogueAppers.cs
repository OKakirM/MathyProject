using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAppers : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField, Range(0f, 20f)] private float distanceToReach = 10f;
    public Animator anim;
    private float playerDistance;
    private bool isActivated;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();

        if(playerDistance <= distanceToReach)
        {
            isActivated = true;
        } 
        else
        {
            isActivated = false;
        }

        anim.SetBool("isActivated", isActivated);
    }

    private void CheckDistance()
    {
        playerDistance = Vector2.Distance(transform.position, player.position);
    }
}
