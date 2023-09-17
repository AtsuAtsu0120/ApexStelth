using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisteryEventArea : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("‚­‚è‚ [I");
            GameLogicMaster.Instance.stageInfo.Missions.Find(x => x.Name == "“‡‚Ì“ä").OnComplete();
        }
    }
}
