using UnityEngine;

public class Sprint : WalkOnGround
{
    public Sprint(CharacterStateManager stateManager) : base(stateManager) { }

    public override void OnEnter()
    {
        Debug.Log("State is just Sprint");
        Speed = 80;
        MaxSpeed = 18.0f;
    }

    public override void OnExit()
    {

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnEnterCollider(Collision collision)
    {
        
    }
}
