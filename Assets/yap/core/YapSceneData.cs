using System;
using UnityEngine;

public enum YapFileLeafType : int
{
	Unkown = 0,
	Line = 1,
	SetActor = 2,
}

[Serializable]
public struct NodeData
{
	public string Content;
	public YapFileLeafType LeafType;
	public int[] Transitions;
}

public class YapSceneData : ScriptableObject
{
	public string SceneName;
	public NodeData[] Lines;
}
