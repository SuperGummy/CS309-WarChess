using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class CharacterRenderer : MonoBehaviour
{

    [SerializeField] public Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ForTest()
    {
        Debug.Log("Successfully visited character renderer");
    }

    public void SetSprite(Sprite spriteToSet)
    {
        spriteRenderer.sprite = spriteToSet;
    }

    public void SetController(RuntimeAnimatorController controllerToSet)
    {
        Debug.Log("Set controller " + controllerToSet);
        animator.runtimeAnimatorController = controllerToSet;
    }

    public void SetAnimation(int horizontal, int vertical)
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

}
