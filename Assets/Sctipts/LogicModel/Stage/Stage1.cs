using System;
using System.Collections.Generic;

public class Stage1 : Stage
{
    public Stage1(Action OnChangeMissionState)
    {
        Missions = new()
        {
            new FoundMistery(OnChangeMissionState),
            new MeetTarget(OnChangeMissionState)
        };
    }
}
