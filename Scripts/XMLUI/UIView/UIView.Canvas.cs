using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class UIView
{
	public Canvas Canvas { get; internal set; }
	public CanvasScaler CanvasScaler { get; internal set; }
	public GraphicRaycaster GraphicRaycaster { get; internal set; }
	public CanvasGroup CanvasGroup { get; internal set; }

    protected void InitializeRendering(bool _root)
    {
        IsRoot = _root;
        if (IsRoot)
        {
            Canvas = GameObject.GetComponent<Canvas>();
            if (Canvas == null)
            {
                Canvas = GameObject.AddComponent<Canvas>();
            }

            Canvas.renderMode = RenderMode;

            CanvasScaler = GameObject.GetComponent<CanvasScaler>();
            if (CanvasScaler == null)
            {
                CanvasScaler = GameObject.AddComponent<CanvasScaler>();
            }

            CanvasScaler.uiScaleMode = ScaleMode;
            CanvasScaler.referenceResolution = ReferenceResolution;

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
}
