using Prashalt.Unity.ConversationGraph;
using Prashalt.Unity.ConversationGraph.Conponents;
using UnityEngine;

public class ConversationObject : MonoBehaviour, IActionable
{
    [SerializeField] ConversationGraphAsset asset;
    [SerializeField] ConversationSystemUGUI conversationSystem;

    public (bool, string) EnableAction()
    {
        return (true, "話しかける。");
    }

    public virtual void OnActionKey()
    {
        conversationSystem.SetConversationAsset(asset);
        conversationSystem.StartConversation();
    }
}
