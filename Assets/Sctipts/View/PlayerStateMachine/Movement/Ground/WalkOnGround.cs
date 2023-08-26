using UnityEngine;
using UnityEngine.InputSystem;

public class WalkOnGround : Walk
{
    private float JumpPower = 1000.0f;
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
        PlayerAudioManager.Instance.StartFootStep(collision.transform.tag);
    }
    public override void OnCollisionStay(Collision collision)
    {
        if(!isSpeedZero)
        {
            PlayerAudioManager.Instance.StartFootStep(collision.transform.tag);
        }
    }
    public override void OnExitCollider(Collision collision)
    {
        stateManager.ChangeState(new AirWalk(stateManager));
    }
    public override void OnExit()
    {
        PlayerAudioManager.Instance.StopFootStep();
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

        //両方ともゼロだったら足音を消す
        if (isSpeedZero && PlayerAudioManager.Instance.GetAudioSourcePlaying())
        {
            PlayerAudioManager.Instance.StopFootStep();
            stateManager.animator.SetTrigger("Idle");
        }
        else
        {
            stateManager.animator.SetTrigger("Walk");
        }

        //アクションボタンの表示
        var rayOffsetY = 0.5f;
        var rayDistance = 8.0f;
        var rayHalfExraytents = new Vector3(0.2f, 0.2f, 0.2f);
        var rayDirection = stateManager.camTransform.forward; //プレイヤーはy方向の角度が変わらないのでカメラの正面を使う
        var isAbleAction = true;
        var layer = ~(1 << 6 | 1 << 7 | 1 << 3);
        if (Physics.BoxCast(transform.position + Vector3.up * rayOffsetY, rayHalfExraytents, rayDirection, out var hit, Quaternion.identity, rayDistance, layer, QueryTriggerInteraction.Ignore))
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
                isAbleAction = false;
            }
        }
        else
        {
            isAbleAction = false;
        }

        //アクションボタンを非表示にする。
        if (!isAbleAction)
        {
            stateManager.actionButton.SetActive(false);
        }
    }

    public override void OnPerformUp(InputAction.CallbackContext ctx)
    {
        stateManager.rb.AddForce(new (0.0f, JumpPower, 0.0f));
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
        stateManager.actionButton.SetActive(true);
        this.hit = hit;
        action = OnCoverAction;
    }
    public void OnCoverAction()
    {
        //ポジションを変更
        collider ??= transform.GetComponent<CapsuleCollider>();
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
}
