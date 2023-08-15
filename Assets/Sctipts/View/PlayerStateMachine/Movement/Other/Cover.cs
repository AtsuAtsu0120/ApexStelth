using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cover : MovementState
{
    private Vector2 inputVector;
    private float MaxSpeed = 5.0f;
    private float Speed = 50.0f;
    public Cover(CharacterStateManager stateManager) : base(stateManager) { }

    public override void OnCancelDown(InputAction.CallbackContext ctx)
    {
        
    }

    public override void OnCancelSprint(InputAction.CallbackContext ctx)
    {
        
    }

    public override void OnCancelUp(InputAction.CallbackContext ctx)
    {
        
    }

    public override void OnEnter()
    {
        stateManager.ChangeViewPointState(new Overlooking(stateManager));
        stateManager.actionButton.SetActive(false);
    }

    public override void OnEnterCollider(Collision collision)
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnExitCollider(Collision collision)
    {
        throw new System.NotImplementedException();
    }

    public override void OnFixedUpdate()
    {
        var transform = stateManager.transform;
        var edgeType = EdgeType.None;
        //もし壁を超えそうだったら止まる
        if (Physics.BoxCast(transform.position + Vector3.up * 0.5f, new Vector3(0.2f, 0.2f, 0.2f), stateManager.camTransform.forward, out var hit, Quaternion.identity, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.tag.Contains("Ground"))
            {
                return;
            }
            //端を計算する
            //設置点を取得
            var castPoint = hit.point;
            //設置点とオブジェクトの中心の距離を取る
            var distance = Vector3.Distance(castPoint, hit.transform.position);

            //オブジェクトの相対的な長さを取得する。
            var facingSide = StelthUtility.GetFacingSide(hit);
            var coverArea = StelthUtility.GetObjectEdgeDirection(facingSide, hit.transform);
            coverArea *= 0.95f;
            //距離が多かったらそのオブジェクトの端であることから、それ以上動けないようにする。
            if (distance > coverArea)
            {
                //TODO: EdgeTypeが逆になる場合があると思うので修正求
                var positionMagnitude = transform.position.sqrMagnitude;
                var hitPositionMagnitude = hit.transform.position.sqrMagnitude;

                var magnitudeDifference = positionMagnitude - hitPositionMagnitude;
                if (magnitudeDifference > 0)
                {
                    edgeType = EdgeType.Right;
                }
                else
                {
                    edgeType = EdgeType.Left;
                }
            }
            else
            {
                edgeType = EdgeType.None;
            }
        }
        //キャラ的には正面。視点的には左右移動。
        float3 forward = new(transform.right.x, 0, transform.right.z);

        if(inputVector.x < 0 && edgeType == EdgeType.Left)
        {
            inputVector.x = 0;
            MaxSpeed = 0.0f;
        }
        else if(inputVector.x > 0 && edgeType == EdgeType.Right)
        {
            inputVector.x = 0;
            MaxSpeed = 0.0f;
        }
        else
        {
            MaxSpeed = 5.0f;
        }
        //入力から計算する。
        var horizontalForce = forward * inputVector.x;
        //力を与える。
        stateManager.rb.AddForce(horizontalForce);

        var velocityWithoutY = new Vector3(stateManager.rb.velocity.x, 0f, stateManager.rb.velocity.z);
        //最大速度に減少させる。
        velocityWithoutY = Vector3.ClampMagnitude(velocityWithoutY, MaxSpeed);

        stateManager.rb.velocity = new(velocityWithoutY.x, stateManager.rb.velocity.y, velocityWithoutY.z);
    }

    public override void OnPerformAction(InputAction.CallbackContext ctx)
    {
        stateManager.ChangeState(new WalkOnGround(stateManager));
    }

    public override void OnPerformDown(InputAction.CallbackContext ctx)
    {
        
    }

    public override void OnPerformSprint(InputAction.CallbackContext ctx)
    {
        
    }

    public override void OnPerformUp(InputAction.CallbackContext ctx)
    {
        
    }

    public override void OnCollisionStay(Collision collision)
    {
        
    }

    public override void OnUpdate()
    {
        //移動
        float2 movementVector = stateManager.moveAction.ReadValue<Vector2>();

        movementVector *= Speed;
        var input = inputVector;
        input.x = movementVector.x;
        inputVector = input;
    }
    public enum EdgeType
    {
        None,
        Right,
        Left,
    }
}