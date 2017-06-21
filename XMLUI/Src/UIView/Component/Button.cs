﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;

public class Button : UIView
{
    private MethodInfo MethodInfo { get; set; }

    private Sprite Up { get; set; }
    private Sprite Down { get; set; }
    private Sprite Disable { get; set; }

    public HashSet<Text> Labels { get; private set; }

    public Button()
    {
        Labels = new HashSet<Text>();
    }

    internal void InitializeButton(XmlNode _xmlNode, XmlNode _styleXml)
    {
		string methodName = _xmlNode.GetStringValue("OnClick");
        if (string.IsNullOrEmpty(methodName) == false)
            MethodInfo = ReflectionUtility.FindMethod(this, methodName);

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

        foreach(Text t in Labels)
        {
            t.Text.color = t.Up;
        }
    }

    public override void OnPointerDown(BaseEventData e)
    {
        base.OnPointerDown(e);

        Image.sprite = Down;

        foreach (Text t in Labels)
        {
            t.Text.color = t.Down;
        }
    }
}