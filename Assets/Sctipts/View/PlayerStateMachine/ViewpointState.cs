public abstract class ViewpointState
{
    protected CharacterStateManager stateManager { get; private set; }
    public ViewpointState(CharacterStateManager stateManager)
    {
        this.stateManager = stateManager;
    }
    /// <summary>
    /// このステートに切り替わったとき
    /// </summary>
    public abstract void OnEnter();
    /// <summary>
    /// このステートの時のアップデート
    /// </summary>
    public abstract void OnUpdate();
    /// <summary>
    /// このステートのときのフィックスドアップデート
    /// </summary>
    public abstract void OnFixedUpdate();
    /// <summary>
    /// このステートに切り替わったとき
    /// </summary>
    public abstract void OnExit();
}
