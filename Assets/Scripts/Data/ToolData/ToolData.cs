using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ToolData", menuName = "Tool Data", order = 51)]
public class ToolData : ScriptableObject
{
    public GameObject prefab;

    [Header("基本識別")]
    public string ToolName = "New Tool";
    // (可選) 如果你想根據類型做特殊判斷，可以加個枚舉
    //public Transform prefab;
    public Sprite sprite;


}
