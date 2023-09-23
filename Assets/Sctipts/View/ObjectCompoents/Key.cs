using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IActionable
{
    public string guid { get; private set; } = Guid.NewGuid().ToString();

    public (bool, string) EnableAction()
    {
        return (true, "�������");
    }

    public void OnActionKey()
    {
        Debug.Log("Key���擾���܂����B");
        GameViewMaster.Instance.GetActivePlayerComponent().hasKeys.Add(this);
    }
}
