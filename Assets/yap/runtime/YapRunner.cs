using UnityEngine.Assertions;

public interface IYapRunnerContext { }

public class YapRunner
{
	private YapSceneData m_scene;
	private int m_currentNodeIndex;

	public void Populate(YapSceneData scene)
	{
		m_scene = scene;
		m_currentNodeIndex = 0;
	}

	public void Advance(IYapRunnerContext context = null)
	{
		Assert.IsNotNull(m_scene);
		LineData line = m_scene.Lines[m_currentNodeIndex];
		if (line.Transitions.Length == 0)
		{
			Stop();
		}
		m_currentNodeIndex = line.Transitions[0];
	}

	public string GetCurrentLine()
	{
		Assert.IsNotNull(m_scene);
		return m_scene.Lines[m_currentNodeIndex].Content;
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
}
