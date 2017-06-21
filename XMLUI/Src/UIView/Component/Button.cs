using UnityEngine;
using UnityEngine.EventSystems;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;

public class Button : UIView
{
    private MethodInfo MethodInfo { get; set; }

    private Sprite UpSprite { get; set; }
    private Sprite DownSprite { get; set; }

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

        UpSprite = Image.sprite;

        var downImgName = _xmlNode.GetStringValue("Down", _styleXml);
        if (string.IsNullOrEmpty(downImgName) == false)
        {
            DownSprite = Resources.Load<Sprite>(downImgName);
        }
        else
        {
            DownSprite = Image.sprite;
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

        Image.sprite = UpSprite;

        foreach(Text t in Labels)
        {
            t.Text.color = t.UpColor;
        }
    }

    public override void OnPointerDown(BaseEventData e)
    {
        base.OnPointerDown(e);

        Image.sprite = DownSprite;

        foreach (Text t in Labels)
        {
            t.Text.color = t.DownColor;
        }
    }
}