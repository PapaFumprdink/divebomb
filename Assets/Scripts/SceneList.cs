using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Scene List")]
public sealed class SceneList : ScriptableObject
{
    public int MenuSceneIndex;
    public int GameSceneIndex;
    public int OptionsSceneIndex;
    public int ControlsSceneIndex;
}
