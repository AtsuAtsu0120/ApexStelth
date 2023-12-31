using Prashalt.Unity.Utility.Superclass;
using UnityEngine;

public class PlayerAudioManager : SingletonMonoBehaviour<PlayerAudioManager>
{
    public AudioSource activeAudioSource { get; private set; }

    [Header("FootStep Clips")]
    [SerializeField] private AudioClip stoneFootStep;
    [SerializeField] private AudioClip woodFootStep;
    [SerializeField] private AudioClip sandyFootStep;
    [SerializeField] private AudioClip meadowFootStep;
    [Header("Other Clips")]
    [SerializeField] private AudioClip windNoise;
    [SerializeField] private AudioClip getItem;

    public void ChangeActivePlayer()
    {
        activeAudioSource = GameViewMaster.Instance.GetActivePlayerComponent().GetComponent<AudioSource>();
    }
    public void StartWindNoise()
    {
        if (GetAudioSourcePlaying()) return;
        activeAudioSource.clip = windNoise;
        activeAudioSource.Play();
    }
    public void StopWindNoise()
    {
        activeAudioSource.Stop();
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
            "SandyGround" => sandyFootStep,
            "MeadowGround" => meadowFootStep,
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
    public void PlayGetItem()
    {
        activeAudioSource.PlayOneShot(getItem);
    }
    public bool GetAudioSourcePlaying()
    {
        return activeAudioSource is not null && activeAudioSource.isPlaying;
    }
}
