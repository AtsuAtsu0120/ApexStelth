using UnityEngine;

public class NormalViewpoint : ViewpointState
{
    private float Sensibility = 1.0f;
    private float MaxLimit = 85.0f;
    private float MinLimit = 0.0f;
    public NormalViewpoint(CharacterStateManager stateManager) : base(stateManager)
    {

    }

    public override void OnEnter()
    {
        MinLimit = 360 - MaxLimit;
    }

    public override void OnExit()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnUpdate()
    {
        //Ž‹“_ˆÚ“®
        var lookVector = stateManager.lookAction.ReadValue<Vector2>();
        var camTransform = stateManager.transform.GetChild(0);
        //c‰ñ“]‚ÍƒJƒƒ‰‚ð“®‚©‚·
        var camLocalAngle = stateManager.camTransform.localEulerAngles;
        camLocalAngle.x += -lookVector.y * Sensibility;
        //c‰ñ“]‚Í§ŒÀ‚ ‚è
        if (camLocalAngle.x > MaxLimit && camLocalAngle.x < 180)
        {
            camLocalAngle.x = MaxLimit;
        }
        if (camLocalAngle.x < MinLimit && camLocalAngle.x > 180)
        {
            camLocalAngle.x = MinLimit;
        }
        camTransform.localEulerAngles = camLocalAngle;
        var localAngle = stateManager.transform.localEulerAngles;
        //‰¡‰ñ“]‚ÍPlayerŽ©‘Ì‚ð“®‚©‚·
        localAngle.y += lookVector.x * Sensibility;
        stateManager.transform.localEulerAngles = localAngle;
    }
}
