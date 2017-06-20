using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public partial class UIView
{
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
        string path = "UI/UIStyles/" + Style;
        TextAsset textAsset = Resources.Load(path) as TextAsset;
        XmlDocument xmlDocument = null;
        if (textAsset == null)
        {
            Debug.LogWarning("cannot find style xml: " + path);
        }
        else if (Utility.TryLoadXml(textAsset, out xmlDocument))
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
        foreach(var node in nodes)
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
        if(FontsByName.TryGetValue(_fontName, out font))
        {
            return font;
        }
        else
        {
            Debug.LogWarning(string.Format("cannot find font {0}. using fallback font instead.", _fontName));
            return FontsByName["Arial"];
        }
    }
}
