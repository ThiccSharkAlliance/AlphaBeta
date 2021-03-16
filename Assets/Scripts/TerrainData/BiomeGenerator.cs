using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainData
{
    public class BiomeGenerator : MonoBehaviour
    {
        public static byte GenerateVoxelType(float x, float y, float z, float scale, Unity.Mathematics.Random numGen)
        {
            float altitude = Noise.Generate2DNoiseValue(x, z, scale, numGen, 0);
            float moisture = Noise.Generate2DNoiseValue(x, z, scale, numGen, 0);

            int blockType = 0;

            if (y <= altitude)
            {
                // Dirt
                blockType = 2;

                if (y > altitude - 1)
                {
                    if (moisture > 0.7 * scale)
                    {
                        if (altitude > 0.9 * scale)
                        {
                            // Snow
                            blockType = 5;
                        }
                        else if (altitude > 0.75 * scale)
                        {
                            // Rock
                            blockType = 3;
                        }
                        else if (altitude > 0.5 * scale)
                        {
                            // Grass
                            blockType = 1;
                        }
                        else if (altitude > 0.3 * scale)
                        {
                            // Forest
                            blockType = 7;
                        }
                        else if (altitude > 0.25 * scale)
                        {
                            // Beach
                            blockType = 8;
                        }
                        else
                        {
                            // Water
                            blockType = 6;
                        }
                    }
                    else if (moisture > 0.4 * scale)
                    {
                        if (altitude > 0.9 * scale)
                        {
                            // Snow
                            blockType = 5;
                        }
                        else if (altitude > 0.75 * scale)
                        {
                            // Rock
                            blockType = 3;
                        }
                        else if (altitude > 0.25 * scale)
                        {
                            // Grass
                            blockType = 1;
                        }
                    }
                    else
                    {
                        if (altitude > 0.75 * scale)
                        {
                            // Rock
                            blockType = 3;
                        }
                        else if (altitude > 0.5 * scale)
                        {
                            // Plains (Dry Grass)
                            blockType = 9;
                        }
                        else if (altitude > 0.25 * scale)
                        {
                            // Desert
                            blockType = 4;
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
