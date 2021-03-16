using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainData
{
    public class BiomeGenerator : MonoBehaviour
    {
        public static int GenerateVoxelType(float x, float y, float z, float scale, Unity.Mathematics.Random numGen)
        {
            float altitude = Noise.Generate2DNoiseValue(x, z, scale, numGen, 0);
            float moisture = Noise.Generate2DNoiseValue(x, z, scale, numGen, 0);

            int blockType = 0;

            if (y <= altitude)
            {
                // Dirt

                if (y > altitude - 1)
                {
                    if (moisture > 0.7 * scale)
                    {
                        if (altitude > 0.9 * scale)
                        {
                            // Snow
                        }
                        else if (altitude > 0.75 * scale)
                        {
                            // Rock
                        }
                        else if (altitude > 0.5 * scale)
                        {
                            // Grass
                        }
                        else if (altitude > 0.3 * scale)
                        {
                            // Forest
                        }
                        else if (altitude > 0.25 * scale)
                        {
                            // Sand
                        }
                        else
                        {
                            // Water
                        }
                    }
                    else if (moisture > 0.4 * scale)
                    {
                        if (altitude > 0.9 * scale)
                        {
                            // Snow
                        }
                        else if (altitude > 0.75 * scale)
                        {
                            // Rock
                        }
                        else if (altitude > 0.25 * scale)
                        {
                            // Grass
                        }
                    }
                    else
                    {
                        if (altitude > 0.75 * scale)
                        {
                            // Rock
                        }
                        else if (altitude > 0.5 * scale)
                        {
                            // Plains (Dry Grass)
                        }
                        else if (altitude > 0.25 * scale)
                        {
                            // Desert
                        }
                    }
                }
            }

            return blockType;
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
