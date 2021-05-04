using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformInfo
{
    public Vector3 position;
    public Quaternion rotation;
    public float branchWidth;
}

public class TreeLSystem : MonoBehaviour
{
#pragma warning disable 0649
    private Stack<TransformInfo> transformStack = new Stack<TransformInfo>();
    [SerializeField] private GameObject branchPrefab;
    [SerializeField] private GameObject leafPrefab;
    [Range(0, 10)] [SerializeField] private int treeType = 0;
    [SerializeField] private int trunkLength = 1;
    [SerializeField] private int branchCount = 4;
    [SerializeField] private float branchLength = 10;
    [SerializeField] private float branchWidth = 0.3f;
    [SerializeField] private float branchTaper = 1f;
    [SerializeField] private float branchAngle = 30;
    [SerializeField] private float angleVariation = 10;
    [SerializeField] private float leafSize = 0.2f;

    private const string axiom = "X";
    private string treeString = "";
    private Dictionary<char, string>[] rules =
    {
        new Dictionary<char, string> { { 'X', "[F-[X+X]+F[+FX]-X]" }, { 'F', "FF" } },
        new Dictionary<char, string> { { 'X', "[-FX][+FX][FX]" }, { 'F', "FF" } },
        new Dictionary<char, string> { { 'X', "[-FX]X[+FX][+F-FX]" }, { 'F', "FF" } },
        new Dictionary<char, string> { { 'X', "[FF[+XF-F+FX]--F+F-FX]" }, { 'F', "FF" } },
        new Dictionary<char, string> { { 'X', "[FX[+F[-FX]FX][-F-FXFX]]" }, { 'F', "FF" } },
        new Dictionary<char, string> { { 'X', "[F[+FX][*+FX][/+FX]]" }, { 'F', "FF" } },
        new Dictionary<char, string> { { 'X', "[*+FX]X[+FX][/+F-FX]" }, { 'F', "FF" } },
        new Dictionary<char, string> { { 'X', "[F[-X+F[+FX]][*-X+F[+FX]][/-X+F[+FX]-X]]" }, { 'F', "FF" } },
        new Dictionary<char, string> { { 'X', "[FFF[-X+F[+FX]][*-X+F[+FX]][/-X+F[+FX]-X]]" }, { 'F', "FF" } },
        new Dictionary<char, string> { { 'X', "[[-X+F[+FX]]F[*-X+F[+FX]][/-X+F[+FX]-X]]" }, { 'F', "FF" } },
        new Dictionary<char, string> { { 'X', "[[*-X+F[+FX]]F[/-X+F[+FX]-X]F[*-X+F[+FX]]F[/-X+F[+FX]-X]F[*-X+F[+FX]]F[/-X+F[+FX]-X]F[*-X+F[+FX]]F[/-X+F[+FX]-X]]" }, { 'F', "FF" } }
    };
#pragma warning restore 0649

    // Start is called before the first frame update
    void Start()
    {
        GenerateTree();
    }

    public void GenerateTree()
    {
        if (!Application.isPlaying)
        {
            ClearTree();
        }

        treeString = axiom;

        string currentString = "";

        for (int i = 0; i < branchCount; i++)
        {
            foreach (char c in treeString)
            {
                if (rules[treeType].ContainsKey(c))
                {
                    currentString += rules[treeType][c];
                }
                else
                {
                    currentString += c.ToString();
                }
            }

            treeString = currentString;
            currentString = "";
        }

        string trunkString = new string('F', trunkLength);
        treeString = treeString.Insert(1, trunkString);

        Debug.Log(treeString);

        float currentBranchWidth = branchWidth / 10;

        for(int i = 0; i < treeString.Length; i++)
        {
            switch (treeString[i])
            {
                case 'F':
                    Vector3 initialBranchPosition = transform.position;

                    LineRenderer branchObject = new LineRenderer();
                    
                    if (treeString[i + 1] == 'X' || treeString[i + 11] == 'X' || treeString[i + 3] == 'F' && treeString[i + 4] == 'X')
                    {
                        Quaternion storedRotation = transform.rotation;
                        transform.rotation = Quaternion.identity;
                        transform.Rotate(Vector3.forward * 180);
                        transform.Translate(Vector3.up * (leafSize / 5));
                        branchObject = Instantiate(leafPrefab, transform).GetComponent<LineRenderer>();
                        branchObject.startWidth = leafSize / 10;
                        branchObject.endWidth = leafSize / 10;
                        transform.rotation = storedRotation;
                    }
                    else
                    {
                        transform.Translate(Vector3.up * (branchLength / 10));
                        branchObject = Instantiate(branchPrefab, transform).GetComponent<LineRenderer>();
                        branchObject.startWidth = currentBranchWidth;
                        currentBranchWidth -= branchTaper / 100;
                        branchObject.endWidth = currentBranchWidth;
                    }
                    branchObject.SetPosition(0, initialBranchPosition);
                    branchObject.SetPosition(1, transform.position);
                    break;
                case 'X':
                    break;
                case '+':
                    transform.Rotate(Vector3.back * (branchAngle + Random.Range(-angleVariation, angleVariation)));
                    break;
                case '-':
                    transform.Rotate(Vector3.forward * (branchAngle + Random.Range(-angleVariation, angleVariation)));
                    break;
                case '*':
                    transform.Rotate(Vector3.up * 120);
                    break;
                case '/':
                    transform.Rotate(Vector3.down * 120);
                    break;
                case '[':
                    transformStack.Push(new TransformInfo() { position = transform.position, rotation = transform.rotation, branchWidth = currentBranchWidth });
                    break;
                case ']':
                    TransformInfo ti = transformStack.Pop();
                    currentBranchWidth = ti.branchWidth;
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;
            }
        }
    }

    public void ClearTree()
    {
        if (Application.isPlaying)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            var tempList = transform.Cast<Transform>().ToList();
            foreach (var child in tempList)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
