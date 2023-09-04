using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IActionable
{
    public Animator animator;
    public void OnActionKey()
    {
        animator.SetTrigger("Open");
    }
}
