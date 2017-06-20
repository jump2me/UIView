using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Xml;
using System.Linq;

public partial class UIView 
{
    public int ID { get; private set; }
    public string Name { get; internal set; }
    public GameObject GameObject { get; internal set; }
    public Type Type { get; internal set; }
    public bool IsRoot { get; internal set; }
    public UIView ClassRoot { get; internal set; }

	public UIView()
    {
        Type = GetType();
        Name = Type.Name;

        GameObject = new GameObject()
        {
            name = Name,
        };

        RectTransform = GameObject.AddComponent<RectTransform>();

        InitializeEventTrigger();

        ID = UIView.CreateID();
    }

    protected void SetLayoutXml()
    {
        string path = string.Format("UI/UIViews/{0}", Name);

        TextAsset textAsset = Resources.Load(path) as TextAsset;
        XmlDocument layoutXmlDocument = null;
        if (textAsset == null)
        {
            Debug.LogWarning("cannot find view xml: " + path);
        }
        else if (Utility.TryLoadXml(textAsset, out layoutXmlDocument))
        {
            UIView.ReadLayoutXmlDocument(layoutXmlDocument, this);
        }
    }

    public void ApplyLayoutXmlNode(XmlNode _xmlNode, UIView _view)
    {
        // GameObject Name
        string name = _xmlNode.GetStringValue("Name");
        if (string.IsNullOrEmpty(name) == false)
            _view.GameObject.name = name;

        // Canvas
        bool isRoot = _xmlNode.GetBoolValue("isRoot");
        _view.InitializeRendering(isRoot);

        // Find Style Data
        string styleId = "Default";
        if (_xmlNode.Attributes["Style"] != null)
            styleId = _xmlNode.Attributes["Style"].Value;

        XmlNode styleXml = StyleList.Find(e => e.Name == _view.Name && e.Attributes["Style"].Value == styleId);

        // width and height
        float width = _xmlNode.GetFloatValue("Width", styleXml);
        float height = _xmlNode.GetFloatValue("Height", styleXml);
       
        _view.RectTransform.sizeDelta = new Vector2(width, height);

        // anchor & offset
        string anchorValue = _xmlNode.GetStringValue("Anchor", styleXml);
        float offsetX = _xmlNode.GetFloatValue("OffsetX", styleXml);
        float offsetY = _xmlNode.GetFloatValue("OffsetY", styleXml);

        eAnchor anchor = eAnchor.MiddleCenter;
        if(string.IsNullOrEmpty(anchorValue) == false)
            anchor = (eAnchor)Enum.Parse(typeof(eAnchor), anchorValue);

        _view.RectTransform.SetAnchor(anchor, offsetX, offsetY);

        // margin
        string marginValue = _xmlNode.GetStringValue("Margin");
        if(string.IsNullOrEmpty(marginValue) == false)
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
		if(viewType == typeof(Text))
        {
            Text text = _view as Text;
            text.InitializeText(_xmlNode, styleXml);
        }
        else if(viewType == typeof(Button))
        {
            Button button = _view as Button;
            button.InitializeButton(_xmlNode, styleXml);
        }
    }
}