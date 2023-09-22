using Cysharp.Threading.Tasks;
using Prashalt.Csharp.Utility.Superclass;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class GameLogicMaster : PureSingleton<GameLogicMaster>
{
    public bool IsPlayer1Active { get; set; } = true;

    #region Property

    public float ConfirmedPercent { get; private set; } = 0.0f;
    public bool IsConfirming { get; private set; } = false;

    public bool Is2PlayersMode { get; set; } = false;
    public GameObject[] PlayerObjects { get; private set; } = new GameObject[2];

    public StelthInputActions Actions { get; set; }

    public Stage stageInfo = new Stage1(GameViewMaster.Instance.UpdateMissonView);

    #endregion
    #region Private_Field

    private CancellationTokenSource resetConfirmedPercentCancellationToken;

    #endregion
    /// <summary>
    /// 敵に見つかったとき
    /// </summary>
    /// <param name="deltaTime"></param>
    public void StartConfirmePlayerByEnemyJob(float deltaTime)
    {
        resetConfirmedPercentCancellationToken?.Cancel();
        if (ConfirmedPercent >= 5) 
        {
            GameOver();
            return;
        }

        var result = new NativeArray<float>(1, Allocator.TempJob);
        var hander = new ConfirmePlayerByEnemy()
        {
            confirmedPercent = ConfirmedPercent,
            deltaTime = deltaTime,
            result = result
        }.Schedule();
        JobHandle.ScheduleBatchedJobs();

        hander.Complete();

        ConfirmedPercent = result[0];
        result.Dispose();
    }
    /// <summary>
    /// C# JobSystemを利用して徐々にConfirmedPercentを減少させ、0まで行く。
    /// </summary>
    public async void StartResetConfirmedPercent()
    {
        resetConfirmedPercentCancellationToken = new();
        await ResetConfirmedPercent(resetConfirmedPercentCancellationToken.Token);
    }
    public async UniTask ResetConfirmedPercent(CancellationToken token)
    {
        //少し遅延させる
        await UniTask.Delay(1500);
        //遅延中にキャンセルされているなら終わり
        if (token.IsCancellationRequested) return;

        //ConfirmedPercentが0になるまで減らすJobを飛ばす
        for (;ConfirmedPercent >= 0;)
        {
            var result = new NativeArray<float>(1, Allocator.TempJob);
            var hander = new DecreaseConfirmePercent()
            {
                confirmedPercent = ConfirmedPercent,
                result = result
            }.Schedule();
            JobHandle.ScheduleBatchedJobs();

            hander.Complete();

            ConfirmedPercent = result[0];
            result.Dispose();

            await UniTask.Delay(10);
            if (token.IsCancellationRequested) return;
        }
        ConfirmedPercent = 0;
    }
    public void GameOver()
    {
        Debug.Log("ゲームオーバー");
    }
    #region Jobs
    [BurstCompile]
    public struct ConfirmePlayerByEnemy : IJob
    {
        public float confirmedPercent;
        public float deltaTime;

        public NativeArray<float> result;
        public void Execute()
        {
            result[0] = confirmedPercent + deltaTime;
        }
    }
    [BurstCompile]
    public struct DecreaseConfirmePercent : IJob
    {
        public float confirmedPercent;

        public NativeArray<float> result;

        public void Execute()
        {
            result[0] = confirmedPercent - 0.05f;
        }
    }
    #endregion
}
