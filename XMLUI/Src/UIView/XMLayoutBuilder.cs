using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;
using System.Xml;
using System.Collections.Generic;

public static class XMLayoutBuilder 
{
    public static void Build(UIView _view)
	{
		string path = string.Format("UI/XML/Views/{0}", _view.Name);

		TextAsset textAsset = Resources.Load(path) as TextAsset;
		XmlDocument layoutXmlDocument = null;
		if (textAsset == null)
		{
			Debug.LogWarning("cannot find view xml: " + path);
		}
		else if (XMLUtility.TryLoadXml(textAsset, out layoutXmlDocument))
		{
            ReadLayoutXmlDocument(layoutXmlDocument, _view);
		}
	}

	public static void ReadLayoutXmlDocument(XmlDocument _layoutXml, UIView _rootUIView)
	{
		XmlElement layoutXmlRoot = _layoutXml.DocumentElement;
		List<XmlNode> layoutXmlNodes = layoutXmlRoot.GetAllChildrenXMLNodesRecursively();

		Dictionary<XmlNode, UIView> viewByNodes = new Dictionary<XmlNode, UIView>();

		viewByNodes.Add(layoutXmlRoot, _rootUIView);

		_rootUIView.ClassRoot = _rootUIView;
		ApplyLayoutXmlNode(layoutXmlRoot, _rootUIView);

		foreach (XmlNode childLayoutXmlNode in layoutXmlNodes)
		{
			XmlNode parentNode = childLayoutXmlNode.ParentNode;
			UIView childUIView = XMLUI.CreateView(childLayoutXmlNode.Name);
			if (childUIView == null)
				return;

			childUIView.ClassRoot = _rootUIView;

			if (parentNode == layoutXmlRoot)
			{
				childUIView.Parent = _rootUIView.RectTransform;
			}
			else
			{
				childUIView.Parent = viewByNodes[parentNode].RectTransform;
			}

			viewByNodes.Add(childLayoutXmlNode, childUIView);

			ApplyLayoutXmlNode(childLayoutXmlNode, childUIView);
		}
	}

	public static void ApplyLayoutXmlNode(XmlNode _xmlNode, UIView _view)
	{
		// GameObject Name
		string name = _xmlNode.GetStringValue("Name");
		if (string.IsNullOrEmpty(name) == false)
			_view.GameObject.name = name;

		// Canvas
		bool isRoot = _xmlNode.GetBoolValue("Root");
		_view.InitializeCanvasRender(isRoot);

		bool ignoreInput = _xmlNode.GetBoolValue("IgnoreInput");
		if (ignoreInput == false)
			_view.InitializeInput();

		// Find Style Data
		string styleId = "Default";
		if (_xmlNode.Attributes["Style"] != null)
			styleId = _xmlNode.Attributes["Style"].Value;

		XmlNode styleXml = XMLUI.StyleList.Find(e => e.Name == _view.Name && e.Attributes["Style"].Value == styleId);

		// width and height
		float width = _xmlNode.GetFloatValue("Width", styleXml);
		float height = _xmlNode.GetFloatValue("Height", styleXml);

		_view.RectTransform.sizeDelta = new Vector2(width, height);

		// anchor & offset
		string anchorValue = _xmlNode.GetStringValue("Anchor", styleXml);
		float offsetX = _xmlNode.GetFloatValue("OffsetX", styleXml);
		float offsetY = _xmlNode.GetFloatValue("OffsetY", styleXml);

		eAnchor anchor = eAnchor.MiddleCenter;
		if (string.IsNullOrEmpty(anchorValue) == false)
			anchor = (eAnchor)Enum.Parse(typeof(eAnchor), anchorValue);

		_view.RectTransform.SetAnchor(anchor, offsetX, offsetY);

		// margin
		string marginValue = _xmlNode.GetStringValue("Margin");
		if (string.IsNullOrEmpty(marginValue) == false)
		{
			string[] margins = _xmlNode.Attributes["Margin"].Value.Split(',');
			float left = float.Parse(margins[0]);
			float right = float.Parse(margins[1]);
			float top = float.Parse(margins[2]);
			float bottom = float.Parse(margins[3]);

			Vector2 offsetMin = new Vector2(left, bottom);
			Vector2 offsetMax = new Vector2(-right, -top);

			_view.RectTransform.offsetMin = offsetMin;
			_view.RectTransform.offsetMax = offsetMax;
		}

		// Image
		string imgName = _xmlNode.GetStringValue("Image", styleXml);
		if (string.IsNullOrEmpty(imgName) == false)
		{
			var imgType = (UnityEngine.UI.Image.Type)Enum.Parse(typeof(UnityEngine.UI.Image.Type), _xmlNode.GetStringValue("Type", styleXml));
			_view.InitializeImageComponent(imgName, imgType);
		}

		Type viewType = _view.GetType();

		// Text
		if (viewType == typeof(Text))
		{
			Text text = _view as Text;
			text.InitializeText(_xmlNode, styleXml);
		}
		else if (viewType == typeof(Button))
		{
			Button button = _view as Button;
			button.InitializeButton(_xmlNode, styleXml);
		}
	}
}