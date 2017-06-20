using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;

public static class XMLUtility
{
    public static List<XmlNode> GetAllChildrenXMLNodesRecursively(this XmlNode _xmlNode)
    {
        var xmlNodeList = new List<XmlNode>();
        foreach (XmlNode xmlNode in _xmlNode.ChildNodes)
        {
            xmlNodeList.Add(xmlNode);
            xmlNode.GetAllChildrenXMLNodesRecursively(xmlNodeList);
        }

        return xmlNodeList;
    }

    public static void GetAllChildrenXMLNodesRecursively(this XmlNode _xmlNode, List<XmlNode> _xmlNodeList)
    {
        foreach (XmlNode xmlNode in _xmlNode.ChildNodes)
        {
            _xmlNodeList.Add(xmlNode);
            xmlNode.GetAllChildrenXMLNodesRecursively(_xmlNodeList);
        }
    }

    private static XmlNode GetValidXmlNode(string _attributeName, XmlNode _current, XmlNode _style)
    {
        XmlNode node = null;
        if (_current.Attributes[_attributeName] != null)
            node = _current;
        else if (_style != null && _style.Attributes[_attributeName] != null)
            node = _style;

        return node;
    }

    public static bool GetBoolValue(this XmlNode _current, string _attributeName, XmlNode _style = null)
    {
        XmlNode node = GetValidXmlNode(_attributeName, _current, _style);

        if (node == null)
            return false;

        var value = false;
        if (bool.TryParse(node.Attributes[_attributeName].Value, out value))
        {
            return value;
        }
        else
            return false;
    }

    public static float GetFloatValue(this XmlNode _current, string _attributeName, XmlNode _style = null)
    {
        XmlNode node = GetValidXmlNode(_attributeName, _current, _style);

        if (node == null)
            return 0f;

        var value = 0f;
        if (float.TryParse(node.Attributes[_attributeName].Value, out value))
        {
            return value;
        }
        else
            return 0f;
    }

    public static int GetIntValue(this XmlNode _current, string _attributeName, XmlNode _style = null)
    {
        XmlNode node = GetValidXmlNode(_attributeName, _current, _style);

        if (node == null)
            return 0;

        var value = 0;
        if (int.TryParse(node.Attributes[_attributeName].Value, out value))
        {
            return value;
        }
        else
            return 0;
    }

    public static string GetStringValue(this XmlNode _current, string _attributeName, XmlNode _style = null)
    {
        XmlNode node = GetValidXmlNode(_attributeName, _current, _style);

        if (node == null)
            return string.Empty;
        else
            return node.Attributes[_attributeName].Value;
    }

    public static bool TryLoadXml(TextAsset _textAsset, out XmlDocument _xmlDoc)
    {
        _xmlDoc = new XmlDocument();

        try
        {
            _xmlDoc.LoadXml(_textAsset.text);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
