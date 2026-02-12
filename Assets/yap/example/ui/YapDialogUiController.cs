using TMPro;
using UnityEngine;

public class YapDialogUiController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_dialogText;

	[SerializeField]
	private TextMeshProUGUI m_actorText;

	[SerializeField]
	private YapSceneData m_scene;

	private YapRunner m_runner = new YapRunner();

	private float m_advanceTimer;

	private void Start()
	{
		m_runner.StartYapping(m_scene);

		SetDialogText(m_runner.GetCurrentLine());
		SetActorText(m_runner.GetCurrentActor());
	}

	private void Update()
	{
		if (!m_runner.IsFinished())
		{
			m_advanceTimer += Time.deltaTime;
			if (m_advanceTimer > 3)
			{
				m_advanceTimer = 0;
				m_runner.Advance();
				if (!m_runner.IsFinished())
				{
					SetDialogText(m_runner.GetCurrentLine());
					SetActorText(m_runner.GetCurrentActor());
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	public void SetDialogText(string text)
	{
		m_dialogText.SetText(text);
	}
	public void SetActorText(string text)
	{
		m_actorText.SetText(text);
	}
}
