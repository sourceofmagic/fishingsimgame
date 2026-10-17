using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Рисуем стандартный инспектор (наши поля config, generateOnStart и т.д.)
        base.OnInspectorGUI();

        TerrainGenerator generator = (TerrainGenerator)target;

        EditorGUILayout.Space(10); // Отступ для красоты

        // Создаем кнопку
        if (GUILayout.Button("Сгенерировать ландшафт", GUILayout.Height(30)))
        {
            // Записываем состояние объекта для возможности отмены (Ctrl+Z)
            Undo.RecordObject(generator, "Generate Terrain");

            // Вызываем наш метод генерации
            generator.GenerateTerrain();

            // Помечаем сцену как измененную, чтобы Unity предложила её сохранить
            EditorUtility.SetDirty(generator);
        }
    }
}