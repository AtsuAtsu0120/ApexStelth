public abstract class ViewpointState
{
    protected CharacterStateManager stateManager { get; private set; }
    public ViewpointState(CharacterStateManager stateManager)
    {
        this.stateManager = stateManager;
    }
    /// <summary>
    /// ���̃X�e�[�g�ɐ؂�ւ�����Ƃ�
    /// </summary>
    public abstract void OnEnter();
    /// <summary>
    /// ���̃X�e�[�g�̎��̃A�b�v�f�[�g
    /// </summary>
    public abstract void OnUpdate();
    /// <summary>
    /// ���̃X�e�[�g�̂Ƃ��̃t�B�b�N�X�h�A�b�v�f�[�g
    /// </summary>
    public abstract void OnFixedUpdate();
    /// <summary>
    /// ���̃X�e�[�g�ɐ؂�ւ�����Ƃ�
    /// </summary>
    public abstract void OnExit();
}
