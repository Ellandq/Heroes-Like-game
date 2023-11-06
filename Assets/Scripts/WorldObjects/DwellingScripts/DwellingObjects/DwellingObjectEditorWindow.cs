using UnityEditor;
using UnityEngine;

public class DwellingObjectEditorWindow : EditorWindow
{
    private DwellingObject dwellingObject;

    [MenuItem("Custom/Initialize Unit Cost")]
    public static void ShowWindow()
    {
        GetWindow<DwellingObjectEditorWindow>("Initialize Unit Cost");
    }

    private void OnGUI()
    {
        GUILayout.Label("Dwelling Object for Initialization", EditorStyles.boldLabel);
        dwellingObject = EditorGUILayout.ObjectField("Dwelling Object", dwellingObject, typeof(DwellingObject), false) as DwellingObject;

        if (dwellingObject != null)
        {
            if (GUILayout.Button("Initialize Unit Cost"))
            {
                dwellingObject.unitCost = new ResourceIncome();
                EditorUtility.SetDirty(dwellingObject);
            }
        }
    }
}
