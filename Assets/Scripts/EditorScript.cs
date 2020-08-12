using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorScript : EditorWindow
{
    int damage = 1;
    float charge = 0.1f;
    float gameTimeScale = 0f;

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

        GUILayout.Label("Add charge to player: " + charge);
        charge = EditorGUILayout.FloatField("charge to add", charge);

        if (GUILayout.Button("Charge up"))
        {
            FindObjectOfType<CombatCharge>().AddCharge(charge);
        }


        GUILayout.Label("Change time scale");
        gameTimeScale = EditorGUILayout.Slider(gameTimeScale, 1, 20);

        if (GUILayout.Button("Reset save"))
        {
            File.Delete(Path.Combine(Application.persistentDataPath, "save" + ".sav"));
        }
    }

    private void OnInspectorUpdate()
    {
        Time.timeScale = gameTimeScale;
    }
}
