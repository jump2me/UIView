using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;
using System.Xml;
using System.Collections.Generic;

public static class XMLUI
{
#if UNITY_EDITOR
    public static HashSet<UIView> UIViews = new HashSet<UIView>();
#endif

    public static Assembly Assembly { get; private set; }

    private static RenderMode m_renderMode = UnityEngine.RenderMode.ScreenSpaceOverlay;
    public static RenderMode RenderMode
    {
        get
        {
            return m_renderMode;
        }
        set
        {
            m_renderMode = value;
        }
    }

    private static CanvasScaler.ScaleMode m_uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    public static CanvasScaler.ScaleMode ScaleMode
    {
        get
        {
            return m_uiScaleMode;
        }
        set
        {
            m_uiScaleMode = value;
        }
    }

    private static Vector2 m_referenceResolution = new Vector2(1280f, 720f);
    public static Vector2 ReferenceResolution
    {
        get
        {
            return m_referenceResolution;
        }
        set
        {
            m_referenceResolution = value;
        }
    }

    public static void Run()
    {
        Assembly = System.Reflection.Assembly.GetExecutingAssembly();

        InitializeStyle();
    }

    public static TYPE CreateView<TYPE>() where TYPE : UIView
    {
        TYPE view = Activator.CreateInstance<TYPE>();
        XMLayoutBuilder.Build(view);

        return view;
    }

    public static UIView CreateView(string _id)
    {
        string typeName = _id;
        Type type = Assembly.GetType(typeName);

        if (type == null)
        {
            Debug.LogWarning("cannot find class: " + typeName);
            return null;
        }

        UIView view = (UIView)Activator.CreateInstance(type);
        XMLayoutBuilder.Build(view, false);

        return view;
    }

    private static string m_style = "Default";
    public static string Style
    {
        get
        {
            return m_style;
        }
        set
        {
            m_style = value;

            InitializeStyle();
        }
    }

    public static Dictionary<string, Font> FontsByName { get; private set; }

    public static List<XmlNode> StyleList = new List<XmlNode>();
    private static void InitializeStyle()
    {
        string path = "UI/XML/Styles/" + Style;
        TextAsset textAsset = Resources.Load(path) as TextAsset;
        XmlDocument xmlDocument = null;
        if (textAsset == null)
        {
            Debug.LogWarning("cannot find style xml: " + path);
        }
        else if (XMLUtility.TryLoadXml(textAsset, out xmlDocument))
        {
            StyleList = xmlDocument.GetAllChildrenXMLNodesRecursively();

            LoadFonts();
        }
    }

    private static void LoadFonts()
    {
        FontsByName = new Dictionary<string, Font>();
        FontsByName.Add("Arial", Resources.GetBuiltinResource<Font>("Arial.ttf"));

        var nodes = StyleList.FindAll(e => e.Name.Equals("Font"));
        foreach (var node in nodes)
        {
            var fontName = node.GetStringValue("Name");
            var fontPath = node.GetStringValue("Asset");
            var font = Resources.Load<Font>(fontPath);
            FontsByName.Add(fontName, font);
        }
    }

    public static Font FindFont(string _fontName)
    {
        Font font;
        if (FontsByName.TryGetValue(_fontName, out font))
        {
            return font;
        }
        else
        {
            Debug.LogWarning(string.Format("cannot find font {0}. using fallback font instead.", _fontName));
            return FontsByName["Arial"];
        }
    }

#if UNITY_EDITOR
    public static void Dump()
    {
        foreach(UIView view in UIViews)
        {
            Debug.Log(view);
        }
    }
#endif
}
