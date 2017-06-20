using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIView
{
    public RectTransform RectTransform { get; internal set; }

	private RectTransform m_parent;
	public RectTransform Parent
	{
		get
		{
			return m_parent;
		}
		set
		{
			m_parent = value;

			RectTransform.SetParent(m_parent, false);
			RectTransform.localPosition = Vector3.zero;
			RectTransform.localScale = Vector3.one;
			RectTransform.localRotation = Quaternion.identity;
		}
	}
}
