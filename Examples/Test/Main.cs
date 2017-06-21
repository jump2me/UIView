using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Main : MonoBehaviour {

    public UnityEngine.UI.Button testButton;

    public TestView TestView { get; private set; }
    void Start () {
        
        XMLUI.Run();

        TestView = XMLUI.CreateView<TestView>();
        testButton.onClick.AddListener(OnTestButtonClicked);
    }

    void OnTestButtonClicked()
    {
        //TestView.TestString.Value = System.DateTime.Now.ToLongTimeString();
        XMLUI.Dump();
    }
}
