using UnityEngine;
using System.ComponentModel;

public class TestView : UIView
{
    public Property<string> TestString { get; set; }
	public TestView()
	{
        TestString = new Property<string>();
        TestString.Value = "This is a test Property.";
	}

    public void OnRedButtonClicked()
    {
        Debug.Log("Red Button has been clicked.");
    }
}