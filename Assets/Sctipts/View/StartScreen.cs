using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] List<Button> buttons;
    private bool isReady = false;
    public void OnStartSoloPlayButton()
    {
        StartLoadScene();
        isReady = true;
    }
    public void OnStartDuoPlayButton()
    {
        StartLoadScene();
        isReady = true;
    }
    private void StartLoadScene()
    {
        foreach(var button in buttons)
        {
            button.interactable = false;
        }
        var scene = SceneManager.LoadSceneAsync("1-Porison", LoadSceneMode.Additive);
        scene.allowSceneActivation = false;
        scene.completed += LoadScene;
    }
    private async void LoadScene(AsyncOperation operation)
    {
        await UniTask.WaitUntil(() => isReady);
        operation.allowSceneActivation = true;
    }
}
