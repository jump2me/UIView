using UnityEngine;
using System.ComponentModel;

public class TestView : UIView
{
    public Property<string> TestString { get; set; }
	public TestView()
	{
        TestString = new Property<string>();
        TestString.Value = "Click Me!";
	}

    public void OnButtonClicked()
    {
        TestString.Value = "Clicked!";
    }
}