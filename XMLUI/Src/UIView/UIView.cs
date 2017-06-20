using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Xml;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public partial class UIView
{
    public int ID { get; private set; }
    public string Name { get; internal set; }
    public GameObject GameObject { get; internal set; }
    public Type Type { get; internal set; }
    public bool IsRoot { get; internal set; }
    public UIView ClassRoot { get; internal set; }

	public Canvas Canvas { get; internal set; }
	public CanvasScaler CanvasScaler { get; internal set; }
	public GraphicRaycaster GraphicRaycaster { get; internal set; }
	public CanvasGroup CanvasGroup { get; internal set; }

	public RectTransform RectTransform { get; internal set; }

	private RectTransform m_parent;
	public RectTransform Parent
	{
		get
		{
			return m_parent;
		}
		set
		{
			m_parent = value;

			RectTransform.SetParent(m_parent, false);
			RectTransform.localPosition = Vector3.zero;
			RectTransform.localScale = Vector3.one;
			RectTransform.localRotation = Quaternion.identity;
		}
	}

	public UnityEngine.UI.Image Image { get; internal set; }
	public UnityEngine.UI.Text Text { get; internal set; }
    public EventTrigger EventTrigger { get; private set; }

	public UIView()
    {
        Type = GetType();
        Name = Type.Name;

        GameObject = new GameObject()
        {
            name = Name,
        };

        RectTransform = GameObject.AddComponent<RectTransform>();
    }

    public void SetLayoutXml()
    {
        string path = string.Format("UI/XML/Views/{0}", Name);

        TextAsset textAsset = Resources.Load(path) as TextAsset;
        XmlDocument layoutXmlDocument = null;
        if (textAsset == null)
        {
            Debug.LogWarning("cannot find view xml: " + path);
        }
        else if (XMLUtility.TryLoadXml(textAsset, out layoutXmlDocument))
        {
            XMLUI.ReadLayoutXmlDocument(layoutXmlDocument, this);
        }
    }

    public void ApplyLayoutXmlNode(XmlNode _xmlNode, UIView _view)
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
            InitializeInput();

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

    protected void InitializeCanvasRender(bool _root)
	{
		IsRoot = _root;
		if (IsRoot)
		{
			Canvas = GameObject.GetComponent<Canvas>();
			if (Canvas == null)
			{
				Canvas = GameObject.AddComponent<Canvas>();
			}

			Canvas.renderMode = XMLUI.RenderMode;

			CanvasScaler = GameObject.GetComponent<CanvasScaler>();
			if (CanvasScaler == null)
			{
				CanvasScaler = GameObject.AddComponent<CanvasScaler>();
			}

			CanvasScaler.uiScaleMode = XMLUI.ScaleMode;
			CanvasScaler.referenceResolution = XMLUI.ReferenceResolution;

			GraphicRaycaster = GameObject.GetComponent<GraphicRaycaster>();
			if (GraphicRaycaster == null)
			{
				GraphicRaycaster = GameObject.AddComponent<GraphicRaycaster>();
			}
		}

		CanvasGroup = GameObject.GetComponent<CanvasGroup>();
		if (CanvasGroup == null)
		{
			CanvasGroup = GameObject.AddComponent<CanvasGroup>();
		}
	}

	private bool m_interactable = true;
	public bool Interactable
	{
		get
		{
			return m_interactable;
		}
		set
		{
			m_interactable = value;

			UpdateInteractable();
		}
	}

	private bool m_visible = true;
	public bool Visible
	{
		get
		{
			return m_visible;
		}
		set
		{
			m_visible = value;

			UpdateVisible();
		}
	}

	void UpdateVisible()
	{
		if (Canvas != null)
		{
			Canvas.enabled = m_visible;
		}
		else
		{
			GameObject.SetActive(m_visible);
		}
	}

	void UpdateInteractable()
	{

	}

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

	public void InitializeInput()
	{
        EventTrigger = GameObject.GetComponent<EventTrigger>();
		if (EventTrigger == null)
		{
			EventTrigger = GameObject.AddComponent<EventTrigger>();
		}

		EventTrigger.triggers = new List<EventTrigger.Entry>();
		var triggers = System.Enum.GetValues(typeof(EventTriggerType));
		foreach (EventTriggerType t in triggers)
		{
			var entry = new EventTrigger.Entry();
			entry.eventID = t;
			switch (t)
			{
				case EventTriggerType.PointerClick:
					entry.callback.AddListener(OnPointerClicked);
					break;
				case EventTriggerType.PointerUp:
					entry.callback.AddListener(OnPointerUp);
					break;
				case EventTriggerType.PointerDown:
					entry.callback.AddListener(OnPointerDown);
					break;
				default:
					continue;
			}

			EventTrigger.triggers.Add(entry);
		}
	}

	public virtual void OnPointerClicked(BaseEventData e)
	{

	}

	public virtual void OnPointerUp(BaseEventData e)
	{

	}

	public virtual void OnPointerDown(BaseEventData e)
	{

	}
}