using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] List<Button> buttons;
    public void OnStartSoloPlayButton()
    {
        StartLoadScene();
    }

    private void StartLoadScene()
    {
        foreach(var button in buttons)
        {
            button.interactable = false;
        }
        var scene = SceneManager.LoadSceneAsync("1-Porison", LoadSceneMode.Additive);
    }
}
