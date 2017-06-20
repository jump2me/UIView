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

    public void InitializeCanvasRender(bool _root)
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

    public void InitializeImageComponent(string _imgName, UnityEngine.UI.Image.Type _imgType)
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