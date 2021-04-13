using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelTerrain.Grid;

public class Manager : MonoBehaviour
{

    public Resources allResources;
    public VirtualCurrency virtualCurrency;
    Manager manager;
    public GameObject buildTabScroll;
    bool buildSelectorVisible = false;
    public Sprite nullIcon;
    List<int> unlockedBuildOptionsList = new List<int>();
    List<GameObject> unlockedSpriteList = new List<GameObject>();
    float iconSize = 128f;
    float gapSize = 16f;
    GameObject iconHolder;
    public GameObject newPlaceIndicatorPrefab;
    GameObject newPlaceIndicator;
    public Font theFont;
    float panelHeight;
    public LayerMask ignoreLayer;
    bool prevMenuState = true;
    public Text moneyText;
    int currentSelection = 0;
    private List<GameObject> placeHolderPrefabs = new List<GameObject>();
    public List<PlayerBaseData> bases = new List<PlayerBaseData>();
    float initialisationPause = 1f;
    bool started = false;

    #region Singleton
    private void OnEnable()
    {
        if (manager == null)
        {
            manager = this;
        }
        else
        {
            if (manager != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }//Singleton Enrico
    #endregion

    //private void Awake()
    //{
       
    //    virtualCurrency = FindObjectOfType<VirtualCurrency>();

    //}

    //private void Start()
    //{
    //    //  StartCoroutine(StartAfterPause());
    //    OnStart();
    //    StartCoroutine(CheckTimer());
    //}

  //  IEnumerator StartAfterPause()
    public void OnStart()
    {
        //  yield return new WaitForSeconds(initialisationPause);
    
        
        newPlaceIndicator = GameObject.Instantiate(newPlaceIndicatorPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        buildTabScroll.transform.position = new Vector2(-1000f, -1000f);
        iconHolder = buildTabScroll.transform.Find("IconHolder").gameObject;
        RebuildSelectableBuildings();
        moneyText.text = virtualCurrency.SealsCurrency.ToString();
        started = true;
        bool first = true;
        foreach(PlayerBaseData playerBase in bases)
        {
            GameObject theBase = GameObject.Instantiate(playerBase.basePlate, playerBase.basePlatePostion, Quaternion.identity);
            Camera.main.GetComponent<CameraControl>().playerPoint = theBase.gameObject;
            foreach (PlayerBaseData.BuildingInfo building in playerBase.baseBuildings)
            {
                GameObject go = GameObject.Instantiate(building.type.prefab, playerBase.basePlatePostion + building.buildingPosOffset, Quaternion.Euler(0, building.buildingRot, 0));
                go.SetActive(true);
            }
            if (first)
            {
                GameObject VE = GameObject.Find("VoxelEngine");
                if (VE == null) { break; }
                VE.GetComponent<VoxelTerrain.Voxel.InfoData.WorldInfo>().Origin = theBase.transform;
                StartCoroutine(StartAfterPause(playerBase.basePlatePostion, playerBase.playerShip));
                

            }

        }
    }

    IEnumerator StartAfterPause(Vector3 startPoint, GameObject playerShip)
    {
        yield return new WaitForSeconds(initialisationPause);
        GameObject player = GameObject.Instantiate(playerShip, startPoint + new Vector3(0,1,0), Quaternion.identity);
    }

    public void RebuildSelectableBuildings()
    {
        //Clear icons
        foreach (GameObject g2 in unlockedSpriteList)
        {
            Destroy(g2);
        }
        unlockedSpriteList.Clear();
        foreach(GameObject g2 in placeHolderPrefabs)
        {
            Destroy(g2);
        }
        unlockedBuildOptionsList.Clear();
        placeHolderPrefabs.Clear();
        //rebuild;
        for (int i = 0; i < allResources.resourceInfo.Length; i++)
        {
            if (allResources.resourceInfo[i].unlocked)
            {
                unlockedBuildOptionsList.Add(i);
                GameObject go = GameObject.Instantiate(allResources.resourceInfo[i].prefab,newPlaceIndicator.transform.position,newPlaceIndicator.transform.rotation);

                go.name = allResources.resourceInfo[i].prefab.name;////////////////////////////////////////// THIS REMOVES THE "(CLONE)" IN THE NAME OF THE ITEM. Needed to pass a sting for the purchase

                go.transform.parent = newPlaceIndicator.transform;
                placeHolderPrefabs.Add(go);
            }
        }


        int unlockedCount = unlockedBuildOptionsList.Count;
        float scrollPlanelHeight = ((unlockedCount + 2) * iconSize) + ((unlockedCount + 3) * gapSize);
        float scrollPlaneWidth = iconSize + (gapSize * 2);
        float startY = ((scrollPlanelHeight / 2)) - gapSize - (iconSize / 2);
        panelHeight = startY * 2;
        RectTransform iconHolderRectTransform = iconHolder.GetComponent<RectTransform>();

        iconHolderRectTransform.sizeDelta = new Vector2(scrollPlaneWidth, scrollPlanelHeight);
        iconHolderRectTransform.position = buildTabScroll.transform.position - new Vector3(0f, scrollPlanelHeight / 2f);

        GameObject g = new GameObject();
        g.name = "Null";
        g.transform.parent = iconHolder.transform;
        RectTransform tr = g.AddComponent<RectTransform>();
        tr.position = iconHolder.transform.position + new Vector3(0f, startY);
        startY -= gapSize + iconSize;
        tr.sizeDelta = new Vector2(iconSize, iconSize);
        Image sr = g.AddComponent<Image>();
        sr.sprite = nullIcon;
        unlockedSpriteList.Add(g);
        Text costText;
        for (int i = 0; i < unlockedBuildOptionsList.Count; i++)
        {


            g = new GameObject();
            g.name = allResources.resourceInfo[unlockedBuildOptionsList[i]].resourceName;
            g.transform.parent = iconHolder.transform;
            tr = g.AddComponent<RectTransform>();
            tr.position = iconHolder.transform.position + new Vector3(0f, startY);


            tr.sizeDelta = new Vector2(iconSize, iconSize);
            sr = g.AddComponent<Image>();
            sr.sprite = allResources.resourceInfo[unlockedBuildOptionsList[i]].iconSprite;

            unlockedSpriteList.Add(g);
            GameObject t = new GameObject();
            unlockedSpriteList.Add(t);
            t.name = allResources.resourceInfo[unlockedBuildOptionsList[i]].resourceName + " Cost";
            t.transform.parent = iconHolder.transform;
            costText = t.AddComponent<Text>();
            tr = t.GetComponent<RectTransform>();
            tr.position = iconHolder.transform.position + new Vector3(0f, startY - (iconSize / 2f) - gapSize * 2);
            costText.text = allResources.resourceInfo[unlockedBuildOptionsList[i]].buildingCost.ToString();

            if (allResources.resourceInfo[unlockedBuildOptionsList[i]].buildingCost > virtualCurrency.SealsCurrency)
            {
                costText.color = Color.red;
            }
            else
            {
                costText.color = Color.black;
            }

            costText.font = theFont;
            costText.fontSize = 24;
            startY -= gapSize + iconSize;
        }

        g = new GameObject();
        g.name = "Null";
        g.transform.parent = iconHolder.transform;
        tr = g.AddComponent<RectTransform>();
        tr.position = iconHolder.transform.position + new Vector3(0f, startY);
        startY -= gapSize + iconSize;
        tr.sizeDelta = new Vector2(iconSize, iconSize);
        sr = g.AddComponent<Image>();
        sr.sprite = nullIcon;
        unlockedSpriteList.Add(g);
    }

    public void BuildSelector()
    {
        for (int i = 0; i < unlockedBuildOptionsList.Count; i++)
        {
            if (i == currentSelection)
            {
                placeHolderPrefabs[i].SetActive(true);
            }
            else
            {
                placeHolderPrefabs[i].SetActive(false);
            }

        }

        Color canBuildColor = Color.white;
        Color placeIndicatorColor = Color.green;
        if (allResources.resourceInfo[unlockedBuildOptionsList[currentSelection]].buildingCost > virtualCurrency.SealsCurrency)
        {
            placeIndicatorColor = Color.red;
            canBuildColor = Color.red;
        }
        newPlaceIndicator.GetComponent<MeshRenderer>().material.color = placeIndicatorColor;
        if (allResources.resourceInfo[unlockedBuildOptionsList[currentSelection]].prefab.GetComponent<MeshRenderer>() != null)
        {
            allResources.resourceInfo[unlockedBuildOptionsList[currentSelection]].prefab.GetComponent<MeshRenderer>().sharedMaterial.color = canBuildColor;
        }

        foreach (Transform child in allResources.resourceInfo[unlockedBuildOptionsList[currentSelection]].prefab.transform)
        {
            if (child.gameObject.GetComponent<MeshRenderer>() != null)
            {
                child.gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = canBuildColor;
            }
        }

    }

    public void MakeBuildSelectorVisible()
    {

        buildSelectorVisible = !buildSelectorVisible;
        prevMenuState = !buildSelectorVisible;
        if (buildSelectorVisible)
        {
            BuildSelector();
        }
    }
    public bool TryToBuild()
    {
        if (allResources.resourceInfo[unlockedBuildOptionsList[currentSelection]].buildingCost > virtualCurrency.SealsCurrency)
        {
            return false;
        }
        else
        {
            GameObject toBeCloned = placeHolderPrefabs[currentSelection];
            GameObject newBuilding = GameObject.Instantiate(toBeCloned, newPlaceIndicator.transform.position, newPlaceIndicator.transform.rotation);
            // newBuilding.transform.localScale = new Vector3(newBuilding.transform.localScale.x, placeIndicator.transform.localScale.y * newBuilding.transform.localScale.y, newBuilding.transform.localScale.z);

            newBuilding.transform.localScale = new Vector3(newBuilding.transform.localScale.x, newPlaceIndicator.transform.localScale.y * newBuilding.transform.localScale.y, newBuilding.transform.localScale.z);
            //  Material newMat = new Material(newBuilding.GetComponent<MeshRenderer>().material);
            //  newBuilding.GetComponent<MeshRenderer>().material = newMat;
            //Note for Jack. Use one material.

            //USING THE "toBeCloned" NAME AS A STRING FOR THE ITEM PUCHASE ON CLOUD
            virtualCurrency.PurchaseUpgrade(toBeCloned.name, allResources.resourceInfo[unlockedBuildOptionsList[currentSelection]].buildingCost);
            moneyText.text = virtualCurrency.SealsCurrency.ToString();

            return true;
        }
    }

    public void Update()
    {
        if (started)
        {
            moneyText.text = virtualCurrency.SealsCurrency.ToString();

            if (Input.GetKeyDown(KeyCode.F5)){virtualCurrency.AddCurrency();} //CHEAT FOR ADFDING MONEY TO INVENTORY

            if (buildSelectorVisible)
            {
                buildTabScroll.transform.position = Input.mousePosition + new Vector3(iconSize, 0f);
                if (Input.mouseScrollDelta != Vector2.zero)
                {
                    if (Input.mouseScrollDelta.y < 0f)
                    {
                        currentSelection++;
                    }
                    if (Input.mouseScrollDelta.y > 0f)
                    {
                        currentSelection--;
                    }

                    if (currentSelection < 0) currentSelection = 0;
                    if (currentSelection >= unlockedBuildOptionsList.Count) currentSelection = unlockedBuildOptionsList.Count - 1;

                    if (unlockedBuildOptionsList.Count < 2)
                    {
                        iconHolder.transform.position = buildTabScroll.transform.position;
                    }
                    else
                    {
                        iconHolder.transform.position = buildTabScroll.transform.position - new Vector3(0f, -72f + (72f * unlockedBuildOptionsList.Count) - (((gapSize + iconSize)) * currentSelection));
                    }
                    BuildSelector();
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //cast a ray
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, ~ignoreLayer)) //if the ray hits something
                {
                    // Vector3 weaponFocus = hit.point;
                    // Vector3 snap = new Vector3(Mathf.RoundToInt(weaponFocus.x), Mathf.RoundToInt(weaponFocus.y), Mathf.RoundToInt(weaponFocus.z));
                    // newPlaceIndicator.transform.position = snap;

                    newPlaceIndicator.transform.position = GridSnapper.SnapToGrid(hit.point);
                }

            }
            else
            {
                buildTabScroll.transform.position = new Vector2(-1000f, -1000f);
                newPlaceIndicator.transform.position = new Vector3(0f, 0f, 0f);
                //for (int i = 0; i < allResources.resourceInfo.Length; i++)
                //{
                //    allResources.resourceInfo[i].prefab.SetActive(false);
                //}
            }
        }
    }

    #region LOOTBOX_TIMER
    /// <summary>
    /// Stuff for LootBox logic
    /// </summary>


    public Button rewardButton;
    public Text timer;

    public void callGrantLootBox()
    {
        virtualCurrency.GrantLootBox();
        DateTime CurrentTime = DateTime.Now.AddMinutes(5); ///1440 minutes in a day. 
        CurrentTime.SaveDate();
        StopAllCoroutines();
        StartCoroutine(CheckTimer());
    }

    public IEnumerator CheckTimer()
    {
        DateTime SavedTime = DateTimeExtension.GetSavedDate();
        TimeSpan RemaningTime = SavedTime.Subtract(DateTime.Now);
        if (RemaningTime.TotalMinutes > 0)
        {
            rewardButton.gameObject.SetActive(false);
        }
        else
        {
            rewardButton.gameObject.SetActive(true);
            timer.text = "Collect Reward!!";
        }

        while (RemaningTime.TotalMinutes > 0)
        {
            RemaningTime = SavedTime.Subtract(DateTime.Now);
            timer.text = "Wait For Reward \n" + RemaningTime.Hours + ":" + RemaningTime.Minutes + ":" + RemaningTime.Seconds;
            yield return new WaitForSeconds(1f);
        }
        if (RemaningTime.TotalMinutes <= 0)
        {
            rewardButton.gameObject.SetActive(true);
            timer.text = "Collect Reward!!";
        }
    }
}

public static class DateTimeExtension
{
    public static void SaveDate(this DateTime _date, string Key = "SavedDate") { string d = Convert.ToString(_date); PlayerPrefs.SetString(Key, d); }
    public static DateTime GetSavedDate(string key = "SavedDate") { if (PlayerPrefs.HasKey("SavedDate")) { string d = PlayerPrefs.GetString("SavedDate"); return Convert.ToDateTime(d); } else { return DateTime.Now; } }
}
#endregion

