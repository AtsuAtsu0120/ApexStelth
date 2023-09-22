using Prashalt.Unity.ConversationGraph;
using Prashalt.Unity.ConversationGraph.Conponents;
using UnityEngine;

public class ConversationObject : MonoBehaviour, IActionable
{
    [SerializeField] ConversationGraphAsset asset;
    [SerializeField] ConversationSystemUGUI conversationSystem;

    public bool EnableAction()
    {
        return true;
    }

    public virtual void OnActionKey()
    {
        conversationSystem.SetConversationAsset(asset);
        conversationSystem.StartConversation();
    }
}