using TMPro;
using UnityEngine;

public class YapDialogUiController : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_dialogText;

	[SerializeField]
	private YapSceneData m_scene;

	private YapRunner m_runner = new YapRunner();

	private float m_advanceTimer;

	private void Start()
	{
		m_runner.Populate(m_scene);

		SetDialogText(m_runner.GetCurrentLine());
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
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	public void SetDialogText(string Text)
	{
		m_dialogText.SetText(Text);
	}
}
