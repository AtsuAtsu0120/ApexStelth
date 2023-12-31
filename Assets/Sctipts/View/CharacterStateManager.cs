using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class CharacterStateManager : MonoBehaviour
{
    #region Fields_InputSystem

    private StelthInputActions actions;
    [HideInInspector] public InputAction moveAction;
    [HideInInspector] public InputAction lookAction;

    private InputAction upAction;
    private InputAction downAction;
    private InputAction sprintAction;
    private InputAction action;
    private InputAction playerChange;
    private InputAction openInventry;

    private bool isInited = false;

    #endregion

    #region Fields_Other

    public Rigidbody rb;

    public Transform camTransform;

    public float3 inputVector;

    public GameObject actionButton;

    public TextMeshProUGUI actionText;

    public Animator animator;

    private ViewpointState previousViewpoint;

    #endregion

    #region Fields_State

    public MovementState currentMovementState { get; private set; }
    public ViewpointState currentViewpointState { get; private set; }

    #endregion

    public void Start()
    {
        //カーソルの無効化
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        if (!isInited)
        {
            actionText = actionButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            //InputSystemの初期化
            GameLogicMaster.Instance.Actions ??= new();
            actions = GameLogicMaster.Instance.Actions;

            actions.Enable();
            moveAction = actions.Player.Move;
            lookAction = actions.Player.Look;

            upAction = actions.Player.Up;
            downAction = actions.Player.Down;
            sprintAction = actions.Player.Sprint;

            action = actions.Player.Action;
            openInventry = actions.Player.Inventory;
            playerChange = actions.Player.PlayerChange;

            upAction.Enable();
            downAction.Enable();
            sprintAction.Enable();
            action.Enable();
            playerChange.Enable();
            openInventry.Enable();

            playerChange.performed += GameViewMaster.Instance.ChangePlayer;
            openInventry.performed += OnChangeGUI;
            //ここまで

            PlayerAudioManager.Instance.ChangeActivePlayer();

            isInited = true;

            //Rigidbodyの取得
            rb = transform.GetComponent<Rigidbody>();
            animator = transform.GetComponent<Animator>();

            //カメラの場所を取得
            camTransform = transform.GetChild(0).transform;
        }

        ChangeState(new AirWalk(this));

        currentViewpointState = new NormalViewpoint(this);
    }

    private void OpenInventry_performed(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        currentMovementState?.OnUpdate();
        currentViewpointState?.OnUpdate();
    }
    private void FixedUpdate()
    {
        animator.SetFloat("Speed", rb.velocity.z + rb.velocity.x);
        currentMovementState?.OnFixedUpdate();
    }
    private void OnCollisionEnter(Collision collision)
    {
        currentMovementState.OnEnterCollider(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        currentMovementState.OnCollisionStay(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        currentMovementState.OnExitCollider(collision);
    }
    public void ChangeState(MovementState newState)
    {
        ////キーをリセット
        if (currentMovementState is not null)
        {
            upAction.performed -= currentMovementState.OnPerformUp;
            downAction.performed -= currentMovementState.OnPerformDown;
            downAction.canceled -= currentMovementState.OnCancelDown;
            sprintAction.performed -= currentMovementState.OnPerformSprint;
            sprintAction.canceled -= currentMovementState.OnCancelSprint;
            action.performed -= currentMovementState.OnPerformAction;
        }

        currentMovementState?.OnExit();
        currentMovementState = newState;

        //キーを設定
        upAction.performed += currentMovementState.OnPerformUp;
        upAction.canceled += currentMovementState.OnCancelDown;
        downAction.performed += currentMovementState.OnPerformDown;
        downAction.canceled += currentMovementState.OnCancelDown;
        sprintAction.performed += currentMovementState.OnPerformSprint;
        sprintAction.canceled += currentMovementState.OnCancelSprint;
        action.performed += currentMovementState.OnPerformAction;

        currentMovementState.OnEnter();
    }
    public void ChangeViewPointState(ViewpointState state)
    {
        currentViewpointState?.OnExit();
        currentViewpointState = state;
        currentViewpointState.OnEnter();
    }
    public void OnChangeGUI(CallbackContext _)
    {
        GameViewMaster.Instance.ChangeActiveInventory();
        if(currentViewpointState.GetType() == typeof(LockViewPoint))
        {
            ChangeViewPointState(previousViewpoint);
        }
        else
        {
            previousViewpoint = currentViewpointState;
            ChangeViewPointState(new LockViewPoint(this));
        }

    }
}
