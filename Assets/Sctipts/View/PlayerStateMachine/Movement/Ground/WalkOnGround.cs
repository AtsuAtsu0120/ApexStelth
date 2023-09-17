using System.ComponentModel.Design.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkOnGround : Walk, ICheckGround
{
    private float JumpPower = 500.0f;
    private CapsuleCollider collider;
    private RaycastHit hit;
    private Side facingSide;

    private bool isSpeedZero;
    public WalkOnGround(CharacterStateManager stateManager) : base(stateManager)
    {

    }

    public override void OnEnter()
    {
        Speed = 50.0f;
        MaxSpeed = 15.0f;

        if(stateManager.currentViewpointState.GetType() == typeof(Overlooking))
        {
            stateManager.ChangeViewPointState(new NormalViewpoint(stateManager));
        }
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
    public override void OnExit()
    {
        PlayerAudioManager.Instance.StopFootStep();
        stateManager.actionButton.SetActive(false);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        var isHorizontalMagnitudeZero = Vector3.Magnitude(horizontalForce) == 0;
        var isVerticalMagnitudeZero = Vector3.Magnitude(verticalForce) == 0;

        //慣性をなくす
        if (isHorizontalMagnitudeZero)
        {
            var localVelocity = transform.InverseTransformDirection(stateManager.rb.velocity);
            localVelocity.x = 0;
            stateManager.rb.velocity = transform.TransformDirection(localVelocity);
        }
        if (isVerticalMagnitudeZero)
        {
            var localVelocity = transform.InverseTransformDirection(stateManager.rb.velocity);
            localVelocity.z = 0;
            stateManager.rb.velocity = transform.TransformDirection(localVelocity);
        }

        isSpeedZero = isHorizontalMagnitudeZero && isVerticalMagnitudeZero;

        //設置判定
        CheckGround();


        //両方ともゼロだったら足音を消す
        if (isSpeedZero && PlayerAudioManager.Instance.GetAudioSourcePlaying())
        {
            PlayerAudioManager.Instance.StopFootStep();
        }

        //アクションボタンの表示
        var rayDistance = 8.0f;
        var rayHalfExraytents = new Vector3(0.1f, 0.1f, 0.1f);
        var rayDirection = stateManager.camTransform.forward; //プレイヤーはy方向の角度が変わらないのでカメラの正面を使う
        var enableAction = true;
        var layer = ~(1 << 6 | 1 << 7 | 1 << 3 | 1 << 2);

        if (Physics.BoxCast(stateManager.camTransform.position, rayHalfExraytents, rayDirection, out var hit, Quaternion.identity, rayDistance, layer, QueryTriggerInteraction.Ignore))
        {
            //カバー以外のアクション可能オブジェクト
            if(hit.transform.TryGetComponent<IActionable>(out var component))
            {
                action = component.OnActionKey;
                enableAction = true;
            }
            //カバー
            else
            {
                //端を計算する
                //設置点を取得
                var castPoint = hit.point;
                //設置点とオブジェクトの中心の距離を取る
                var distance = Vector3.Distance(castPoint, hit.transform.position);

                //オブジェクトの相対的な長さを取得する。
                facingSide = StelthUtility.GetFacingSide(hit);
                var hitObjectEdgeDirection = StelthUtility.GetObjectEdgeDirection(facingSide, hit.transform);

                //オブジェクトの長さを0.8倍してカバーの範囲を決定する
                hitObjectEdgeDirection *= 0.8f;

                //距離が多かったらそのオブジェクトの端であることから、カバーができるようボタンを表示する。
                if (distance > hitObjectEdgeDirection)
                {
                    OnAbleCover(hit);
                }
                else
                {
                    enableAction = false;
                }
            }
        }
        else
        {
            enableAction = false;
        }

        //アクションボタンを表示する。
        stateManager.actionButton.SetActive(enableAction);
    }

    public override void OnPerformUp(InputAction.CallbackContext ctx)
    {
        stateManager.rb.AddForce(new (0.0f, JumpPower, 0.0f), ForceMode.Acceleration);
        stateManager.ChangeState(new AirWalk(stateManager));
    }
    public override void OnPerformSprint(InputAction.CallbackContext ctx)
    {
        stateManager.ChangeState(new Sprint(stateManager));
    }
    public override void OnPerformDown(InputAction.CallbackContext ctx)
    {
        stateManager.ChangeState(new Crouch(stateManager));
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public void OnAbleCover(RaycastHit hit)
    {
        this.hit = hit;
        action = OnCoverAction;
    }
    public void OnCoverAction()
    {
        //ポジションを変更
        collider ??= transform.GetComponentInChildren<CapsuleCollider>();
        var coverPosition = facingSide switch
        {
            Side.Forward => hit.point + hit.transform.forward * collider.radius,
            Side.Backward => hit.point - hit.transform.forward * collider.radius,
            //要修正？
            Side.Right => hit.point - hit.transform.forward * collider.radius,
            Side.Left => hit.point - hit.transform.forward * collider.radius,
            //ここまで
            Side.Error => hit.point - hit.transform.forward * collider.radius,
            _ => hit.point - hit.transform.forward * collider.radius
        };
        coverPosition.y = transform.position.y;
        transform.position = coverPosition;

        //壁を向くように
        transform.rotation = facingSide switch
        {
            Side.Forward => Quaternion.AngleAxis(180, Vector3.up) * hit.transform.rotation,
            Side.Backward => hit.transform.rotation,
            Side.Right => Quaternion.AngleAxis(90, Vector3.up) * hit.transform.rotation,
            Side.Left => Quaternion.AngleAxis(270, Vector3.up) * hit.transform.rotation,
            _ => hit.transform.rotation
        };

        //ステートの変更
        stateManager.ChangeState(new Cover(stateManager));
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
            if (!isSpeedZero)
            {
                PlayerAudioManager.Instance.StartFootStep(hit.transform.tag);
            }
        }
        else
        {
            stateManager.ChangeState(new AirWalk(stateManager));
        }
    }
}
