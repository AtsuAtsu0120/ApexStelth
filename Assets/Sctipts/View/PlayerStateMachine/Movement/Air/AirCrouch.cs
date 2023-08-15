using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCrouch : AirWalk
{
    public AirCrouch(CharacterStateManager stateManager) : base(stateManager) { }

    public override void OnEnter()
    {
        Debug.Log("Air Crouch");
        stateManager.ChangeViewPointState(new CrouchViewpoint(stateManager));
    }
    public override void OnExit()
    {
        Debug.Log("Air Crouch Exit");
        stateManager.ChangeViewPointState(new ViewPointAfterCrouch(stateManager));
    }
}
