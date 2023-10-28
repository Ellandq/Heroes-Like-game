using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DwellingObject))]
public class DwellingObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DwellingObject dwellingObject = (DwellingObject)target;

        // Check if the unitCost field is assigned
        if (dwellingObject.unitCost != null)
        {
            EditorGUILayout.LabelField("Unit Cost", EditorStyles.boldLabel);

            // Display and edit the values of the ResourceIncome component
            dwellingObject.unitCost.gold = EditorGUILayout.IntField("Gold", dwellingObject.unitCost.gold);
            dwellingObject.unitCost.wood = EditorGUILayout.IntField("Wood", dwellingObject.unitCost.wood);
            dwellingObject.unitCost.ore = EditorGUILayout.IntField("Ore", dwellingObject.unitCost.ore);
            dwellingObject.unitCost.gems = EditorGUILayout.IntField("Gems", dwellingObject.unitCost.gems);
            dwellingObject.unitCost.mercury = EditorGUILayout.IntField("Mercury", dwellingObject.unitCost.mercury);
            dwellingObject.unitCost.sulfur = EditorGUILayout.IntField("Sulfur", dwellingObject.unitCost.sulfur);
            dwellingObject.unitCost.crystal = EditorGUILayout.IntField("Crystal", dwellingObject.unitCost.crystal);
        }
    }
}
