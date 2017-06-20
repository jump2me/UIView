using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using System.Xml;
using System.Reflection;

public class Button : UIView
{
    private MethodInfo MethodInfo { get; set; }

    private Sprite Up { get; set; }
    private Sprite Down { get; set; }
    private Sprite Disable { get; set; }

    public Button()
    {
        
    }

    internal void InitializeButton(XmlNode _xmlNode, XmlNode _styleXml)
    {
		string methodName = _xmlNode.GetStringValue("OnClick");
        if (string.IsNullOrEmpty(methodName) == false)
		    MethodInfo = FindMethod(methodName);

        Up = Image.sprite;
        Disable = Image.sprite;

        var downImgName = _xmlNode.GetStringValue("Down", _styleXml);
        if (string.IsNullOrEmpty(downImgName) == false)
        {
            Down = Resources.Load<Sprite>(downImgName);
        }
        else
        {
            Down = Image.sprite;
        }
    }

    public override void OnPointerClicked(BaseEventData e)
    {
        base.OnPointerClicked(e);

        if (MethodInfo == null)
            return;

        MethodInfo.Invoke(ClassRoot, null);
    }

    public override void OnPointerUp(BaseEventData e)
    {
        base.OnPointerUp(e);

        Image.sprite = Up;
    }

    public override void OnPointerDown(BaseEventData e)
    {
        base.OnPointerDown(e);

        Image.sprite = Down;
    }
}