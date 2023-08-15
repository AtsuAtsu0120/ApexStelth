using UnityEngine;

public class Overlooking : ViewpointState
{
    private GameObject fpsCam;
    public Overlooking(CharacterStateManager stateManager) : base(stateManager) { }

    public override void OnEnter()
    {
        fpsCam = stateManager.camTransform.gameObject;
        fpsCam.SetActive(false);
    }

    public override void OnExit()
    {
        fpsCam.SetActive(true);
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnUpdate()
    {
        //視点は固定なのでなし。
    }
}