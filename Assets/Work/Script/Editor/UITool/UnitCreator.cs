using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class UnitCreator : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    private Sprite m_Sprite;

    [MenuItem("Tool/UnitCreator")]
    public static void ShowExample()
    {
        UnitCreator wnd = GetWindow<UnitCreator>();
        wnd.titleContent = new GUIContent("UnitCreator");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        //_field.value
        //VisualElements objects can contain other VisualElement following a tree hierarchy.
        //VisualElement label = new Label("Hello World! From C#");
        //root.Add(label)
        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}
