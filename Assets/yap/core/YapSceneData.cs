using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LineData
{
	public string Content;
	public int[] Transitions;
}

public class YapSceneData : ScriptableObject
{
	public string SceneName;
	public LineData[] Lines;
}
