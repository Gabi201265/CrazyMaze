using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.CompareTag("player1"))
        {
            if (Input.GetAxis("P2_Vertical") != 0)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
        else if (this.gameObject.CompareTag("player2"))
        {
            if (Input.GetAxis("P1_Vertical") != 0)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }

        }
    }
}
