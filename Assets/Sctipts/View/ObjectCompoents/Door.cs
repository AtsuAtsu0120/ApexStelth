using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(StelthAudio))]
public class Door : MonoBehaviour, IActionable
{
    public Animator animator;
    private StelthAudio stelthAudio;

    public virtual void Start()
    {
        stelthAudio = GetComponent<StelthAudio>();
    }
    public virtual (bool, string) EnableAction()
    {
        return (true, "ƒhƒA‚ðŠJ‚¯‚é");
    }

    public virtual void OnActionKey()
    {
        DoorOpen();
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("NPC") && other.isTrigger == false)
        {
            DoorOpen();
        }
    }
    public void DoorOpen()
    {
        stelthAudio.Play();
        animator.SetTrigger("Open");
    }
}
