using UnityEngine;

public class StelthEnemyComponent : StelthNPC
{
    private bool isQuitting = false;

    public override void Start()
    {
        base.Start();
        isVigilance = true;
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
