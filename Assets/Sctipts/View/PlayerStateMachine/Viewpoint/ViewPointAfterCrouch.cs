public class ViewPointAfterCrouch : ViewpointState
{
    private float NormalCameraHeghit = 0.7f;
    public ViewPointAfterCrouch(CharacterStateManager stateManager) : base(stateManager) { }

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
        //‚µ‚á‚ª‚İ‚ÌƒJƒƒ‰ˆÚ“®B
        if (stateManager.camTransform.localPosition.y < NormalCameraHeghit)
        {
            var position = stateManager.camTransform.transform.localPosition;
            position.y += 0.01f;
            stateManager.camTransform.localPosition = position;
        }
        else
        {
            stateManager.ChangeViewPointState(new NormalViewpoint(stateManager));
        }
    }
}
