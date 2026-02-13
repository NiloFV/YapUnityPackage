using System;
using UnityEngine;

public enum YapFileLeafType : int
{
	Unkown = 0,
	Line = 1,
	SetActor = 2,
	Marker = 3,
	Command = 4,
}

public enum CommandType: int
{
	None = 0,
	Jump = 1,
}

[Serializable]
public struct NodeData
{
	public string Content;
	public YapFileLeafType LeafType;
	public CommandType Command;
	public int[] Transitions;
}

public class YapSceneData : ScriptableObject
{
	public string SceneName;
	public NodeData[] Lines;
}
