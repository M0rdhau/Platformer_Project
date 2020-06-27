using UnityEditor;
using UnityEngine;

public class EditorScript : EditorWindow
{
    int damage = 1;

    [MenuItem("Window/Player damager, etc.")]
    public static void ShowWindow()
    {
        GetWindow<EditorScript>("Custom editor script");
    }

    private void OnGUI()
    {
        GUILayout.Label("Damage player for " + damage + " point", EditorStyles.boldLabel);
        damage = EditorGUILayout.IntField("damage done to player", damage);

        if (GUILayout.Button("Damage"))
        {
            FindObjectOfType<PlayerHealth>().KnockBackHit(damage, Random.Range(0, 1) == 1);
        }
    }
}
