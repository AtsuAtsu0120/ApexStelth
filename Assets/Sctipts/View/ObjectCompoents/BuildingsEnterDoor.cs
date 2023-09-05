using Prashalt.Unity.ConversationGraph;
using UnityEngine;

public class BuildingsEnterDoor : Door, IActionable
{
    [SerializeField] private ConversationGraphAsset changeCharacterTutrial;
    public override void OnActionKey()
    {
        GameViewMaster.Instance.conversationSystemUGUI.SetConversationAsset(changeCharacterTutrial);
        GameViewMaster.Instance.conversationSystemUGUI.StartConversation();
        base.OnActionKey();
    }
}
