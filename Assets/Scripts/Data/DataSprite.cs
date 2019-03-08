using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSprite", menuName = "Scriptable Objects/Sprite")]
public class DataSprite : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
}