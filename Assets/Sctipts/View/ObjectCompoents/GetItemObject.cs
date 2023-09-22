using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemObject : MonoBehaviour, IActionable
{
    [SerializeField] private Item item;

    public bool EnableAction()
    {
        return true;
    }

    public void OnActionKey()
    {
        var player = GameViewMaster.Instance.GetActivePlayerComponent();
        GameViewMaster.Instance.UpdateItem(item.name);
        player.hasItems.Add(item);

        Destroy(gameObject);
    }
}
