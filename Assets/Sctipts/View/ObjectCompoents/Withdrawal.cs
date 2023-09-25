using UnityEngine;

public class Withdrawal : MonoBehaviour, IActionable
{
    public (bool, string) EnableAction()
    {
        if (GameLogicMaster.Instance.stageInfo.Missions.Exists(x => x.IsOptinal == false && x.State != MissionState.Completed))
        {
            return (false, "Missionを達成して脱出");
        }
        else
        {
            return (true, "脱出する。");
        }
    }

    public void OnActionKey()
    {
        GameViewMaster.Instance.OnClear();
    }
}
