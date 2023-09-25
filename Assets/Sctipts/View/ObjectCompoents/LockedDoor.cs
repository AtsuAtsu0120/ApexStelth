using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Door
{
    [SerializeField] private Key correspondingKey;
#if UNITY_EDITOR
    public override void Start()
    {
        base.Start();
        if(correspondingKey is null)
        {
            Debug.LogError("鍵付きのドアに対応する鍵が設定されていません。");
        }
    }
#endif
    public override void OnActionKey()
    {
        if(GameViewMaster.Instance.GetActivePlayerComponent().hasKeys.Exists(x => x.guid == correspondingKey.guid))
        {
            base.OnActionKey();
        }
        //鍵がなかったら鍵の音を鳴らす
    }
    public override (bool, string) EnableAction()
    {
        var actinable = GameViewMaster.Instance.GetActivePlayerComponent().hasKeys.Exists(x => x.guid == correspondingKey.guid);

        if (actinable)
        {
            return (true, "ドアを開ける");
        }
        else
        {
            return (true, "鍵がかかっている。");
        }
    }
}
