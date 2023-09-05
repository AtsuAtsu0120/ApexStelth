using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, IActionable
{
    public Animator animator;
    public virtual void OnActionKey()
    {
        Debug.Log("Open");
        animator.SetTrigger("Open");
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.CompareTag("NPC"))
        {
            animator.SetTrigger("Open");
        }
    }
}
