using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundMistery : Mission
{
    public FoundMistery(Action OnChangeState) : base(OnChangeState)
    {
        Name = "“‡‚Ì“ä";
        Description = "“‡‚Ì“ä‚ğ’T‚êB";
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
        Name = "\"úl\"(”CˆÓ)";
        Description = "˜S‰®‚Åî•ñ‚ğ•·‚«o‚¹B";
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
        Name = "“´ŒA‚Ì“ä";
        Description = "“´ŒA‚Ös‚¯B";
        State = MissionState.Inactive;
    }
    public override void OnComplete()
    {
        
    }
}
