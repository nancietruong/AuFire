using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    static readonly int isOpenDoorHash = Animator.StringToHash("isOpenDoor");

    Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        animator.SetBool(isOpenDoorHash, true);
    }

    public void CloseDoor()
    {
        animator.SetBool(isOpenDoorHash, false);
    }
}
