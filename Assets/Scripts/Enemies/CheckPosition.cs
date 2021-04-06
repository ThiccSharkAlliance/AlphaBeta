using System.Collections;
using System.Collections.Generic;
using TerrainData;
using UnityEngine;
using VoxelTerrain.Voxel;
using VoxelTerrain.Voxel.Jobs;

public class CheckPosition : MonoBehaviour
{
    [SerializeField] private VoxelEngine _engine;
    Transform player;
    public GameObject myPrefab;


    float PosX;
    float PosZ;
    float times = 0;
    bool continueSpawn = true;
    [SerializeField] public int distanceFromPlayer = 4;
    [SerializeField] float maxDistance = 400;

    public List<GameObject> listOfObjects;

    // Start is called before the first frame update
    void Start()
    {
    }

    private float getPosition(float PosX, float PosZ, float ground, float scale, float groundScale)
    {
            float altitude = Noise.Generate2DNoiseValue(PosX, PosZ, scale, _engine.WorldInfo.NumGenAltitude, ground);
            float position = altitude - groundScale;
            return position;
    }


    private bool validatePosition(float PosX, float PosZ, float ground, float scale, float groundScale)
    {

        Unity.Mathematics.Random RNG = _engine.WorldInfo.NumGenAltitude;

        float altitude = Noise.Generate2DNoiseValue(PosX, PosZ, scale, RNG, ground);
        float groundAltitude = altitude - groundScale;
        float altitude2 = Noise.Generate2DNoiseValue(PosX + 0.5f, PosZ, scale, RNG, ground);
        float groundAltitude2 = altitude - groundScale;
        float altitude3 = Noise.Generate2DNoiseValue(PosX - 0.5f, PosZ, scale, RNG, ground);
        float groundAltitude3 = altitude3 - groundScale;
        float altitude4 = Noise.Generate2DNoiseValue(PosX, PosZ + 0.5f, scale, RNG, ground);
        float groundAltitude4 = altitude4 - groundScale;
        float altitude5 = Noise.Generate2DNoiseValue(PosX, PosZ - 0.5f, scale, RNG, ground);
        float groundAltitude5 = altitude5 - groundScale;


        if (groundAltitude2 > (groundAltitude + 0.1f) || groundAltitude2 < (groundAltitude - 0.1f))
        {
            return false;
        }

        if (groundAltitude3 > (groundAltitude + 0.1f) || groundAltitude3 < (groundAltitude - 0.1f))
        {
            return false;
        }

        if (groundAltitude4 > (groundAltitude + 0.1f) || groundAltitude4 < (groundAltitude - 0.1f))
        {
            return false;
        }

        if (groundAltitude5 > (groundAltitude + 0.1f) || groundAltitude5 < (groundAltitude - 0.1f))
        {
            return false;
        }

        float position = getPosition(PosX, PosZ, ground, scale, groundScale);
        var transform = new Vector3(PosX, position + 1, PosZ);

        if (listOfObjects.Count > 0)
        {
            foreach (GameObject item in listOfObjects)
            {
                Debug.LogWarning(Vector3.Distance(transform, item.transform.position));
                if (Vector3.Distance(transform, item.transform.position) < 30)
                {
                    return false;
                }

            }
        }

       // Debug.LogError(groundAltitude2);
       // Debug.LogError(groundAltitude3);
       // Debug.LogError(groundAltitude4);
       // Debug.LogError(groundAltitude5);

        return true;

    }

    // Update is called once per frame
    void Update()
    {

        CheckActivation();


        if (times > 2)
        {
            continueSpawn = false;
        }
       

        if(GameObject.FindWithTag("Player") == null)
        {

        }else
        {
            player = GameObject.FindWithTag("Player").transform;

            if (continueSpawn)
            {
                float addOrSubtract = UnityEngine.Random.value < 0.5f ? 1 : -1;
                float randomSignX = UnityEngine.Random.value < 0.5f ? 1 : -1;
                float randomSignZ = UnityEngine.Random.value < 0.5f ? 1 : -1;

                float PosX = player.position.x - (Random.Range(50, 70) * randomSignX);
                float PosZ = player.position.z - (Random.Range(50, 70) * randomSignZ);

                //Debug.LogWarning(PosX);
                //Debug.LogWarning(PosZ);

                float ground = _engine.WorldInfo.GroundLevel;
                float scale = _engine.NoiseScale;

                float groundScale = (ground * scale);

                float position = 0;

                if (validatePosition(PosX, PosZ, ground, scale, groundScale))
                {
                    position = getPosition(PosX, PosZ, ground, scale, groundScale);

                    GameObject Enemy = Instantiate(myPrefab, new Vector3(PosX, position + 1, PosZ), Quaternion.identity);

                    Enemy.tag = "Enemy";

                    string ScriptName = "DeactiveEnemies";
                    System.Type MyScriptType = System.Type.GetType(ScriptName + ",Assembly-CSharp");

                    Enemy.AddComponent(MyScriptType);

                    times++;
                }

            }
        }
        
    }

    void CheckActivation()
    {
        if (listOfObjects.Count > 0)
        {
            foreach (GameObject item in listOfObjects)
            {
                if (Vector3.Distance(player.transform.position, item.transform.position) < distanceFromPlayer)
                {
                    item.SetActive(true);
                    listOfObjects.Remove(item);
                }

                if (Vector3.Distance(player.transform.position, item.transform.position) > maxDistance)
                {
                    item.SetActive(true);
                    listOfObjects.Remove(item);
                    times--;
                }
            }
        }

    }

    public void AddToList(GameObject newEnemy)
    {
        listOfObjects.Add(newEnemy);
    }

}
