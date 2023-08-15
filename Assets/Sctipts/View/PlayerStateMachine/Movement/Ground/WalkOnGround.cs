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
        PlayerAudioManager.Instance.StopFootStep();
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
        //�������Ȃ���
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
        //�����Ƃ��[���������瑫��������
        if (isSpeedZero && PlayerAudioManager.Instance.GetAudioSourcePlaying())
        {
            PlayerAudioManager.Instance.StopFootStep();
        }

        //�A�N�V�����{�^���̕\��
        var rayOffsetY = 0.5f;
        var rayDistance = 8.0f;
        var rayHalfExraytents = new Vector3(0.2f, 0.2f, 0.2f);
        var rayDirection = stateManager.camTransform.forward; //�v���C���[��y�����̊p�x���ς��Ȃ��̂ŃJ�����̐��ʂ��g��
        var isAbleAction = true;
        var layer = ~(1 << 6 | 1 << 7 | 1 << 3);
        if (Physics.BoxCast(transform.position + Vector3.up * rayOffsetY, rayHalfExraytents, rayDirection, out var hit, Quaternion.identity, rayDistance, layer, QueryTriggerInteraction.Ignore))
        {
            //�[���v�Z����
            //�ݒu�_���擾
            var castPoint = hit.point;
            //�ݒu�_�ƃI�u�W�F�N�g�̒��S�̋��������
            var distance = Vector3.Distance(castPoint, hit.transform.position);

            //�I�u�W�F�N�g�̑��ΓI�Ȓ������擾����B
            facingSide = StelthUtility.GetFacingSide(hit);
            var hitObjectEdgeDirection = StelthUtility.GetObjectEdgeDirection(facingSide, hit.transform);

            //�I�u�W�F�N�g�̒�����0.8�{���ăJ�o�[�͈̔͂����肷��
            hitObjectEdgeDirection *= 0.8f;

            //���������������炻�̃I�u�W�F�N�g�̒[�ł��邱�Ƃ���A�J�o�[���ł���悤�{�^����\������B
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

        //�A�N�V�����{�^�����\���ɂ���B
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
        //�|�W�V������ύX
        collider ??= transform.GetComponent<CapsuleCollider>();
        var coverPosition = facingSide switch
        {
            Side.Forward => hit.point + hit.transform.forward * collider.radius,
            Side.Backward => hit.point - hit.transform.forward * collider.radius,
            //�v�C���H
            Side.Right => hit.point - hit.transform.forward * collider.radius,
            Side.Left => hit.point - hit.transform.forward * collider.radius,
            //�����܂�
            Side.Error => hit.point - hit.transform.forward * collider.radius,
            _ => hit.point - hit.transform.forward * collider.radius
        };
        coverPosition.y = transform.position.y;
        transform.position = coverPosition;

        //�ǂ������悤��
        transform.rotation = facingSide switch
        {
            Side.Forward => Quaternion.AngleAxis(180, Vector3.up) * hit.transform.rotation,
            Side.Backward => hit.transform.rotation,
            Side.Right => Quaternion.AngleAxis(90, Vector3.up) * hit.transform.rotation,
            Side.Left => Quaternion.AngleAxis(270, Vector3.up) * hit.transform.rotation,
            _ => hit.transform.rotation
        };

        //�X�e�[�g�̕ύX
        stateManager.ChangeState(new Cover(stateManager));
    }
}