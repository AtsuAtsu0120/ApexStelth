using UnityEngine;

public class StelthEnemyComponent : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private bool isReseted = false;
    private bool isQuitting = false;
    public bool ShotConfirmeRaycast()
    {
        //敵がプレイヤーを見つける用の処理
        if(Physics.BoxCast(transform.position + Vector3.up * 0.5f, new Vector3(50f, 0.2f, 0.2f), transform.forward, out var hit, Quaternion.identity, 10.0f, 1 << 6, QueryTriggerInteraction.Ignore))
        {
            if (StelthUtility.IsHitInAngle(hit.transform, transform, 1.4f, 0.6f))
            {
                //DeltaTimeじゃないほうがいい説ある。
                GameLogicMaster.Instance.StartConfirmePlayerByEnemyJob(Time.deltaTime);
                isReseted = true;
            }
            return true;
        }
        else
        {
            if(isReseted)
            {
                isReseted = false;
                GameLogicMaster.Instance.StartResetConfirmedPercent();
            }
            return false;
        }
    }
    public void OnApplicationQuit()
    {
        isQuitting = true;
    }
    public void OnDestroy()
    {
        if (isQuitting) return;
        var playerComponent = GameViewMaster.Instance.GetActivePlayerComponent();
        
        if (playerComponent == null) return;
        playerComponent.TryRemoveNearEnemy(gameObject);
    }
}
