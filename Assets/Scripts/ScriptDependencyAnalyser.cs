using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

public class ScriptDependencyAnalyser : MonoBehaviour
{
    public static ScriptDependencyAnalyser Instance;

    private Dictionary<System.Type, List<System.Type>> dependencies = new Dictionary<System.Type, List<System.Type>>();
    private Dictionary<System.Type, List<string>> accessedVariables = new Dictionary<System.Type, List<string>>();

    private void Awake ()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void AnalyseDependencies()
    {
        MonoBehaviour[] components = FindObjectsOfType<MonoBehaviour>();

        foreach (MonoBehaviour component in components)
        {
            System.Type type = component.GetType();
            List<System.Type> dependenciesList = new List<System.Type>();
            List<string> accessedVariablesList = new List<string>();

            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                System.Type fieldType = field.FieldType;
                if (fieldType.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    dependenciesList.Add(fieldType);
                }
                else
                {
                    accessedVariablesList.Add(field.Name);
                }
            }

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                System.Type propertyType = property.PropertyType;
                if (propertyType.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    dependenciesList.Add(propertyType);
                }
                else
                {
                    accessedVariablesList.Add(property.Name);
                }
            }

            dependencies[type] = dependenciesList;
            accessedVariables[type] = accessedVariablesList;
        }
    }

    public void SaveDependencies()
    {
        string path = Application.dataPath + "/dependencies.txt";
        using (StreamWriter writer = new StreamWriter(path))
        {
            foreach (System.Type type in dependencies.Keys)
            {
                writer.WriteLine(type.Name + ":");
                foreach (System.Type dependency in dependencies[type])
                {
                    writer.WriteLine("  " + dependency.Name);

                    if (dependencies.ContainsKey(dependency))
                    {
                        List<string> accessedVariablesList = accessedVariables[dependency];
                        foreach (string variable in accessedVariablesList)
                        {
                            writer.WriteLine("  " + variable + " - " + dependency.Name);
                        }
                    }
                }
                writer.WriteLine();
            }
        }
    }
}

