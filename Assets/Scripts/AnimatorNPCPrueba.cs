using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorNPCPrueba : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool move;
    [SerializeField] private bool work;
    [SerializeField] private bool fun;
    [SerializeField] private bool idle;


    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (move) {
            animator.SetBool("isMoving", true);
        }
        if (idle) {
            animator.SetBool("isMoving", false);
        }
    }
}
