using Assets.blackwhite_side_scroller.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow2D : MonoBehaviour
{
	public float LerpSpeedX = 5f;
	public float LerpSpeedY = 10f;
	public Transform m_Target;
	public float m_XOffset = 0;
	public float m_YOffset = 0;

	void Awake()
	{
		if (m_Target == null)
		{
			m_Target = GameObject.FindGameObjectWithTag(Constants.TagPlayer).transform;
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
				transform.position = new Vector3(
						Mathf.Lerp(transform.position.x, targetX, LerpSpeedX * Time.deltaTime),
						Mathf.Lerp(transform.position.y, targetY, LerpSpeedY * Time.deltaTime),
						transform.position.z);
			else
				transform.position = new Vector3(targetX, targetY, transform.position.z);
		}
	}
}
