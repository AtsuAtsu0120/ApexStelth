using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisteryEventArea : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("���肠�[�I");
            GameLogicMaster.Instance.stageInfo.Missions.Find(x => x.Name == "���̓�").OnComplete();
        }
    }
}
