using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Door
{
    [SerializeField] private Key correspondingKey;
#if UNITY_EDITOR
    public void Start()
    {
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
    }
}
