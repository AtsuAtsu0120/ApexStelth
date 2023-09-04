using Cysharp.Threading.Tasks;
using Prashalt.Unity.ConversationGraph.Conponents;
using Prashalt.Unity.Utility.Superclass;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class GameViewMaster : SingletonMonoBehaviour<GameViewMaster>
{
    [SerializeField] private Slider confirmedPercent;
    [SerializeField] private ConversationSystemUGUI conversationSystemUGUI;

    [SerializeField] private GameObject player1Obj;
    [SerializeField] private GameObject player2Obj;

    [SerializeField] private Transform player1SpawnPoint;
    [SerializeField] private Transform player2SpawnPoint;

    [SerializeField] private GameObject textUI;

    private Player player1;
    private Player player2;
    private bool isPlayer1Active;
    protected override void Awake()
    {
        base.Awake();

        player1 = player1Obj.GetComponent<Player>();
        player2 = player2Obj.GetComponent<Player>();
    }

    protected void Start()
    {
        conversationSystemUGUI.StartConversation();
        conversationSystemUGUI.OnConversationFinishedEvent += () => textUI.SetActive(false);

        player1Obj.transform.position= player1SpawnPoint.position;
        player2Obj.transform.position = player2SpawnPoint.position;
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
        if (isPlayer1Active)
        {
            player1Obj.SetActive(false);
            player2Obj.SetActive(true);

            isPlayer1Active = false;
        }
        else
        {
            player1Obj.SetActive(true);
            player2Obj.SetActive(false);

            isPlayer1Active = true;
        }
    }
}
