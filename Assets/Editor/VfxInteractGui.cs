using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using VoxelTerrain.Interactions;

namespace Editor
{
    [CustomEditor(typeof(VfxInteraction))]
    public class VfxInteractGui : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var vi = (VfxInteraction) target;

            EditorGUILayout.BeginHorizontal();
            //Button to select single shape type
            if (GUILayout.Button("Single"))
            {
                vi.Shape = FlattenShape.Single;
            }
            //Button to select square shape type
            if (GUILayout.Button("Square"))
            {
                vi.Shape = FlattenShape.Square;
            }

            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            //Button to select circle shape type
            if (GUILayout.Button("Circle"))
            {
                vi.Shape = FlattenShape.Circular;
            }
            //Button to select sphere shape type
            if (GUILayout.Button("Sphere"))
            {
                vi.Shape = FlattenShape.Sphere;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            //Set VFX field. Leave this outside as only one VFX is needed
            EditorGUILayout.LabelField("VFX", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f));
            vi.VFX = (VisualEffect) EditorGUILayout.ObjectField(vi.VFX, typeof(VisualEffect), true, GUILayout.MinWidth(100f), GUILayout.MaxWidth(150f));
            
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();

            switch (vi.Shape)
            {
                case FlattenShape.Default:
                    EditorGUILayout.HelpBox("Select a shape", MessageType.Warning);
                    break;
                case FlattenShape.Single:
                    break;
                case FlattenShape.Square:
                    #region Square
                    //New Line
                    EditorGUILayout.BeginHorizontal();
                    //Set spawn rate field label
                    EditorGUILayout.LabelField("SpawnRate", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    //spawnrate string text field, sets the variable
                    vi.SpawnRateStringId = EditorGUILayout.TextField(vi.SpawnRateStringId, GUILayout.Width(100f));
                    //spawn rate int field, sets the variable
                    vi.SpawnRate = EditorGUILayout.IntField(vi.SpawnRate, GUILayout.Width(60f));
                    
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    //Set spawn point ID
                    EditorGUILayout.LabelField("Spawn Point ID", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    vi.SpawnPointStringId = EditorGUILayout.TextField(vi.SpawnPointStringId, GUILayout.Width(100f));
                    
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    //Set particle life ID and value
                    EditorGUILayout.LabelField("Particle Life", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    vi.ParticleLifeStringId = EditorGUILayout.TextField(vi.ParticleLifeStringId, GUILayout.Width(100f));
                    vi.ParticleLife = EditorGUILayout.FloatField(vi.ParticleLife, GUILayout.Width(60f));
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.BeginHorizontal();
                    //Set box radius ID and value
                    EditorGUILayout.LabelField("Box Radius", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    vi.BoxRadiusStringId = EditorGUILayout.TextField(vi.BoxRadiusStringId, GUILayout.Width(100f));
                    vi.BoxRadius = EditorGUILayout.FloatField(vi.BoxRadius, GUILayout.Width(60f));
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;
                case FlattenShape.Circular:
                    break;
                case FlattenShape.Sphere:
                    #region Sphere
                    //New Line
                    EditorGUILayout.BeginHorizontal();
                    //Set spawn rate field label
                    EditorGUILayout.LabelField("SpawnRate", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    //spawnrate string text field, sets the variable
                    vi.SpawnRateStringId = EditorGUILayout.TextField(vi.SpawnRateStringId, GUILayout.Width(100f));
                    //spawn rate int field, sets the variable
                    vi.SpawnRate = EditorGUILayout.IntField(vi.SpawnRate, GUILayout.Width(60f));
                    
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    //Set spawn point ID
                    EditorGUILayout.LabelField("Spawn Point ID", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    vi.SpawnPointStringId = EditorGUILayout.TextField(vi.SpawnPointStringId, GUILayout.Width(100f));
                    
                    EditorGUILayout.EndHorizontal();
                    //Set particle life ID and value
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Particle Life", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    vi.ParticleLifeStringId = EditorGUILayout.TextField(vi.ParticleLifeStringId, GUILayout.Width(100f));
                    vi.ParticleLife = EditorGUILayout.FloatField(vi.ParticleLife, GUILayout.Width(60f));
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.BeginHorizontal();
                    //Set sphere radius ID and value
                    EditorGUILayout.LabelField("Sphere Radius", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    vi.SphereRadiusStringId = EditorGUILayout.TextField(vi.SphereRadiusStringId, GUILayout.Width(100f));
                    vi.SphereRadius = EditorGUILayout.FloatField(vi.SphereRadius, GUILayout.Width(60f));
                    EditorGUILayout.EndHorizontal();
                    #endregion
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //If changes were made, set object as dirty.
            if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(vi);

        }
    }
}
