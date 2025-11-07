using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenList", menuName = "Scriptable Objects/GenList")]
public class GenList : ScriptableObject
{
    public List<GameObject> generators;
}
