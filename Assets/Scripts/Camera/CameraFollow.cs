using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public PlayerController pController;
    public float fowardView;
    public float lookDownTimer;

    private Vector2 targetPos;
    private Vector2 pos;

    public Vector2 maxPosRightUp, maxPosLeftDown;
    public float cameraSpeed;
    private float lookDownCounter = 0f;

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        targetPos.x = target.transform.position.x;
        targetPos.y = target.transform.position.y;

        if (targetPos.x >= maxPosRightUp.x && targetPos.x <= maxPosLeftDown.x && !pController.isDead)
        {
            if(pController.direction.x != 0)
            {
                pos.x = pController.direction.x > 0 ? targetPos.x + fowardView : targetPos.x - fowardView;
            }
            else
            {
                pos.x = targetPos.x;
            }
        }

        if(targetPos.y <= maxPosRightUp.y && targetPos.y >= maxPosLeftDown.y && !pController.isDead && !pController.isCrouching)
        {
            pos.y = targetPos.y;
            lookDownCounter = 0f;
        }

        if (pController.isCrouching)
        {
            lookDownCounter += Time.deltaTime;
            if(lookDownCounter >= lookDownTimer)
            {
                pos.y = targetPos.y - fowardView;
            }
        }

        if(pController.isDead)
        {
            pos.x = targetPos.x;
            pos.y = targetPos.y;
        }

        transform.position = Vector3.Lerp(transform.position, new Vector3(pos.x, pos.y, -1), cameraSpeed * Time.deltaTime);
    }
}
