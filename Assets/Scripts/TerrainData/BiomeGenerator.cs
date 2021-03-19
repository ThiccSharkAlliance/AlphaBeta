using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainData
{
    public class BiomeGenerator : MonoBehaviour
    {
        public static byte GenerateVoxelType(float x, float y, float z, float scale, Unity.Mathematics.Random numGenAltitude, Unity.Mathematics.Random numGenMoisture, float groundLevel)
        {
            float altitude = Noise.Generate2DNoiseValue(x, z, scale, numGenAltitude, groundLevel);
            float moisture = Noise.Generate2DNoiseValue(x, z, scale, numGenMoisture, 0);

            int blockType = 0;

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
                if (moisture > 25 && altitude <= 0.7 * biomeScale)
                {
                    // Water
                    blockType = 6;
                }
                else if (moisture <= 15 && altitude <= 0.35 * biomeScale && y > -20)
                {
                    // Air
                    blockType = 0;
                }
                else
                {
                    // Dirt
                    blockType = 2;
                }

                if (y > groundAltitude - 1)
                {
                    if (moisture > 25)
                    {
                        if (altitude > 1 * biomeScale)
                        {
                            // Snow
                            blockType = 5;
                        }
                        else if (altitude > 0.9 * biomeScale)
                        {
                            // Rock
                            blockType = 3;
                        }
                        else if (altitude > 0.8 * biomeScale)
                        {
                            // Grass
                            blockType = 1;
                        }
                        else if (altitude > 0.75 * biomeScale)
                        {
                            // Forest
                            blockType = 7;
                        }
                        else if (altitude > 0.7 * biomeScale)
                        {
                            // Beach
                            blockType = 8;
                        }
                        else
                        {
                            // Air
                            blockType = 0;
                        }
                    }
                    else if (moisture > 15)
                    {
                        if (altitude > 1 * biomeScale)
                        {
                            // Snow
                            blockType = 5;
                        }
                        else if (altitude > 0.8 * biomeScale)
                        {
                            // Rock
                            blockType = 3;
                        }
                        else if (altitude > 0.6 * biomeScale)
                        {
                            // Grass
                            blockType = 1;
                        }
                        else if (altitude > 0.5 * biomeScale)
                        {
                            // Forest
                            blockType = 7;
                        }
                        else
                        {
                            // Grass
                            blockType = 1;
                        }
                    }
                    else
                    {
                        if (altitude > 1.05 * biomeScale)
                        {
                            // Snow
                            blockType = 5;
                        }
                        if (altitude > 0.8 * biomeScale)
                        {
                            // Rock
                            blockType = 3;
                        }
                        else if (altitude > 0.6 * biomeScale)
                        {
                            // Plains (Dry Grass)
                            blockType = 9;
                        }
                        else if (altitude > 0.35 * biomeScale)
                        {
                            // Desert
                            blockType = 4;
                        }
                        else
                        {
                            // Air
                            blockType = 0;
                        }
                    }
                }
            }

            return (byte) blockType;
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
