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
            Debug.LogError("���t���̃h�A�ɑΉ����錮���ݒ肳��Ă��܂���B");
        }
    }
#endif
    public override void OnActionKey()
    {
        if(GameViewMaster.Instance.GetActivePlayerComponent().hasKeys.Exists(x => x.guid == correspondingKey.guid))
        {
            base.OnActionKey();
        }
        //�����Ȃ������献�̉���炷
    }
    public override (bool, string) EnableAction()
    {
        var actinable = GameViewMaster.Instance.GetActivePlayerComponent().hasKeys.Exists(x => x.guid == correspondingKey.guid);

        if (actinable)
        {
            return (true, "�h�A���J����");
        }
        else
        {
            return (true, "�����������Ă���B");
        }
    }
}
