using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemObject : MonoBehaviour, IActionable
{
    [SerializeField] private Item item;

    public (bool, string) EnableAction()
    {
        return (true, $"{item.name}ÇèEÇ§");
    }

    public void OnActionKey()
    {
        var player = GameViewMaster.Instance.GetActivePlayerComponent();
        player.hasItems.Add(item);

        GameViewMaster.Instance.UpdateItem(item.name);

        Destroy(gameObject);
    }
}
