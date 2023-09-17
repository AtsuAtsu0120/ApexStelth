using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mission
{
    public Mission(Action OnChangeState)
    {
        this.OnChangeState += OnChangeState;
    }
    public MissionState State { get; set; }
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public bool IsOptinal { get; protected set; } = false;

    public Action OnChangeState;
    public virtual void OnComplete()
    {
        State = MissionState.Completed;
        OnChangeState?.Invoke();
    }
}
public enum MissionState
{
    Inactive,
    workInProgress,
    Completed
}
