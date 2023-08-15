using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    #region Fields_FromInspector

    [Header("Value Settings")]
    [SerializeField] private float Speed = 50.0f;
    [SerializeField] private float JumpPower = 1000.0f;
    [SerializeField] private float Sensibility = 1.0f;
    [SerializeField] private float CrounchSpeed = 25.0f;
    [SerializeField] private float CrounchBoost = 3.0f;
    [SerializeField] private float CrouchCameraHeghit = 0.2f;

    [SerializeField] private float MaxSpeed = 8.0f;
    [SerializeField] private float MaxCrouchSpeed = 15.0f;
    [SerializeField] private float MaxSprintSpeed = 13.0f;
    [SerializeField] private float MaxClimbSpeed = 5.0f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI text;
    [Header("Layer")]
    [SerializeField] private LayerMask WallLayer;

    #endregion

    #region Fields_InputSystem

    private StelthInputActions actions;
    private InputAction moveAction;
    private InputAction lookAction;

    private InputAction upAction;
    private InputAction downAction;
    private InputAction sprintAction;

    #endregion

    #region Fields_Other

    private Rigidbody rb;
    private Transform camTransform;
    private float3 inputVector;

    private float crouchCooltime;
    private float standupCameraHeghit;

    private bool isWallMode = false;

    private bool onGround = false;
    private bool onWall = false;

    private bool isCrouch = false;
    private bool isSprintBeforeCrouch;
    private bool isSprint = false;
    private bool isJumping = false;

    private bool isStraight = false;

    private bool fixedHorizontalLook = false;

    #endregion
    #region Fields_Constant

    private readonly float MaxLimit = 85;
    private readonly float MinLimit = 360 - 85;
    private readonly float StraightBonus = 2;
    private float DefaultMaxCrouchSpeed;
    private float DefaultMaxClimbSpeed;

    /// <summary>
    /// 壁のぼりに入ったタイミングの角度を保存しておく変数。
    /// </summary>
    private float wallModeAngleY;
    private GameObject wall;

    #endregion

    #region Methods_Unity
    private void Start()
    {
        //カーソルの無効化
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //InputSystemの初期化
        actions = new();
        actions.Enable();
        moveAction = actions.Player.Move;
        lookAction = actions.Player.Look;

        upAction = actions.Player.Up;
        upAction.performed += PerformUpAction;

        downAction = actions.Player.Down;
        downAction.performed += PerformDownAction;
        downAction.canceled += CancelDownAction;

        sprintAction = actions.Player.Sprint;
        sprintAction.performed += PerformSprintAction;
        sprintAction.canceled += CancelSpritAction;
        //ここまで

        //Rigidbodyの取得
        rb = GetComponent<Rigidbody>();

        //子のカメラを取得
        camTransform = transform.GetChild(0).transform;
        standupCameraHeghit = camTransform.localPosition.y;

        //しゃがんでいるときの速度を保存しておく
        DefaultMaxCrouchSpeed = MaxCrouchSpeed;
        DefaultMaxClimbSpeed = MaxClimbSpeed;
    }
    private void OnDisable()
    {
        actions.Disable();
        actions = null;
    }
    private void Update()
    {
        Profiler.BeginSample("CharacterController-Update");
        //視点移動
        var lookVector = lookAction.ReadValue<Vector2>();
        var camTransfrom = transform.GetChild(0);
        //縦回転はカメラを動かす
        var camLocalAngle = camTransform.localEulerAngles;
        camLocalAngle.x += -lookVector.y * Sensibility;
        //縦回転は制限あり
        if (camLocalAngle.x > MaxLimit && camLocalAngle.x < 180)
        {
            camLocalAngle.x = MaxLimit;
        }
        if (camLocalAngle.x < MinLimit && camLocalAngle.x > 180)
        {
            camLocalAngle.x = MinLimit;
        }
        camTransform.localEulerAngles = camLocalAngle;
        var localAngle = transform.localEulerAngles;
        //横回転はPlayer自体を動かす
        localAngle.y += lookVector.x * Sensibility;
        //壁のぼり中は制限あり
        if (fixedHorizontalLook)
        {
            var angleY = localAngle.y - wallModeAngleY;

            if (angleY > 30 && angleY < 0)
            {
                localAngle.y = 30;
            }
            if (angleY < -30 && angleY > 0)
            {  
                localAngle.y = 330;
            }
        }
        transform.localEulerAngles = localAngle;

        //移動
        float2 movementVector = moveAction.ReadValue<Vector2>();

        movementVector *= Speed;
        inputVector.x = movementVector.x;
        inputVector.z = movementVector.y;
        Profiler.EndSample();
    }
    private void FixedUpdate()
    {
        Profiler.BeginSample("CharacterController-FixedUpdate");
        onWall = GetWallStatus(0.5f, 3.0f, transform.forward);
        #region CharactorMovement

        //壁のぼり
        var (_, verticalForce) = GetDirectionForce(inputVector, transform);
        if (onWall && GetStraghitStatus(verticalForce) && isJumping)
        {
            //壁の方向を見る。
            var direction = wall.transform.position - transform.position;
            var lookAngle = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAngle, 0.1f);
            wallModeAngleY = transform.localEulerAngles.y;

            //壁モードにする。
            isWallMode = true;
            fixedHorizontalLook = true;
            Debug.Log("ウォールモード");
        }
        else if(!onWall)
        {
            isWallMode = false;
            fixedHorizontalLook = false;
        }

        //壁登りの時
        if (isWallMode)
        {
            AddForceUp(rb, transform, inputVector);
        }
        //地上の時
        else
        {
            //重力
            if (!onGround)
            {
                inputVector.y = Physics.gravity.y;
            }

            //スライディングのクールタイムの減少
            if (crouchCooltime >= 0.0f)
            {
                crouchCooltime -= Time.deltaTime;
            }

            AddForceForward(rb, transform, inputVector);
        }

        #endregion

        #region CrounchCamera

        //しゃがみのカメラ移動。
        if (isCrouch && camTransform.localPosition.y > CrouchCameraHeghit)
        {
            var position = camTransform.transform.localPosition;
            position.y -= 0.01f;
            camTransform.localPosition = position;
        }
        else if(!isCrouch && camTransform.localPosition.y < standupCameraHeghit)
        {
            var position = camTransform.transform.localPosition;
            position.y += 0.01f;
            camTransform.localPosition = position;
        }

        #endregion
        Profiler.EndSample();
    }
    #endregion

    #region Methods_Action

    private void PerformUpAction(InputAction.CallbackContext ctx)
    {
        if (!onGround) return;
        rb.AddForce(new float3(0, JumpPower, 0));
        //ジャンプ中
        isJumping = true;
    }
    private void PerformDownAction(InputAction.CallbackContext ctx)
    {
        //壁につかまっていたら壁のぼりをキャンセル
        if(isWallMode)
        {
            isWallMode = false;
            fixedHorizontalLook = false;
        }
        else
        {
            isCrouch = true;
            isSprintBeforeCrouch = isSprint;
            isSprint = false;
            if (crouchCooltime <= 0.0f)
            {
                inputVector.z += CrounchBoost;
                crouchCooltime = 5.0f;
            }
        }
    }
    private void CancelDownAction(InputAction.CallbackContext ctx)
    {
        isCrouch = false;
        isSprint = isSprintBeforeCrouch;

        //減速している速度をリセット
        MaxCrouchSpeed = DefaultMaxCrouchSpeed;
    }
    private void PerformSprintAction(InputAction.CallbackContext ctx)
    {
        isSprint = true;
        isCrouch = false;
        isJumping = true;
    }
    private void CancelSpritAction(InputAction.CallbackContext ctx)
    {
        isSprint = false;
    }

    #endregion

    #region Methods_Collision

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            onGround = true;
            MaxClimbSpeed = DefaultMaxClimbSpeed;
            fixedHorizontalLook = false;
            isJumping = false;
            isWallMode = false;
            inputVector.y = 0;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            onGround = false;
            isJumping = false;
        }
    }

    #endregion

    #region Methods_Other

    private void AddForceForward(Rigidbody rb, Transform transfrom, float3 inputVector)
    {
        //方向ごとに取得
        var force = GetDirectionForce(inputVector, transform);
        var verticalForce = force.verticalForce;
        var horizontalForce = force.horizontalForce;

        //力を与える
        rb.AddForce(verticalForce);
        rb.AddForce(horizontalForce);

        //方向によって速度ボーナスを与えれる。
        isStraight = GetStraghitStatus(verticalForce);

        //重力を与える。
        rb.AddForce(new(0, inputVector.y, 0), ForceMode.Acceleration);

        //状態に応じて挙動を変更
        if(onGround)
        {
            var speed = MaxSpeed;
            if(isCrouch)
            {
                //しゃがみ続けると減速
                if(MaxCrouchSpeed > 2.0f)
                {
                    MaxCrouchSpeed -= 0.1f;
                }
                //しゃがみ最大速度
                speed = MaxCrouchSpeed;
            }
            else if(isSprint)
            {
                //ダッシュの最大速度
                speed = MaxSprintSpeed;
            }

            //まっすぐ進んでいたら速度にボーナスを与える。
            if(isStraight)
            {
                speed *= StraightBonus;
            }
            //最大速度を設定
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);

            //慣性をなくす:しゃがんでいないときは慣性なし
            if (!isCrouch)
            {
                if (Vector3.Magnitude(horizontalForce) == 0)
                {
                    var localVelocity = transform.InverseTransformDirection(rb.velocity);
                    localVelocity.x = 0;
                    rb.velocity = transform.TransformDirection(localVelocity);
                }
                if (Vector3.Magnitude(verticalForce) == 0)
                {
                    var localVelocity = transform.InverseTransformDirection(rb.velocity);
                    localVelocity.z = 0;
                    rb.velocity = transform.TransformDirection(localVelocity);
                }
            }
        }
        text.text = "Speed:" + rb.velocity.magnitude.ToString();
    }
    public void AddForceUp(Rigidbody rb, Transform transfrom, float3 inputVector)
    {
        var speed = MaxClimbSpeed;
        if (speed < 0)
        {
            Debug.Log("スピードが0以下になったよ");
            //重力を与える。
            rb.AddForce(new(0, Physics.gravity.y, 0), ForceMode.Acceleration);
            isWallMode = false;
            fixedHorizontalLook = false;
        }
        else
        {
            MaxClimbSpeed -= 0.05f;
            var (upForce, horizontalForce) = GetWallForce(inputVector, transform);
            rb.AddForce(upForce);
            rb.AddForce(horizontalForce);
            //最大速度を設定
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
        }
    }
    private bool GetWallStatus(float offsetY, float distance, Vector3 direction)
    {
        Debug.DrawRay(transform.position + Vector3.up * offsetY, direction, Color.red, distance + 0.5f);
        if (Physics.Raycast(transform.position + Vector3.up * offsetY, direction, out var hit, distance + 0.5f, WallLayer, QueryTriggerInteraction.Ignore))
        {
            wall = hit.transform.gameObject;
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 前方入力をしているかどうか判定
    /// </summary>
    /// <param name="verticalForce"></param>
    /// <returns></returns>
    private bool GetStraghitStatus(Vector3 verticalForce)
    {
        var forceLocalDirection = transform.InverseTransformDirection(verticalForce); 
        if(isWallMode)
        {
            if(forceLocalDirection.y > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (forceLocalDirection.z > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private (Vector3 horizontalForce, Vector3 verticalForce) GetDirectionForce(Vector3 inputVector, Transform transform)
    {
        //正面と右方向のベクトルを取得
        float3 forward = new(transform.forward.x, 0, transform.forward.z);
        float3 right = new(transform.right.x, 0, transform.right.z);

        //入力から計算する。
        var horizontalForce = right * inputVector.x;
        var verticalForce = forward * inputVector.z;

        return (horizontalForce, verticalForce);
    }
    private (Vector3 upForce, Vector3 horizontalForce) GetWallForce(Vector3 inputVector, Transform transform)
    {
        //上と右方向のベクトルを取得
        float3 up = new(0, transform.up.y, 0);
        float3 right = new(transform.right.x, 0, transform.right.z);

        //入力から計算する。
        var horizontalForce = right * inputVector.x;
        var upForce = up * inputVector.z;

        return (upForce, horizontalForce);
    }

    #endregion
}
