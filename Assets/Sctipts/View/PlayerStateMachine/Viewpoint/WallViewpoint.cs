using UnityEngine;

public class WallViewpoint : NormalViewpoint
{
    private float initAngle;
    public WallViewpoint(CharacterStateManager stateManager) : base(stateManager) {  }

    public override void OnEnter()
    {
        initAngle = stateManager.transform.localEulerAngles.y;
    }

    public override void OnExit()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var localAngle = stateManager.transform.localEulerAngles;

        var angleY = localAngle.y - initAngle;

        if (angleY > 30 && angleY < 90)
        {
            localAngle.y = 30 + initAngle;
        }
        if (angleY < -30 && angleY > -90)
        {
            localAngle.y = initAngle - 30;
        }
        stateManager.transform.localEulerAngles = localAngle;
    }
}
