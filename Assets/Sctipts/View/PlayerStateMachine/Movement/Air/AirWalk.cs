using UnityEngine;
using UnityEngine.InputSystem;

public class AirWalk : Walk
{
    public AirWalk(CharacterStateManager stateManager) : base(stateManager)
    {

    }
    public override void OnEnter()
    {
        
    }
    public override void OnFixedUpdate()
    {
        //d—Í‚ð—^‚¦‚éB
        stateManager.rb.AddForce(new(0, Physics.gravity.y, 0), ForceMode.Acceleration);
        base.OnFixedUpdate();

        //•Ç‚Ì‚Ú‚è
        var offsetY = 0.5f;
        var distance = 3.0f;
        var direction = transform.forward;
        if(stateManager.rb.velocity.y > 0 && 1.5f > stateManager.rb.velocity.y)
        {
            if (Physics.Raycast(transform.position + Vector3.up * offsetY, direction, out var hit, distance + 0.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                stateManager.ChangeState(new ClimbWall(stateManager, hit));
                stateManager.ChangeViewPointState(new WallViewpoint(stateManager));
            }
        }
    }
    public override void OnPerformDown(InputAction.CallbackContext ctx)
    {
        stateManager.ChangeState(new AirCrouch(stateManager));
    }
    public override void OnEnterCollider(Collision collision)
    {
        if(collision.collider.tag.Contains("Ground"))
        {
            stateManager.ChangeState(new WalkOnGround(stateManager));
        }
    }

    public override void OnCollisionStay(Collision collision)
    {
        
    }

    public override void OnExitCollider(Collision collision)
    {
        
    }
}
