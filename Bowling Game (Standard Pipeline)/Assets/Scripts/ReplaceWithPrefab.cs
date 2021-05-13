using UnityEngine;
using UnityEditor;
using UnityEngine.ProGrids;

public class LevelEditorTools : EditorWindow
{
    //Module-level variables--------------------------------------------------
    string stringToEdit = "0.03125";

    //Fields to manipulate----------------------------------------------------

    //Replace with prefab
    [SerializeField] private GameObject prefab;


    [MenuItem("Tools/Level Editor Tools")]
    static void CreateLevelEditorTools()
    {
        EditorWindow.GetWindow<LevelEditorTools>();
    }

    private void OnGUI()
    {
        //holds prefab to replace
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("Replace"))
        {
            var selection = Selection.gameObjects;

            for (var i = selection.Length - 1; i >= 0; --i)
            {
                var selected = selection[i];
                var prefabType = PrefabUtility.GetPrefabType(prefab);
                GameObject newObject;

                if (prefabType == PrefabType.Prefab)
                {
                    newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                }
                else
                {
                    newObject = Instantiate(prefab);
                    newObject.name = prefab.name;
                }

                if (newObject == null)
                {
                    Debug.LogError("Error instantiating prefab");
                    break;
                }

                Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");
                newObject.transform.parent = selected.transform.parent;
                newObject.transform.localPosition = selected.transform.localPosition;
                newObject.transform.localRotation = selected.transform.localRotation;
                newObject.transform.localScale = selected.transform.localScale;
                newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
                Undo.DestroyObjectImmediate(selected);
            }
        }

        //pixel perfect size

        GUILayout.Label("Pixel Perfect Size");
        stringToEdit = GUILayout.TextField(stringToEdit, 25);

        GUI.enabled = false;
        EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);

    }
}