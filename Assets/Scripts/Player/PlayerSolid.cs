using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSolid : MonoBehaviour
{
    private SpriteRenderer playerSprite;
    private Shader spriteShader;
    public Color spriteColor;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        spriteShader = Shader.Find("GUI/Text Shader");
    }

    public void ColorSprite()
    {
        playerSprite.material.shader = spriteShader;
        playerSprite.color = spriteColor;
    }

    public void Finish()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ColorSprite();
    }
}
