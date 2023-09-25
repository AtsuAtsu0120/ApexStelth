using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Movie : MonoBehaviour
{
    public List<CinemachineSplineDolly> cameraList;

    private int index = 0;
    private void Start()
    {
        OnChangeCamera();
    }
    private async void OnChangeCamera()
    {
        //‚½‚Ô‚ñ‚±‚±UniRxŽg‚Á‚½‚Ù‚¤‚ªãY—í‚É‚È‚é
        await UniTask.WaitUntil(() => cameraList[index].CameraPosition >= 1);
        cameraList[index].gameObject.SetActive(false);

        index++;

        if (cameraList.Count > index)
        {
            cameraList[index].gameObject.SetActive(true);
            OnChangeCamera();
        }
    }
}
