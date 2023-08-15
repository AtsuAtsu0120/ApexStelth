using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform cube;
    private void Update()
    {
        transform.LookAt(cube);
    }
}
