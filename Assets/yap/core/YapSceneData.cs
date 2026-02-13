using System;
using UnityEngine;



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
	public NodeData[] Nodes;
}
