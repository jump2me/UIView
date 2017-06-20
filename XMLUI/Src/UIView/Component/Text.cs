using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using System.Xml;
using System.Text.RegularExpressions;
using System.Linq;

public class Text : UIView
{
    private string DefaultText { get; set; }
    private Property<string> BoundProperty { get; set; }
    public Text()
    {
        
    }

    internal void InitializeText(XmlNode _xmlNode, XmlNode _style)
    {
        string fontName = _xmlNode.GetStringValue("Font", _style);
		Font font = XMLUI.FindFont(fontName);
		int fontSize = _xmlNode.GetIntValue("Size", _style);
        string hex = _xmlNode.GetStringValue("Color", _style);
        float spacing = _xmlNode.GetFloatValue("Spacing", _style);
        TextAnchor alignment = (TextAnchor)Enum.Parse(typeof(TextAnchor), _xmlNode.GetStringValue("Alignment", _style));
		bool autoWrap = _xmlNode.GetBoolValue("AutoWrap", _style);
		bool bestFit = _xmlNode.GetBoolValue("BestFit", _style);
		int minSize = _xmlNode.GetIntValue("MinSize", _style);
		int maxSize = _xmlNode.GetIntValue("MaxSize", _style);
        string text = _xmlNode.GetStringValue("Text", _style);

        Property<string> property = null;
        string propertyName = string.Empty;
        var result = Regex.Matches(text, @"{[^{}\d]*}")
                          .Cast<Match>()
                          .Select(p => p.Value)
                          .ToList();

        if(result.Count > 0)
        {
            propertyName = result[0].Substring(1, result[0].Length - 2);
            property = (Property<string>)ReflectionUtility.FindProperty(this, propertyName);
        }

        this.InitializeTextComponent(font,
									FontStyle.Normal,
									fontSize,
									spacing,
									true,
									alignment,
									autoWrap ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow,
									VerticalWrapMode.Truncate,
									bestFit,
									minSize,
									maxSize,
                                     ColorUtility.HexToColor(hex),
									text,
		                            property);
    }

    internal void InitializeTextComponent(Font _font, FontStyle _fontStyle, int _fontSize, float _lineSpacing, bool _richText, TextAnchor _alignment,
		HorizontalWrapMode _horizontalOverflow, VerticalWrapMode _verticalOverflow, bool _bestFit, int _minSize, int _maxSize, Color32 _color,
                                 string _text, Property<string> _boundProperty = null)
	{
		Text = GameObject.GetComponent<UnityEngine.UI.Text>();
		if (Text == null)
			Text = GameObject.AddComponent<UnityEngine.UI.Text>();

		Text.font = _font;
		Text.fontStyle = _fontStyle;
		Text.fontSize = _fontSize;
		Text.lineSpacing = _lineSpacing;
		Text.supportRichText = true;
		Text.alignment = _alignment;
		Text.horizontalOverflow = _horizontalOverflow;
		Text.verticalOverflow = _verticalOverflow;
		Text.resizeTextForBestFit = _bestFit;
		Text.resizeTextMinSize = _minSize;
		Text.resizeTextMaxSize = _maxSize;
		Text.color = _color;
		
		Text.raycastTarget = false;

        BoundProperty = _boundProperty;
        if(BoundProperty != null)
        {
            BoundProperty.PropertyChangedEvent += OnPropertyChanged;
        }

        DefaultText = _text;

        UpdateTextComponent();
	}

    private void OnPropertyChanged(object _sender, string _value)
    {
        UpdateTextComponent();
    }

    private void UpdateTextComponent()
    {
        if (BoundProperty != null)
            Text.text = BoundProperty.Value;
        else
            Text.text = DefaultText;
    }
}