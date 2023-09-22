using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Walk : MovementState
{
    //設定項目
    protected float Speed { get; set; }
    protected float MaxSpeed { get; set; }
    protected float StraightBonus { get; set; }

    //プロパティ
    protected Vector3 horizontalForce { get; private set; }
    protected Vector3 verticalForce { get; private set; }
    protected Vector3 inputVector { get; set; }
    protected Transform transform { get; private set; }

    public Walk(CharacterStateManager stateManager) : base(stateManager)
    {
        Speed = 50.0f;
        MaxSpeed = 15.0f;
        StraightBonus = 2.0f;
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnFixedUpdate()
    {
        transform = stateManager.transform;
        //正面と右方向のベクトルを取得
        float3 forward = new(transform.forward.x, 0, transform.forward.z);
        float3 right = new(transform.right.x, 0, transform.right.z);

        //入力から計算する。
        horizontalForce = right * inputVector.x;
        verticalForce = forward * inputVector.z;

        //力を与える
        stateManager.rb.AddForce(verticalForce, ForceMode.Acceleration);
        stateManager.rb.AddForce(horizontalForce, ForceMode.Acceleration);

        var forceLocalDirection = transform.InverseTransformDirection(verticalForce);
        //方向によって速度ボーナスを与えれる。
        var isStraight = false;
        if (forceLocalDirection.y > 0)
        {
            isStraight = true;
        }
        var speed = MaxSpeed;
        //まっすぐ進んでいたら速度にボーナスを与える。
        if (isStraight)
        {
            speed *= StraightBonus;
        }
        var velocityWithoutY = new Vector3(stateManager.rb.velocity.x, 0f, stateManager.rb.velocity.z);
        //最大速度に減少させる
        velocityWithoutY = Vector3.ClampMagnitude(velocityWithoutY, speed);

        stateManager.rb.velocity = new(velocityWithoutY.x, stateManager.rb.velocity.y, velocityWithoutY.z);        
    }

    public override void OnUpdate()
    {
        //移動
        float2 movementVector = stateManager.moveAction.ReadValue<Vector2>();

        movementVector *= Speed;
        var input = inputVector;
        input.x = movementVector.x;
        input.z = movementVector.y;
        inputVector = input;
    }

    public override void OnEnterCollider(Collision collision)
    {

    }
    public override void OnPerformUp(InputAction.CallbackContext ctx)
    {

    }
    public override void OnCancelUp(InputAction.CallbackContext ctx)
    {

    }
    public override void OnPerformDown(InputAction.CallbackContext ctx)
    {

    }
    public override void OnCancelDown(InputAction.CallbackContext ctx)
    {

    }
    public override void OnPerformSprint(InputAction.CallbackContext ctx)
    {
        
    }
    public override void OnCancelSprint(InputAction.CallbackContext ctx)
    {
        
    }
    public override void OnPerformAction(InputAction.CallbackContext ctx)
    {
        base.OnPerformAction(ctx);
    }
}
