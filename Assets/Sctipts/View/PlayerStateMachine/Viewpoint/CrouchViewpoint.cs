public class CrouchViewpoint : ViewpointState
{
    public CrouchViewpoint(CharacterStateManager stateManager) : base(stateManager) { }

    private float CrouchCameraHeghit = 0.2f;

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnUpdate()
    {
        //‚µ‚á‚ª‚Ý‚ÌƒJƒƒ‰ˆÚ“®B
        if (stateManager.camTransform.localPosition.y > CrouchCameraHeghit)
        {
            var position = stateManager.camTransform.transform.localPosition;
            position.y -= 0.01f;
            stateManager.camTransform.localPosition = position;
        }
    }
}
