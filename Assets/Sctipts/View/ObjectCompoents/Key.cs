using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IActionable
{
    public string guid { get; private set; } = Guid.NewGuid().ToString();
    public void OnActionKey()
    {
        Debug.Log("Keyを取得しました。");
        GameViewMaster.Instance.GetActivePlayerComponent().hasKeys.Add(this);
    }
}
