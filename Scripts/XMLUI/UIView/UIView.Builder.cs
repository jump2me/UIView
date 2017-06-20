using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;
using System.Xml;
using System.Collections.Generic;

public partial class UIView
{
    private static int m_id;
    public static int CreateID()
    {
        return m_id++;
    }

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
        view.SetLayoutXml();

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
        view.SetLayoutXml();

        return view;
    }

    public static void ReadLayoutXmlDocument(XmlDocument _layoutXml, UIView _rootUIView)
    {
        XmlElement layoutXmlRoot = _layoutXml.DocumentElement;
        List<XmlNode> layoutXmlNodes = layoutXmlRoot.GetAllChildrenXMLNodesRecursively();

        Dictionary<XmlNode, UIView> viewByNodes = new Dictionary<XmlNode, UIView>();

        viewByNodes.Add(layoutXmlRoot, _rootUIView);

        _rootUIView.ClassRoot = _rootUIView;
        _rootUIView.ApplyLayoutXmlNode(layoutXmlRoot, _rootUIView);

        foreach(XmlNode childLayoutXmlNode in layoutXmlNodes)
        {
            XmlNode parentNode = childLayoutXmlNode.ParentNode;
            UIView childUIView = UIView.CreateView(childLayoutXmlNode.Name);
            if (childUIView == null)
                return;

            childUIView.ClassRoot = _rootUIView;

            if(parentNode == layoutXmlRoot)
            {
                childUIView.Parent = _rootUIView.RectTransform;
            }
            else
            {
                childUIView.Parent = viewByNodes[parentNode].RectTransform;
            }

            viewByNodes.Add(childLayoutXmlNode, childUIView);

            _rootUIView.ApplyLayoutXmlNode(childLayoutXmlNode, childUIView);
        }
    }
}