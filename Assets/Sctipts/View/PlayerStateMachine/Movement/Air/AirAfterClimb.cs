using UnityEngine;

public class AirAfterClimb : Walk
{
    public AirAfterClimb(CharacterStateManager stateManager) : base(stateManager) { }

    public override void OnFixedUpdate()
    {
        //if(stateManager.rb.velocity.y < 0)
        //{
        //    stateManager.ChangeState(new WalkOnGround(stateManager));
        //}
        //d—Í‚ð—^‚¦‚éB
        stateManager.rb.AddForce(new(0, Physics.gravity.y, 0), ForceMode.Acceleration);
        base.OnFixedUpdate();
    }
    public override void OnEnterCollider(Collision collision)
    {
        if (collision.collider.tag.Contains("Ground"))
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
