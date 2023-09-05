using Prashalt.Unity.Utility.Superclass;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<StelthEnemyComponent> _nearEnemies;
    public ReadOnlyCollection<StelthEnemyComponent> NearEnemies { get; private set; }

    private int playerNumber;
    protected void Awake()
    {
        if(_nearEnemies != null) { return; }

        _nearEnemies = new();
        NearEnemies = _nearEnemies.AsReadOnly();
    }

    private void FixedUpdate()
    {
        ShotEnemyRaycast();
    }

    private void OnDisable()
    {
        _nearEnemies.Clear();
    }

    private void ShotEnemyRaycast()
    {
        //敵がプレイヤーを見つける用の処理
        if (Physics.BoxCast(transform.position + Vector3.up * 0.5f, new Vector3(5f, 0.2f, 0.2f), transform.forward, out var hit, Quaternion.identity, 1.0f, 1 << 7, QueryTriggerInteraction.Ignore))
        {
            //自分の視野に入っているか
            if(StelthUtility.IsHitInAngle(hit.transform, transform, 1.4f, 0.6f))
            {
                //相手の背後か
                if(StelthUtility.IsHitInAngle(transform, hit.transform, -0.6f, -1.4f))
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    public void TryRemoveNearEnemy(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<StelthEnemyComponent>(out var component))
        {
            if (_nearEnemies.Contains(component))
            {
                _nearEnemies.Remove(component);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<StelthEnemyComponent>(out var component))
        {
            if (!_nearEnemies.Contains(component))
            {
                _nearEnemies.Add(component);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        TryRemoveNearEnemy(other.gameObject);
    }
}
