using System;
using UnityEngine;

public class Key : MonoBehaviour, IActionable
{
    public string guid { get; private set; } = Guid.NewGuid().ToString();

    public (bool, string) EnableAction()
    {
        return (true, "Œ®‚ð“üŽè");
    }

    public void OnActionKey()
    {
        PlayerAudioManager.Instance.PlayGetItem();
        GameViewMaster.Instance.GetActivePlayerComponent().hasKeys.Add(this);

        Destroy(gameObject);
    }
}
