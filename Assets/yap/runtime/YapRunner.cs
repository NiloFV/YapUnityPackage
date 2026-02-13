using UnityEngine.Assertions;

public interface IYapRunnerContext { }

public class YapRunner
{
	private YapSceneData m_scene;
	private int m_currentNodeIndex;
	private string m_currentActor;

	public void StartYapping(YapSceneData scene, IYapRunnerContext context = null)
	{
		m_scene = scene;
		m_currentNodeIndex = 0;
		m_currentActor = "";
		ConsumeCommandNodes(context);
	}

	public void Advance(IYapRunnerContext context = null)
	{
		Assert.IsNotNull(m_scene);
		NodeData line = m_scene.Lines[m_currentNodeIndex];
		if (line.Transitions.Length == 0)
		{
			Stop();
		}
		m_currentNodeIndex = line.Transitions[0];
		ConsumeCommandNodes(context);
	}

	public string GetCurrentLine()
	{
		Assert.IsNotNull(m_scene);
		return m_scene.Lines[m_currentNodeIndex].Content;
	}
	public string GetCurrentActor()
	{
		return m_currentActor;
	}

	public void Stop()
	{
		Assert.IsNotNull(m_scene);
		m_currentNodeIndex = m_scene.Lines.Length + 1;
	}

	public bool IsFinished()
	{
		Assert.IsNotNull(m_scene);
		return m_currentNodeIndex >= m_scene.Lines.Length;
	}

	private void ConsumeCommandNodes(IYapRunnerContext context = null)
	{
		Assert.IsNotNull(m_scene);
		while (m_currentNodeIndex < m_scene.Lines.Length)
		{
			NodeData line = m_scene.Lines[m_currentNodeIndex];

			switch (line.LeafType)
			{
				case YapFileLeafType.Unkown:
				case YapFileLeafType.Line:
					return;
				case YapFileLeafType.SetActor:
					SetCurrentActor(line.Content, context);
					break;				
			}

			if (line.Transitions.Length == 0)
			{
				Stop();
				break;
			}
			m_currentNodeIndex = line.Transitions[0];
		}
	}

	private void SetCurrentActor(string actor, IYapRunnerContext context = null)
	{
		m_currentActor = actor;
	}
}
