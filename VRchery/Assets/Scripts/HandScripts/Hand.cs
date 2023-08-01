using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    public float speed;
    Animator animator;
    private float drawTarget;
    private float gripTarget;
    private float drawCurrent;
    private float gripCurrent;
    private string animatorDrawParam = "Draw";
    private string animatorGripParam = "Grip";

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        // Reset the animation to the initial state when the object is reactivated
        gripCurrent = Mathf.MoveTowards(1.0f, 0f, Time.deltaTime * speed);
        animator.SetFloat(animatorGripParam, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
        Debug.Log("current" + gripCurrent);
    }

    internal void SetDraw(float v)
    {
        drawTarget = v;
    }

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    void AnimateHand()
    {
        if (drawCurrent != drawTarget)
        {
            drawCurrent = Mathf.MoveTowards(drawCurrent, drawTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorDrawParam, drawCurrent);
        }

        if (gripCurrent != gripTarget)
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorGripParam, gripCurrent);
        }
    }


}