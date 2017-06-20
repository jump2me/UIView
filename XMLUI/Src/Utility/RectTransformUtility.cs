using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;

public enum eAnchor
{
    TopLeft,
    TopCenter,
    TopRight,

    MiddleLeft,
    MiddleCenter,
    MiddleRight,

    BottomLeft,
    BottomCenter,
    BottomRight,

    StretchLeft,
    StretchCenter,
    StretchRight,

    StretchTop,
    StretchMiddle,
    StretchBottom,

    Stretch
}

public static class RectTransformUtility
{
    public static void SetAnchor(this RectTransform _rectTransform, eAnchor _anchor, float _offsetX = 0f, float _offsetY = 0f)
    {
        switch (_anchor)
        {
            case (eAnchor.TopLeft):
                {
                    _rectTransform.anchorMin = new Vector2(0, 1);
                    _rectTransform.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (eAnchor.TopCenter):
                {
                    _rectTransform.anchorMin = new Vector2(0.5f, 1);
                    _rectTransform.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (eAnchor.TopRight):
                {
                    _rectTransform.anchorMin = new Vector2(1, 1);
                    _rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (eAnchor.MiddleLeft):
                {
                    _rectTransform.anchorMin = new Vector2(0, 0.5f);
                    _rectTransform.anchorMax = new Vector2(0, 0.5f);
                    break;
                }
            case (eAnchor.MiddleCenter):
                {
                    _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                }
            case (eAnchor.MiddleRight):
                {
                    _rectTransform.anchorMin = new Vector2(1, 0.5f);
                    _rectTransform.anchorMax = new Vector2(1, 0.5f);
                    break;
                }

            case (eAnchor.BottomLeft):
                {
                    _rectTransform.anchorMin = new Vector2(0, 0);
                    _rectTransform.anchorMax = new Vector2(0, 0);
                    break;
                }
            case (eAnchor.BottomCenter):
                {
                    _rectTransform.anchorMin = new Vector2(0.5f, 0);
                    _rectTransform.anchorMax = new Vector2(0.5f, 0);
                    break;
                }
            case (eAnchor.BottomRight):
                {
                    _rectTransform.anchorMin = new Vector2(1, 0);
                    _rectTransform.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (eAnchor.StretchTop):
                {
                    _rectTransform.anchorMin = new Vector2(0, 1);
                    _rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                }
            case (eAnchor.StretchMiddle):
                {
                    _rectTransform.anchorMin = new Vector2(0, 0.5f);
                    _rectTransform.anchorMax = new Vector2(1, 0.5f);
                    break;
                }
            case (eAnchor.StretchBottom):
                {
                    _rectTransform.anchorMin = new Vector2(0, 0);
                    _rectTransform.anchorMax = new Vector2(1, 0);
                    break;
                }

            case (eAnchor.StretchLeft):
                {
                    _rectTransform.anchorMin = new Vector2(0, 0);
                    _rectTransform.anchorMax = new Vector2(0, 1);
                    break;
                }
            case (eAnchor.StretchCenter):
                {
                    _rectTransform.anchorMin = new Vector2(0.5f, 0);
                    _rectTransform.anchorMax = new Vector2(0.5f, 1);
                    break;
                }
            case (eAnchor.StretchRight):
                {
                    _rectTransform.anchorMin = new Vector2(1, 0);
                    _rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                }

            case (eAnchor.Stretch):
                {
                    _rectTransform.anchorMin = new Vector2(0, 0);
                    _rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                }
        }

        var dx = 1;
        switch (_anchor)
        {
            case eAnchor.BottomRight:
            case eAnchor.MiddleRight:
            case eAnchor.StretchRight:
            case eAnchor.TopRight:
                dx = -1;
                break;
        }
        var dy = 1;
        switch(_anchor)
        {
            case eAnchor.StretchTop:
            case eAnchor.TopCenter:
            case eAnchor.TopLeft:
            case eAnchor.TopRight:
                dy = -1;
                break;
        }

        _rectTransform.anchoredPosition = new Vector3(_offsetX * dx, _offsetY * dy, 0);
    }
}