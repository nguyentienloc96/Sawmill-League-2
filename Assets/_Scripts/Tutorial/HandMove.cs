using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMove : MonoBehaviour
{

    public Animator anim;
    public Transform hand;
    public Transform tfBegin;
    public Transform tfEnd;

    private bool isAction;
    private float timeAction;

    public void OnEnable()
    {
        if (tfBegin != null)
        {
            hand.position = tfBegin.position;
        }
    }

    public void Update()
    {
        if (tfBegin != null && tfEnd != null)
        {
            if (isAction)
            {
                hand.position = Vector3.MoveTowards(hand.position, tfEnd.position, 1.5f * Time.deltaTime);
                if (hand.position == tfEnd.position)
                {
                    hand.position = tfBegin.position;
                    anim.enabled = true;
                    isAction = false;
                    timeAction = 0;
                }
            }
            else
            {
                timeAction += Time.deltaTime;
                if (timeAction >= 1f)
                {
                    anim.Rebind();
                    anim.enabled = false;
                    isAction = true;
                    timeAction = 0;
                }
            }
        }
    }
}
