using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using VoxelTerrain.Interactions;
using VoxelTerrain.Voxel;

namespace Editor
{
    [CustomEditor(typeof(ScriptableVfxInteract))]
    public class VfxInteractGui : UnityEditor.Editor
    {
        private VoxelType _voxType = VoxelType.Default;

        private bool _showShapes;
        private bool _showVoxels;
      
        public override void OnInspectorGUI()
        {
            var vi = (ScriptableVfxInteract) target;

            if (vi.VFXInteraction == null) vi.VFXInteraction = new VfxInteraction();

            if (vi.VFXInteraction.Vfx == null || vi.VFXInteraction.Vfx.Length != 18)
            {
                vi.VFXInteraction.Vfx = new VisualEffect[18];
            }

            if (vi.VFXInteraction.SecondWaveColour == null || vi.VFXInteraction.SecondWaveColour.Length != 18)
            {
                vi.VFXInteraction.SecondWaveColour = new Color[18];
            }

            EditorGUI.BeginChangeCheck();

            #region Shape
            _showShapes = EditorGUILayout.Foldout(_showShapes, "Shapes");

            if (_showShapes)
            {

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Select Shape Type", GUILayout.MinWidth(100f), GUILayout.MaxWidth(130f));
                EditorGUILayout.LabelField("Selected Shape: " + vi.VFXInteraction.Shape, GUILayout.MinWidth(120f), GUILayout.MaxWidth(200f));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                //Button to select single shape type
                if (GUILayout.Button("Single"))
                {
                    vi.VFXInteraction.Shape = FlattenShape.Single;
                }

                //Button to select square shape type
                if (GUILayout.Button("Square"))
                {
                    vi.VFXInteraction.Shape = FlattenShape.Square;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                //Button to select circle shape type
                if (GUILayout.Button("Circle"))
                {
                    vi.VFXInteraction.Shape = FlattenShape.Circular;
                }

                //Button to select sphere shape type
                if (GUILayout.Button("Sphere"))
                {
                    vi.VFXInteraction.Shape = FlattenShape.Sphere;
                }

                EditorGUILayout.EndHorizontal();
            }
            #endregion

            #region VoxelType

            _showVoxels = EditorGUILayout.Foldout(_showVoxels, "Voxels");

            if (_showVoxels)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Select Voxel Type", GUILayout.MinWidth(100f), GUILayout.MaxWidth(130f));
                EditorGUILayout.LabelField("Selected Voxel: " + _voxType, GUILayout.MinWidth(120f), GUILayout.MaxWidth(200f));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Destroy", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Default;
                }

                if (GUILayout.Button("Grass", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Grass;
                }

                if (GUILayout.Button("Dirt", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Dirt;
                }

                if (GUILayout.Button("Stone", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Stone;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Sand", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Sand;
                }

                if (GUILayout.Button("Snow", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Snow;
                }

                if (GUILayout.Button("Water", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Water;
                }

                if (GUILayout.Button("Forest", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Forest;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Beach", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Beach;
                }

                if (GUILayout.Button("Plains", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Plains;
                }

                if (GUILayout.Button("Mud", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Mud;
                }

                if (GUILayout.Button("Ice", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f)))
                {
                    _voxType = VoxelType.Ice;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Savannah Grass", GUILayout.MinWidth(60f), GUILayout.MaxWidth(120f)))
                {
                    _voxType = VoxelType.SavannahGrass;
                }

                if (GUILayout.Button("Savannah Forest", GUILayout.MinWidth(60f), GUILayout.MaxWidth(120f)))
                {
                    _voxType = VoxelType.SavannahForest;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Jungle Forest", GUILayout.MinWidth(60f), GUILayout.MaxWidth(120f)))
                {
                    _voxType = VoxelType.JungleForest;
                }

                if (GUILayout.Button("Pine Forest", GUILayout.MinWidth(60f), GUILayout.MaxWidth(120f)))
                {
                    _voxType = VoxelType.PineForest;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Swamp Forest", GUILayout.MinWidth(60f), GUILayout.MaxWidth(120f)))
                {
                    _voxType = VoxelType.SwampForest;
                }

                if (GUILayout.Button("Sandstone", GUILayout.MinWidth(60f), GUILayout.MaxWidth(120f)))
                {
                    _voxType = VoxelType.Sandstone;
                }

                EditorGUILayout.EndHorizontal();

            }
            #endregion

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Scan for Vfx"))
            {
                vi.VFXInteraction.ScanForVfx = !vi.VFXInteraction.ScanForVfx;
            }

            EditorGUILayout.LabelField(vi.VFXInteraction.ScanForVfx ? "Scanning" : "Idle", GUILayout.MinWidth(100f),
                GUILayout.MaxWidth(130f));
            
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            //Set VFX field. Leave this outside as only one VFX is needed
            EditorGUILayout.LabelField("VFX", GUILayout.MinWidth(30f), GUILayout.MaxWidth(60f));
            vi.VFXInteraction.Vfx[(int) _voxType] = (VisualEffect) EditorGUILayout.ObjectField(vi.VFXInteraction.Vfx[(int) _voxType], typeof(VisualEffect), true,
                GUILayout.MinWidth(100f), GUILayout.MaxWidth(250f));
                    
            EditorGUILayout.EndHorizontal();

            #region EssentialShapeFields

            //New Line
            EditorGUILayout.BeginHorizontal();
            //Set spawn rate field label
            EditorGUILayout.LabelField("SpawnRate", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
            //spawnrate string text field, sets the variable
            vi.VFXInteraction.SpawnRateStringId = EditorGUILayout.TextField(vi.VFXInteraction.SpawnRateStringId, GUILayout.Width(100f));
            //spawn rate int field, sets the variable
            vi.VFXInteraction.SpawnRate = EditorGUILayout.IntField(vi.VFXInteraction.SpawnRate, GUILayout.Width(60f));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            //Set spawn point ID
            EditorGUILayout.LabelField("Spawn Point ID", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
            vi.VFXInteraction.SpawnPointStringId = EditorGUILayout.TextField(vi.VFXInteraction.SpawnPointStringId, GUILayout.Width(100f));

            EditorGUILayout.EndHorizontal();
            //Set particle life ID and value
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Particle Life", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
            vi.VFXInteraction.ParticleLifeStringId =
                EditorGUILayout.TextField(vi.VFXInteraction.ParticleLifeStringId, GUILayout.Width(100f));
            vi.VFXInteraction.ParticleLife = EditorGUILayout.FloatField(vi.VFXInteraction.ParticleLife, GUILayout.Width(60f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            //Set the Spark Spawn Rate ID and Value
            EditorGUILayout.LabelField("Spark Spawn", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
            vi.VFXInteraction.SparkSpawnRateStringId =
                EditorGUILayout.TextField(vi.VFXInteraction.SparkSpawnRateStringId, GUILayout.Width(100f));
            vi.VFXInteraction.SparkSpawnRate = EditorGUILayout.IntField(vi.VFXInteraction.SparkSpawnRate, GUILayout.Width(60f));
            EditorGUILayout.EndHorizontal();

            #endregion

            switch (vi.VFXInteraction.Shape)
            {
                case FlattenShape.Default:
                    EditorGUILayout.HelpBox("Select a shape", MessageType.Warning);
                    break;
                case FlattenShape.Single:
                    #region Single 

                    #endregion
                    break;
                case FlattenShape.Square:
                    #region Square

                    EditorGUILayout.BeginHorizontal();
                    //Set box radius ID and value
                    EditorGUILayout.LabelField("Box Radius", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    vi.VFXInteraction.BoxRadiusXStringId = EditorGUILayout.TextField(vi.VFXInteraction.BoxRadiusXStringId, GUILayout.Width(100f));
                    vi.VFXInteraction.BoxRadiusX = EditorGUILayout.FloatField(vi.VFXInteraction.BoxRadiusX, GUILayout.Width(60f));
                    EditorGUILayout.EndHorizontal();
                    
                    #endregion
                    break;
                case FlattenShape.Circular:
                    #region circular

                    #endregion
                    break;
                case FlattenShape.Sphere:

                    #region Sphere

                    EditorGUILayout.BeginHorizontal();
                    //Set sphere radius ID and value
                    EditorGUILayout.LabelField("Sphere Radius", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    vi.VFXInteraction.SphereRadiusStringId =
                        EditorGUILayout.TextField(vi.VFXInteraction.SphereRadiusStringId, GUILayout.Width(100f));
                    vi.VFXInteraction.SphereRadius = EditorGUILayout.FloatField(vi.VFXInteraction.SphereRadius, GUILayout.Width(60f));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    //Set the Particle Ring count ID and Value
                    EditorGUILayout.LabelField("Paricle Ring Count", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    vi.VFXInteraction.RingSpawnCountStringId =
                        EditorGUILayout.TextField(vi.VFXInteraction.RingSpawnCountStringId, GUILayout.Width(100f));
                    vi.VFXInteraction.RingSpawnCount = EditorGUILayout.FloatField(vi.VFXInteraction.RingSpawnCount, GUILayout.Width(60f));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    //Set the Particle Ring count ID and Value
                    EditorGUILayout.LabelField("Second Wave Colour", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    vi.VFXInteraction.SecondWaveColourStringId =
                        EditorGUILayout.TextField(vi.VFXInteraction.SecondWaveColourStringId, GUILayout.Width(100f));
                    vi.VFXInteraction.SecondWaveColour[(int)_voxType] = EditorGUILayout.ColorField(vi.VFXInteraction.SecondWaveColour[(int)_voxType], GUILayout.Width(60f));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    //Set the smoke spawn ID and Value
                    EditorGUILayout.LabelField("Smoke Spawn", GUILayout.MinWidth(60f), GUILayout.MaxWidth(100f));
                    vi.VFXInteraction.SmokeSpawnStringId =
                        EditorGUILayout.TextField(vi.VFXInteraction.SmokeSpawnStringId, GUILayout.Width(100f));
                    vi.VFXInteraction.SmokeSpawn = EditorGUILayout.IntField(vi.VFXInteraction.SmokeSpawn, GUILayout.Width(60f));
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
