using UnityEngine;

public class Withdrawal : MonoBehaviour, IActionable
{
    public (bool, string) EnableAction()
    {
        if (GameLogicMaster.Instance.stageInfo.Missions.Exists(x => x.IsOptinal == false && x.State != MissionState.Completed))
        {
            return (false, "Mission��B�����ĒE�o");
        }
        else
        {
            return (true, "�E�o����B");
        }
    }

    public void OnActionKey()
    {
        GameViewMaster.Instance.OnClear();
    }
}
