using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class NPCObject : ScriptableObject
{
    /// <summary>
    /// xyzは座標。wは到着後の待ち時間
    /// </summary>
    [SerializeField] public List<Vector4> PassPositionList = new();
    [SerializeField] public bool shouldRandomMove;
    [SerializeField] public UnityEvent unityEvent;
}