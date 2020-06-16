using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow2D : MonoBehaviour
{
	public float LerpSpeed = 10f;
	public Transform m_Target;
	public float m_XOffset = 0;
	public float m_YOffset = 0;

	private float margin = 0.1f;

	void Awake()
	{
		if (m_Target == null)
		{
			m_Target = GameObject.FindGameObjectWithTag("Player").transform;
		}
		Move(false);
	}

	void Update()
	{
		Move(true);
	}

	void Move(bool lerp)
	{
		if (m_Target)
		{
			float targetX = m_Target.position.x;
			targetX += (m_Target.GetComponent<CharacterController2D>().FacingRight ? m_XOffset : -m_XOffset);

			float targetY = m_Target.position.y + m_YOffset;

			//if (Mathf.Abs(transform.position.x - targetX) > margin)
			//	targetX = Mathf.Lerp(transform.position.x, targetX, 1 / m_DampTime * Time.deltaTime);

			//if (Mathf.Abs(transform.position.y - targetY) > margin)
			//	targetY = Mathf.Lerp(transform.position.y, targetY, m_DampTime * Time.deltaTime);

			if (lerp)
				transform.position =
					Vector3.Slerp(transform.position, new Vector3(targetX, targetY, transform.position.z), LerpSpeed * Time.deltaTime);
			else
				transform.position = new Vector3(targetX, targetY, transform.position.z);
		}
	}
}
