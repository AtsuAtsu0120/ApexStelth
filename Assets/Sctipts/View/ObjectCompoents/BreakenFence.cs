using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakenFence : MonoBehaviour, IActionable
{
    public (bool, string) EnableAction()
    {
        var hasItem = GameViewMaster.Instance.GetActivePlayerComponent().hasItems.Exists(x => x.name == "Wrench"); ;
        if(hasItem)
        {
            return (true, "‰ó‚·");
        }
        else
        {
            return (false, "Wrench‚ª•K—v");
        }
       
    }

    public void OnActionKey()
    {
        Destroy(gameObject);
    }
}
