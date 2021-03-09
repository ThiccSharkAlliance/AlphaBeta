using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [System.Serializable]
    public class ResourceInfo 
    {
        public string resourceName;
        public int buildingCost;
        public bool unlocked;
        public Sprite iconSprite;
        public GameObject prefab;
        public int Damage;
    }
    
    public ResourceInfo[] resourceInfo;

    VirtualCurrency virtualCurrency;
    Manager manager;


    public GameObject buildTabScroll;
    bool buildSelectorVisible = false;
    public Sprite nullIcon;
    List<int> unlockedBuildOptionsList = new List<int>();
    List<GameObject> unlockedSpriteList = new List<GameObject>();
    float iconSize = 128f;
    float gapSize = 16f;
    GameObject iconHolder;
    public GameObject placeIndicator;
    public Font theFont;
    float panelHeight;
    public LayerMask ignoreLayer;
    bool prevMenuState = true;
    public Text moneyText;

    int currentSelection = 0;

    /// 
    //private void OnEnable()
    //{
    //    if (manager == null)
    //    {
    //        manager = this;
    //    }
    //    else
    //    {
    //        if (manager != this)
    //        {
    //            Destroy(this.gameObject);
    //        }
    //    }
    //    DontDestroyOnLoad(this.gameObject);
    //}//Singleton Enrico
    ///

    private void Awake()
    {
       
        virtualCurrency = FindObjectOfType<VirtualCurrency>();

    }

    private void Start()
    {
        placeIndicator.transform.position = new Vector3(0f, -1000f, 0f);
        buildTabScroll.transform.position = new Vector2(-1000f, -1000f);
        iconHolder = buildTabScroll.transform.Find("IconHolder").gameObject;
        RebuildSelectableBuildings();
        moneyText.text = virtualCurrency.SealsCurrency.ToString();
        Debug.Log(virtualCurrency.SealsCurrency.ToString());
    }

    public void RebuildSelectableBuildings()
    {
        //Clear icons
        unlockedBuildOptionsList.Clear();

        foreach (GameObject g2 in unlockedSpriteList)
        {
            Destroy(g2);
        }
        unlockedSpriteList.Clear();


        //rebuild;
        for (int i = 0; i < resourceInfo.Length; i++)
        {
            if (resourceInfo[i].unlocked)
            {
                unlockedBuildOptionsList.Add(i);
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
            g.name = resourceInfo[unlockedBuildOptionsList[i]].resourceName;
            g.transform.parent = iconHolder.transform;
            tr = g.AddComponent<RectTransform>();
            tr.position = iconHolder.transform.position + new Vector3(0f, startY);


            tr.sizeDelta = new Vector2(iconSize, iconSize);
            sr = g.AddComponent<Image>();
            sr.sprite = resourceInfo[unlockedBuildOptionsList[i]].iconSprite;

            unlockedSpriteList.Add(g);
            GameObject t = new GameObject();
            unlockedSpriteList.Add(t);
            t.name = resourceInfo[unlockedBuildOptionsList[i]].resourceName + " Cost";
            t.transform.parent = iconHolder.transform;
            costText = t.AddComponent<Text>();
            tr = t.GetComponent<RectTransform>();
            tr.position = iconHolder.transform.position + new Vector3(0f, startY - (iconSize / 2f) - gapSize * 2);
            costText.text = resourceInfo[unlockedBuildOptionsList[i]].buildingCost.ToString();

            if (resourceInfo[unlockedBuildOptionsList[i]].buildingCost > virtualCurrency.SealsCurrency)
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
                resourceInfo[unlockedBuildOptionsList[i]].prefab.SetActive(true);
            }
            else
            {
                resourceInfo[unlockedBuildOptionsList[i]].prefab.SetActive(false);
            }

        }

        Color canBuildColor = Color.white;
        Color placeIndicatorColor = Color.green;
        if (resourceInfo[unlockedBuildOptionsList[currentSelection]].buildingCost > virtualCurrency.SealsCurrency)
        {
            placeIndicatorColor = Color.red;
            canBuildColor = Color.red;
        }
        placeIndicator.GetComponent<MeshRenderer>().material.color = placeIndicatorColor;
        if (resourceInfo[unlockedBuildOptionsList[currentSelection]].prefab.GetComponent<MeshRenderer>() != null)
        {
            resourceInfo[unlockedBuildOptionsList[currentSelection]].prefab.GetComponent<MeshRenderer>().material.color = canBuildColor;
        }

        foreach (Transform child in resourceInfo[unlockedBuildOptionsList[currentSelection]].prefab.transform)
        {
            if (child.gameObject.GetComponent<MeshRenderer>() != null)
            {
                child.gameObject.GetComponent<MeshRenderer>().material.color = canBuildColor;
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
        if (resourceInfo[unlockedBuildOptionsList[currentSelection]].buildingCost > virtualCurrency.SealsCurrency)
        {
            return false;
        }
        else
        {
            GameObject toBeCloned = resourceInfo[unlockedBuildOptionsList[currentSelection]].prefab;
            GameObject newBuilding = GameObject.Instantiate(toBeCloned, toBeCloned.transform.position, toBeCloned.transform.rotation);
            newBuilding.transform.localScale = new Vector3(newBuilding.transform.localScale.x, placeIndicator.transform.localScale.y * newBuilding.transform.localScale.y, newBuilding.transform.localScale.z);
            //  Material newMat = new Material(newBuilding.GetComponent<MeshRenderer>().material);
            //  newBuilding.GetComponent<MeshRenderer>().material = newMat;
            //Note for Jack. Use one material.
            virtualCurrency.SealsCurrency -= resourceInfo[unlockedBuildOptionsList[currentSelection]].buildingCost;

            moneyText.text = virtualCurrency.SealsCurrency.ToString();

            return true;
        }
    }

    public void Update()
    {
        moneyText.text = virtualCurrency.SealsCurrency.ToString();
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
                Vector3 weaponFocus = hit.point;
                Vector3 snap = new Vector3(Mathf.RoundToInt(weaponFocus.x), Mathf.RoundToInt(weaponFocus.y), Mathf.RoundToInt(weaponFocus.z));
                placeIndicator.transform.position = snap;
            }

        }
        else
        {
            buildTabScroll.transform.position = new Vector2(-1000f, -1000f);
            placeIndicator.transform.position = new Vector3(0f, -1000f, 0f);
            for (int i = 0; i < resourceInfo.Length; i++)
            {
                resourceInfo[i].prefab.SetActive(false);
            }
        }
    }
}
