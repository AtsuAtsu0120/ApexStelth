using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionable
{
    void OnActionKey();
    (bool, string) EnableAction();
}
