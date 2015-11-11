using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ComboBox))]
public class ComboBoxEditor : Editor {

    ComboBox cb;
    List<string> options;
    bool ellapsed;
    GUILayoutOption prefixLabelOption;
    void OnEnable()
    {
        cb = (ComboBox)target;
        options = new List<string>();
        ellapsed = false;
        prefixLabelOption = GUILayout.Width(40f);

        if(cb.options != null)
        {
            options = cb.options.ConvertAll(obj => obj.ToString());
        }
    }

    public override void OnInspectorGUI()
    {
        bool change = false;
        serializedObject.Update();

        EditorGUILayout.LabelField("Default Text: ");
        string defaultText = EditorGUILayout.TextField(cb.defaultText);

        if(defaultText != cb.defaultText)
        {
            cb.defaultText = defaultText;
            cb.ReloadDefaultText();
        }

        ellapsed = EditorGUILayout.Foldout(ellapsed, "Options");

        if (ellapsed)
        {
            EditorGUI.indentLevel += 1;
            string newOption;

            for (int i = 1; i < options.Count+1; ++i )
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i + ": ", prefixLabelOption);
                newOption = EditorGUILayout.TextField(options[i-1]);

                if (newOption != options[i-1])
                {
                    options[i-1] = newOption;
                    change = true;
                }

                if(options[i-1] == "")
                {
                    options.RemoveAt(i-1);
                    GUI.FocusControl("");
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField((options.Count + 1) + ": ", prefixLabelOption);
            newOption = EditorGUILayout.TextField("");
            EditorGUILayout.EndHorizontal();

            if(newOption != "")
            {
                options.Add(newOption);
                change = true;
            }

            EditorGUI.indentLevel -= 1;

            if(change)
            {
                cb.options = options;//options.ConvertAll((obj) => (object)obj);

                //cb.ReloadOptions();
            }

            
        }
        if (cb.options != null)
        {
            //EditorGUILayout.LabelField(cb._ellapsed.ToString());
        }
    }

    [MenuItem("GameObject/UI/Combo Box")]
    public static void CreateComboBox()
    {
        ComboBox.Create();
    }

    [MenuItem("GameObject/UI/Make Visible Combo Box")]
    public static void visibleComboBox()
    {
        new List<ComboBox>(GameObject.FindObjectsOfType<ComboBox>()).ForEach(o => o.gameObject.hideFlags = HideFlags.None);
    }
}

