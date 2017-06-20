using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public partial class UIView
{
	public UnityEngine.UI.Image Image { get; internal set; }

	protected void InitializeImageComponent(string _imgName, UnityEngine.UI.Image.Type _imgType)
	{
		if (string.IsNullOrEmpty(_imgName))
			return;

		Image = GameObject.GetComponent<UnityEngine.UI.Image>();
		if (Image == null)
			Image = GameObject.AddComponent<UnityEngine.UI.Image>();

		Sprite sprite = Resources.Load<Sprite>(_imgName);
		Image.type = _imgType;
		Image.sprite = sprite;
	}
}