using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (MapGenerator))]
public class CustomMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator perlin = (MapGenerator)target;

        if (DrawDefaultInspector())
            perlin.GenerateMap();

        if (GUILayout.Button("Generate"))
        {
            perlin.offsetX = Random.Range(-999, 999);
            perlin.offsetY = Random.Range(-999, 999);
            perlin.GenerateMap();
        }
    }
}
