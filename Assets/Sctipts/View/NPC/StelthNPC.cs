using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class StelthNPC : MonoBehaviour
{
    [SerializeField] private NPCObject npc;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    private bool isReseted = false;

    private int nowPositionIndex;
    public float VigilanceLevel { get; set; } = 0;
    protected bool isVigilance = false;
    private float waitTime;

    private Slider slider;

    public virtual void Start()
    {
        slider = GetComponentInChildren<Slider>();

        if (npc is null)
        {
            return;
        }
        agent.updatePosition = false;
        RandomList();
    }
    public bool ShotConfirmeRaycast()
    {
        slider?.gameObject.SetActive(true);
        //敵がプレイヤーを見つける用の処理
        if (isVigilance && Physics.BoxCast(transform.position + Vector3.up * 0.5f, new Vector3(50f, 0.2f, 0.2f), transform.forward, out var hit, Quaternion.identity, 10.0f, 1 << 6, QueryTriggerInteraction.Ignore))
        {
            if (StelthUtility.IsHitInAngle(hit.transform, transform, 1.4f, 0.6f))
            {
                VigilanceLevel += Time.deltaTime;
                if(VigilanceLevel > 3)
                {
                    GameLogicMaster.Instance.StartConfirmePlayerByEnemyJob(Time.deltaTime);
                    isReseted = true;
                }
            }
            slider.value = VigilanceLevel;
            return true;
        }
        else
        {
            if(VigilanceLevel > 0)
            {
                VigilanceLevel -= Time.deltaTime;
            }
            else
            {
                VigilanceLevel = 0;
                slider?.gameObject.SetActive(false);
            }

            if (isReseted)
            {
                isReseted = false;
                GameLogicMaster.Instance.StartResetConfirmedPercent();
            }
            slider.value = VigilanceLevel;
            return false;
        }
    }
    public void FixedUpdate()
    {
        if(npc is null)
        {
            return;
        }
        // 足滑りなしのアニメーション
        // nextPositionからdeltaPositionを算出
        var worldDeltaPosition = agent.nextPosition - transform.position;

        // キャラクターを基点にしたxz平面に射影したdeltaPosition
        var dx = Vector3.Dot(transform.right, worldDeltaPosition);
        var dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Time.deltaTimeから速度を算出
        var velocity = deltaPosition / Time.deltaTime;

        animator.SetFloat("Speed", velocity.y);
        transform.position = agent.nextPosition;

        var hasIndexNext = nowPositionIndex < npc.PassPositionList.Count - 1;
        bool canGoing;

        if (hasIndexNext)
        {
            canGoing = npc.PassPositionList[nowPositionIndex + 1].w <= waitTime;
        }
        else
        {
            canGoing = npc.PassPositionList[0].w <= waitTime;
        }

        var isGoal = agent.remainingDistance <= 0.1f;
        if (isGoal && canGoing)
        {
            //待ち時間のリセット
            waitTime = 0;

            agent.destination = npc.PassPositionList[nowPositionIndex];
            if(hasIndexNext)
            {
                nowPositionIndex++;
            }
            else
            {
                RandomList();
                nowPositionIndex = 0;
            }
        }
        else if(isGoal)
        {
            waitTime += Time.fixedDeltaTime;
        }
    }
    private void RandomList()
    {
        if (npc.shouldRandomMove)
        {
            npc.PassPositionList = npc.PassPositionList.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<StelthAudio>(out var stelthAudio) && stelthAudio.audioSource.isPlaying)
        {
            var audioSource = stelthAudio.audioSource;
            var animationCurve = audioSource.rolloffMode switch
            {
                AudioRolloffMode.Linear => AnimationCurve.Linear(audioSource.minDistance, 1, audioSource.maxDistance, 0),
                AudioRolloffMode.Custom => audioSource.GetCustomCurve(AudioSourceCurveType.CustomRolloff),
                _ => null
            };
            if (animationCurve is null) return;

            var distance = Vector3.Distance(transform.position, stelthAudio.transform.position);
            var volume = animationCurve.Evaluate(distance / audioSource.maxDistance);

            Debug.Log(volume * 0.05f * stelthAudio.GetSuspiciousLevel());
            VigilanceLevel += volume * 0.05f * stelthAudio.GetSuspiciousLevel();
        }
    }
}
