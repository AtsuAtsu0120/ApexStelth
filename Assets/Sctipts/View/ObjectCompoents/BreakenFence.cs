using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakenFence : MonoBehaviour, IActionable
{
    public bool EnableAction()
    {
        return GameViewMaster.Instance.GetActivePlayerComponent().hasItems.Exists(x => x.name == "Wrench");
    }

    public void OnActionKey()
    {
        Destroy(gameObject);
    }
}
