using UnityEngine;
using UnityEngine.InputSystem;

public class Crouch : Walk
{
    public Crouch(CharacterStateManager stateManager) : base(stateManager) { }

    private float MinSpeed = 2.0f;
    public override void OnEnter()
    {
        Speed = 20.0f;
        stateManager.ChangeViewPointState(new CrouchViewpoint(stateManager));
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if(Vector3.Magnitude(stateManager.rb.velocity) <= MinSpeed)
        {
            Speed = 2.0f;
        }
        else
        {
            if (Speed > MinSpeed)
            {
                Speed -= 0.1f;
            }
        }
    }
    public override void OnCancelDown(InputAction.CallbackContext ctx)
    {
        stateManager.ChangeState(new WalkOnGround(stateManager));
    }
    public override void OnExit()
    {
        stateManager.ChangeViewPointState(new ViewPointAfterCrouch(stateManager));
    }

    public override void OnCollisionStay(Collision collision)
    {
        
    }

    public override void OnExitCollider(Collision collision)
    {
        
    }
}
