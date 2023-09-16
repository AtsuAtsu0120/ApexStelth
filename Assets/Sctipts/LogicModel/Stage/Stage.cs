using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stage
{
    public bool IsComplete { get; set; }
    public List<Mission> Missions { get; set; }
}
