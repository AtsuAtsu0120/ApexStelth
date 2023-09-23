using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Withdrawal : MonoBehaviour, IActionable
{
    public (bool, string) EnableAction()
    {
        return (true, "�E�o����B");
    }

    public void OnActionKey()
    {
        if(!GameLogicMaster.Instance.stageInfo.Missions.Exists(x => x.IsOptinal == false && x.State == MissionState.Completed))
        {
            Debug.Log("�E�o�I");
        }
    }
}
