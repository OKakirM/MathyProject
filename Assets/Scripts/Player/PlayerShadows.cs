using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadows : MonoBehaviour
{
    [Header("Shadows")]
    public static PlayerShadows me;
    public GameObject shadow;
    public List<GameObject> pool = new List<GameObject>();
    private float timer;
    [SerializeField, Range(1f, 100f)] private float shadowSpeed;
    public Color shadowColor;

    private void Awake()
    {
        me = this;
    }

    public GameObject GetShadows()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                pool[i].transform.position = transform.position;
                pool[i].transform.rotation = transform.rotation;
                pool[i].GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                pool[i].GetComponent<PlayerSolid>().spriteColor = shadowColor;
                return pool[i];
            }
        }
        GameObject obj = Instantiate(shadow, transform.position, transform.rotation) as GameObject;
        obj.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        obj.GetComponent<PlayerSolid>().spriteColor = shadowColor;
        pool.Add(obj);
        return obj;
    }

    public void ShadowTimer()
    {
        timer += shadowSpeed * Time.deltaTime;
        if (timer > 1)
        {
            GetShadows();
            timer = 0;
        }
    }


}
