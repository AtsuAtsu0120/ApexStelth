using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundMistery : Mission
{
    public FoundMistery(Action OnChangeState) : base(OnChangeState)
    {
        Name = "���̓�";
        Description = "���̓��T��B";
        State = MissionState.workInProgress;
    }

    public override void OnComplete()
    {
        
    }
}
public class MeetTarget : Mission
{
    public MeetTarget(Action OnChangeState) : base(OnChangeState)
    {
        Name = "\"���l\"(�C��)";
        Description = "�S���ŏ��𕷂��o���B";
        State = MissionState.workInProgress;
    }
    public override void OnComplete()
    {
        
    }
}
public class GoCave : Mission
{
    public GoCave(Action OnChangeState) : base(OnChangeState)
    {
        Name = "���A�̓�";
        Description = "���A�֍s���B";
        State = MissionState.Inactive;
    }
    public override void OnComplete()
    {
        
    }
}
