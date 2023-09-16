using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetConversation : ConversationObject
{
    public override void OnActionKey()
    {
        base.OnActionKey();

        GameLogicMaster.Instance.stageInfo.Missions.Find(x => x.Name == "\"úl\"i”CˆÓj").OnComplete();
    }
}
