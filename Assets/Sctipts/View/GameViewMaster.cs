using Prashalt.Unity.ConversationGraph.Conponents;
using Prashalt.Unity.Utility.Superclass;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class GameViewMaster : SingletonMonoBehaviour<GameViewMaster>
{
    [SerializeField] private Slider confirmedPercent;
    public ConversationSystemUGUI conversationSystemUGUI { get; private set; }

    [SerializeField] private GameObject player1Obj;
    [SerializeField] private GameObject player2Obj;

    [SerializeField] private Transform player1SpawnPoint;
    [SerializeField] private Transform player2SpawnPoint;

    [SerializeField] private GameObject textUI;
    [SerializeField] private GameObject normalGUi;
    [SerializeField] private GameObject inventory;

    [SerializeField] private GameObject missionPrefab;
    [SerializeField] private GameObject missionParent;

    [SerializeField] public GameObject gameOver;
    [SerializeField] public GameObject clear;

    [SerializeField] private InventoryScroll scroll;

    private Player player1;
    private Player player2;
    protected override void Awake()
    {
        base.Awake();

        player1 = player1Obj.GetComponent<Player>();
        player2 = player2Obj.GetComponent<Player>();

        conversationSystemUGUI = GetComponent<ConversationSystemUGUI>();
    }

    protected void Start()
    {
        conversationSystemUGUI.StartConversation();
        conversationSystemUGUI.OnConversationFinishedEvent += () => textUI.SetActive(false);
        conversationSystemUGUI.OnConversationStartEvent += () => textUI.SetActive(true);

        player1Obj.transform.position = player1SpawnPoint.position;
        player2Obj.transform.position = player2SpawnPoint.position;

        UpdateMissonView();
    }
    private void Update()
    {
        var playerComponent = GetActivePlayerComponent();

        foreach (var nearEnemy in playerComponent.NearEnemies)
        {
            nearEnemy.ShotConfirmeRaycast();
        }
        confirmedPercent.value = GameLogicMaster.Instance.ConfirmedPercent;
    }

    public Player GetActivePlayerComponent()
    {
        var playerComponent = player2;
        if (GameLogicMaster.Instance.IsPlayer1Active)
        {
            playerComponent = player1;
        }

        return playerComponent;
    }
    public void ChangePlayer(CallbackContext _)
    {
        if (GameLogicMaster.Instance.IsPlayer1Active)
        {
            player1Obj.SetActive(false);
            player2Obj.SetActive(true);

            GameLogicMaster.Instance.IsPlayer1Active = false;
        }
        else
        {
            player1Obj.SetActive(true);
            player2Obj.SetActive(false);

            GameLogicMaster.Instance.IsPlayer1Active = true;
        }
    }
    public void ChangeActiveInventory()
    {
        var nowActiveSelf = inventory.activeSelf;

        inventory.SetActive(!nowActiveSelf);
        normalGUi.SetActive(nowActiveSelf);

        if (nowActiveSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0;
        }
    }
    public void UpdateMissonView()
    {
        foreach (var mission in GameLogicMaster.Instance.stageInfo.Missions)
        {
            if (mission.State != MissionState.workInProgress)
            {
                return;
            }
            var instance = Instantiate(missionPrefab, missionParent.transform);
            var texts = instance.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = mission.Name;
            texts[1].text = mission.Description;
        }
    }
    public void UpdateItem(string name)
    {
        scroll.scrollRect.totalCount = GetActivePlayerComponent().hasItems.Count;
        scroll.scrollRect.RefillCells();
    }
    public void OnClear()
    {
        clear.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;
    }
}
