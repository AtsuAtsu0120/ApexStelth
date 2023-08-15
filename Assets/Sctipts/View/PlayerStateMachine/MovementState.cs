using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MovementState
{
    protected CharacterStateManager stateManager { get; private set; }
    protected Action action { get; set; }
    public MovementState(CharacterStateManager stateManager)
    {
        this.stateManager = stateManager;
        action = () => { };
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
    /// <summary>
    /// 衝突判定
    /// </summary>
    /// <param name="collider"></param>
    public abstract void OnEnterCollider(Collision collision);
    /// <summary>
    /// ステートが変わったタイミングで一度だけ呼ばれる接触判定。接触していないときは呼ばれない。
    /// </summary>
    /// <param name="collision"></param>
    public abstract void OnCollisionStay(Collision collision);
    public abstract void OnExitCollider(Collision collision);
    public abstract void OnPerformUp(InputAction.CallbackContext ctx);
    public abstract void OnCancelUp(InputAction.CallbackContext ctx);
    public abstract void OnPerformDown(InputAction.CallbackContext ctx);
    public abstract void OnCancelDown(InputAction.CallbackContext ctx);
    public abstract void OnPerformSprint(InputAction.CallbackContext ctx);
    public abstract void OnCancelSprint(InputAction.CallbackContext ctx);
    public virtual void OnPerformAction(InputAction.CallbackContext ctx)
    {
        action.Invoke();
    }
}
