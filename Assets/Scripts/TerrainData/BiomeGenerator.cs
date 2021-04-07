using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainData
{
    public class BiomeGenerator : MonoBehaviour
    {
        public static byte GenerateVoxelType(float x, float y, float z, float scale, SimplexNoise.OpenSimplex2S simplexAltitude, SimplexNoise.OpenSimplex2S simplexMoisture, float groundLevel)
        {
            float altitude = Noise.Generate2DNoiseValue(x, z, scale, simplexAltitude, groundLevel);
            float moisture = Noise.Generate2DNoiseValue(x * 0.05f, z * 0.05f, 1, simplexMoisture, 0);

            VoxelTerrain.Voxel.VoxelType voxelType = new VoxelTerrain.Voxel.VoxelType();

            float biomeScale = scale;

            // Set the value at the current coordinate and subtract ground level
            float groundAltitude = altitude - (groundLevel * scale);

            // Anything below ground level is moved up to 0 for flat land
            if (groundAltitude < 0)
            {
                groundAltitude = 0;
            }

            if (y <= groundAltitude)
            {
                // Ice
                if (moisture > 0.94f && altitude <= 0.6 * biomeScale)
                {
                    voxelType = VoxelTerrain.Voxel.VoxelType.Water;
                }
                // Grass Water
                else if (moisture > 0.78f && moisture <= 0.94f && altitude <= 0.6 * biomeScale)
                {
                    voxelType = VoxelTerrain.Voxel.VoxelType.Water;
                }
                // Swamp Water
                else if (moisture > 0.7f && moisture <= 0.78f && altitude <= 0.4 * biomeScale)
                {
                    voxelType = VoxelTerrain.Voxel.VoxelType.Water;
                }
                // Jungle Water
                else if (moisture > 0.62f && moisture <= 0.7f && altitude <= 0.5 * biomeScale)
                {
                    voxelType = VoxelTerrain.Voxel.VoxelType.Water;
                }
                // Desert Canyons
                else if (moisture <= 0.46f && altitude <= 0.4f * biomeScale && altitude > 0.2f * biomeScale && y > -20)
                {
                    voxelType = VoxelTerrain.Voxel.VoxelType.Default;
                }
                // Dirt
                else
                {
                    voxelType = VoxelTerrain.Voxel.VoxelType.Dirt;
                }

                if (y > groundAltitude - 1)
                {
                    // Snow
                    if (moisture > 0.94f)
                    {
                        if (altitude > 1 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Snow;
                        }
                        else if (altitude > 0.8 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Stone;
                        }
                        else if (altitude > 0.75 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Snow;
                        }
                        else if (altitude > 0.7 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.PineForest;
                        }
                        else if (altitude > 0.6 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Snow;
                        }
                        else
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Ice;
                        }
                    }
                    // Grass
                    else if (moisture > 0.78f)
                    {
                        if (altitude > 1 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Snow;
                        }
                        else if (altitude > 0.8 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Stone;
                        }
                        else if (altitude > 0.75 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Grass;
                        }
                        else if (altitude > 0.7 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Forest;
                        }
                        else if (altitude > 0.65 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Grass;
                        }
                        else if (altitude > 0.6 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Beach;
                        }
                        else
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Default;
                        }
                    }
                    // Swamp
                    else if (moisture > 0.7f)
                    {
                        if (altitude > 1 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Snow;
                        }
                        else if (altitude > 0.8 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Stone;
                        }
                        else if (altitude > 0.7 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Grass;
                        }
                        else if (altitude > 0.6 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Mud;
                        }
                        else if (altitude > 0.5 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.SwampForest;
                        }
                        else if (altitude > 0.4 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Mud;
                        }
                        else
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Default;
                        }
                    }
                    // Jungle
                    else if (moisture > 0.62f)
                    {
                        if (altitude > 1.05f * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Snow;
                        }
                        else if (altitude > 0.9 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Stone;
                        }
                        else if (altitude > 0.6 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.JungleForest;
                        }
                        else if (altitude > 0.5 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Beach;
                        }
                        else
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Default;
                        }
                    }
                    // Savannah
                    else if (moisture > 0.46f)
                    {
                        if (altitude > 1.1f * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Snow;
                        }
                        else if (altitude > 0.9f * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Stone;
                        }
                        else if (altitude > 0.7f * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Plains;
                        }
                        else if (altitude > 0.5f * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.SavannahForest;
                        }
                        else
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.SavannahGrass;
                        }
                    }
                    // Desert
                    else
                    {
                        if (altitude > 0.8 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Sandstone;
                        }
                        else if (altitude > 0.4 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Sand;
                        }
                        else if (altitude > 0.2 * biomeScale)
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Default;
                        }
                        else
                        {
                            voxelType = VoxelTerrain.Voxel.VoxelType.Sand;
                        }
                    }
                }
            }

            return (byte) voxelType;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
