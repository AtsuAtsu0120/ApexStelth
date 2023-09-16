using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbWall : MovementState
{
    private float MaxClimbSpeed = 5.0f;
    private float Speed = 50.0f;
    private RaycastHit hit;
    public ClimbWall(CharacterStateManager stateManager, RaycastHit hit) : base(stateManager)
    {
        this.hit = hit;
    }
    protected Vector3 inputVector { get; set; }
    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnFixedUpdate()
    {
        if (MaxClimbSpeed < 0)
        {
            Debug.Log("Speed���[����");
            CancelWall();
        }
        else
        {
            MaxClimbSpeed -= 0.05f;
            //��ƉE�����̃x�N�g�����擾
            float3 up = new(0, stateManager.transform.up.y, 0);
            float3 right = new(stateManager.transform.right.x, 0, stateManager.transform.right.z);

            //���͂���v�Z����B
            var horizontalForce = right * inputVector.x;
            var upForce = up * inputVector.z;

            stateManager.rb.AddForce(upForce);
            stateManager.rb.AddForce(horizontalForce);
            //�ő呬�x��ݒ�
            stateManager.rb.velocity = Vector3.ClampMagnitude(stateManager.rb.velocity, MaxClimbSpeed);
        }
        //�ǂ��Ȃ��Ȃ������߂�p
        if (!Physics.Raycast(stateManager.camTransform.position, stateManager.camTransform.forward, 3.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("�ǂ��Ȃ��ł�");
            CancelWall();
        }

    }
    public override void OnUpdate()
    {
        //�ړ�
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

    public override void OnPerformDown(InputAction.CallbackContext ctx)
    {
        Debug.Log("���Ⴊ�񂾂̂�");
        CancelWall();
    }

    public override void OnPerformSprint(InputAction.CallbackContext ctx)
    {
        
    }

    //�ǃW����
    public override void OnPerformUp(InputAction.CallbackContext ctx)
    {
        var fwdSpeed = Vector3.Dot(stateManager.rb.velocity, stateManager.transform.forward);
        if (fwdSpeed > 0.5)
        {
            var refrectVector = Vector3.Reflect(stateManager.transform.forward, hit.normal);
            refrectVector.y += 0.3f;
            stateManager.rb.AddForce(refrectVector * 1000, ForceMode.Acceleration);
        }
        Debug.Log("�ǃW���������̂�");
        CancelWall();
    }

    public override void OnCancelDown(InputAction.CallbackContext ctx)
    {
        
    }

    public override void OnCancelSprint(InputAction.CallbackContext ctx)
    {
        
    }
    public override void OnPerformAction(InputAction.CallbackContext ctx)
    {

    }

    public override void OnCancelUp(InputAction.CallbackContext ctx)
    {
    }

    private void CancelWall()
    {
        Debug.Log("���炩�̗��R�ŃN���C�~���O���L�����Z��");
        stateManager.ChangeState(new AirAfterClimb(stateManager));
        stateManager.ChangeViewPointState(new NormalViewpoint(stateManager));
    }

    public override void OnCollisionStay(Collision collision)
    {
        
    }

    public override void OnExitCollider(Collision collision)
    {
        
    }
}
