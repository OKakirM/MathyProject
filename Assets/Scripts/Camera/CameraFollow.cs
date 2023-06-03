using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;

    private Vector2 targetPos;
    private Vector2 pos;

    public Vector2 maxPosRightUp, maxPosLeftDown;
    public float cameraSpeed;
    public bool activate = true;

    private void Awake()
    {
        pos.x = targetPos.x + maxPosRightUp.x;
        pos.y = targetPos.y + maxPosLeftDown.y;
        transform.position = Vector3.Lerp(transform.position, new Vector3(pos.x, pos.y, -1), 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        if (activate)
        {
            if (target)
            {
                targetPos.x = target.transform.position.x;
                targetPos.y = target.transform.position.y;

                if (targetPos.x > maxPosRightUp.x && targetPos.x < maxPosLeftDown.x)
                {
                    pos.x = targetPos.x;
                }

                if(targetPos.y < maxPosRightUp.y && targetPos.y > maxPosLeftDown.y)
                {
                    pos.y = targetPos.y;
                }
            }

            transform.position = Vector3.Lerp(transform.position, new Vector3(pos.x, pos.y, -1), cameraSpeed * Time.deltaTime);
        }
    }
}
