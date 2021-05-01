using System.Collections;
using System.Collections.Generic;
using TerrainData;
using UnityEngine;
using VoxelTerrain.Grid;
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


    [SerializeField] public int distanceFromPlayer;
    [SerializeField] float maxDistance;
    [SerializeField] float totalEnemies;

    public List<GameObject> listOfObjects;

    // Start is called before the first frame update
    void Start()
    {
    }

    private float getPosition(float PosX, float PosZ, float ground, float scale, float groundScale)
    {

        float position = 0;

        float altitude = Noise.Generate2DNoiseValue(PosX, PosZ, scale, 4, 2f, _engine.WorldInfo.Seed, ground);

        if(altitude < groundScale)
        {
             position =  groundScale - altitude;
        }
        else if (altitude > groundScale)
        {
             position = altitude - groundScale;
        }
            return position;
    }


    private bool validatePosition(float PosX, float PosZ, float ground, float scale, float groundScale)
    {

        var seed = _engine.WorldInfo.Seed;

        float altitude = Noise.Generate2DNoiseValue(PosX, PosZ, scale, 4, 2f, seed, ground);
        float groundAltitude = altitude - groundScale;
        float altitude2 = Noise.Generate2DNoiseValue(PosX + 0.5f, PosZ, scale, 4, 2f, seed, ground);
        float groundAltitude2 = altitude - groundScale;
        float altitude3 = Noise.Generate2DNoiseValue(PosX - 0.5f, PosZ, scale, 4, 2f, seed, ground);
        float groundAltitude3 = altitude3 - groundScale;
        float altitude4 = Noise.Generate2DNoiseValue(PosX, PosZ + 0.5f, scale, 4, 2f, seed, ground);
        float groundAltitude4 = altitude4 - groundScale;
        float altitude5 = Noise.Generate2DNoiseValue(PosX, PosZ - 0.5f, scale, 4, 2f, seed, ground);
        float groundAltitude5 = altitude5 - groundScale;


        Vector3 testSnap = GridSnapper.SnapToGrid(new Vector3(PosX, groundAltitude, PosZ), 1f, 0.5f);
        Vector3 testSnap2 = GridSnapper.SnapToGrid(new Vector3(PosX, groundAltitude2, PosZ), 1f, 0.5f);
        Vector3 testSnap3 = GridSnapper.SnapToGrid(new Vector3(PosX, groundAltitude3, PosZ), 1f, 0.5f);
        Vector3 testSnap4 = GridSnapper.SnapToGrid(new Vector3(PosX, groundAltitude4, PosZ), 1f, 0.5f);
        Vector3 testSnap5 = GridSnapper.SnapToGrid(new Vector3(PosX, groundAltitude5, PosZ), 1f, 0.5f);

        if (testSnap2.y > (testSnap.y) || testSnap2.y < (testSnap.y))
        {
            return false;
        }

        if (testSnap3.y > (testSnap.y) || testSnap3.y < (testSnap.y))
        {
            return false;
        }

        if (testSnap4.y > (testSnap.y) || testSnap4.y < (testSnap.y))
        {
            return false;
        }

        if (testSnap5.y > (testSnap.y) || testSnap5.y < (testSnap.y))
        {
            return false;
        }

        float position = getPosition(PosX, PosZ, ground, scale, groundScale);
        var transform = new Vector3(PosX, position + 1, PosZ);

        if (listOfObjects.Count > 0)
        {
            foreach (GameObject item in listOfObjects)
            {
                if (Vector3.Distance(testSnap, item.transform.position) < 30)
                {
                    return false;
                }

            }
        }

        return true;

    }

    // Update is called once per frame
    void Update()
    {

        CheckActivation();


        if (times >= totalEnemies)
        {
            continueSpawn = false;
        }

        if (times < totalEnemies)
        {
            continueSpawn = true;
        }


        if (GameObject.FindWithTag("Player") == null)
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

                float ground = _engine.WorldInfo.GroundLevel;
                float scale = _engine.NoiseScale;

                float groundScale = (ground * scale);

                float position = 0;

                if (validatePosition(PosX, PosZ, ground, scale, groundScale))
                {
                    position = getPosition(PosX, PosZ, ground, scale, groundScale);

                    Vector3 testSnap = GridSnapper.SnapToGrid(new Vector3(PosX, position, PosZ), 1f, 0.5f);

                    GameObject Enemy = Instantiate(myPrefab, testSnap, Quaternion.identity);
                    Rigidbody enemyRigidBody = Enemy.AddComponent<Rigidbody>();
                    enemyRigidBody.useGravity = true;
                    enemyRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

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
                    listOfObjects.Remove(item);
                    Destroy(item);
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
