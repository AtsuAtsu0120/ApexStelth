using Prashalt.Unity.Utility.Superclass;
using UnityEngine;

public class PlayerAudioManager : SingletonMonoBehaviour<PlayerAudioManager>
{
    public AudioSource activeAudioSource { get; private set; }

    [Header("Audio Clip")]
    [SerializeField] private AudioClip stoneFootStep;
    [SerializeField] private AudioClip woodFootStep;

    public void ChangeActivePlayer()
    {
        activeAudioSource = GameViewMaster.Instance.GetActivePlayerComponent().GetComponent<AudioSource>();
    }
    /// <summary>
    /// 地面が異なるタグになったときと、地面を歩いている状態になったときのみ呼ぶ
    /// </summary>
    public void StartFootStep(string tagName)
    {
        if (GetAudioSourcePlaying()) return;

        activeAudioSource.clip = tagName switch
        {
            "StoneGround" => stoneFootStep,
            "WoodGround" => woodFootStep,
            _ => null
        };

        if (activeAudioSource.clip is not null)
        {
            activeAudioSource.Play();
        }
    }
    public void StopFootStep()
    {
        activeAudioSource.Pause();
    }
    public bool GetAudioSourcePlaying()
    {
        return activeAudioSource is not null && activeAudioSource.isPlaying;
    }
}
