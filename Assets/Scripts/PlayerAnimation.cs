using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAnimation("SwordAttack");
        }
    }

    private void PlayAnimation(string param)
    {
        Debug.Log($"Playing animation {param}");
        animator.SetTrigger(param);
    }
}