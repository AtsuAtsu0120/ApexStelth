using UnityEngine;
using UnityEngine.InputSystem;

public class AirWalk : Walk, ICheckGround
{
    public AirWalk(CharacterStateManager stateManager) : base(stateManager)
    {

    }
    public override void OnEnter()
    {

    }
    public override void OnExit()
    {

    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        //ê›íuîªíË
        CheckGround();

        if(stateManager.rb.velocity.y < 0)
        {
            PlayerAudioManager.Instance.StartWindNoise();
        }
        base.OnFixedUpdate();

        //ï«ÇÃÇ⁄ÇË
        var offsetY = 0.5f;
        var distance = 3.0f;
        var direction = transform.forward;
        if (stateManager.rb.velocity.y > 0 && 1.5f > stateManager.rb.velocity.y)
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

    }

    public override void OnCollisionStay(Collision collision)
    {
        
    }

    public override void OnExitCollider(Collision collision)
    {
    }

    public void CheckGround()
    {
        var rayRadius = 0.4f;
        var rayDirection = Vector3.down;
        var rayDistance = 0.2f;
        LayerMask layer = 1 << 3;

        RaycastHit hit;
        var result = Physics.SphereCast(transform.position + 0.5f * Vector3.up, rayRadius, rayDirection, out hit, rayDistance, layer, QueryTriggerInteraction.Ignore);

        if (result)
        {
            PlayerAudioManager.Instance.StopWindNoise();
            PlayerAudioManager.Instance.activeAudioSource.time = 0;
            PlayerAudioManager.Instance.StartFootStep(hit.transform.tag);
            stateManager.ChangeState(new WalkOnGround(stateManager));
        }
    }
}
