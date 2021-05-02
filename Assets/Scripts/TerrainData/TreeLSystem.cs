using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformInfo
{
    public Vector3 position;
    public Quaternion rotation;
}

public class TreeLSystem : MonoBehaviour
{
#pragma warning disable 0649
    private Stack<TransformInfo> transformStack = new Stack<TransformInfo>();
    [SerializeField] private GameObject branchPrefab;
    [SerializeField] private GameObject leafPrefab;
    [Range(0, 7)] [SerializeField] private int treeType = 0;
    [SerializeField] private int branchCount = 4;
    [SerializeField] private float branchLength = 10;
    [SerializeField] private float branchAngle = 30;

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
        new Dictionary<char, string> { { 'X', "[F[-X+F[+FX]][*-X+F[+FX]][/-X+F[+FX]-X]]" }, { 'F', "FF" } }
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

        for(int i = 0; i < treeString.Length; i++)
        {
            switch (treeString[i])
            {
                case 'F':
                    Vector3 initialBranchPosition = transform.position;
                    transform.Translate(Vector3.up * branchLength);

                    LineRenderer branchObject = new LineRenderer();
                    if (treeString[(i + 1) % treeString.Length] == 'X' || treeString[(i + 3) % treeString.Length] == 'F' && treeString[(i + 4) % treeString.Length] == 'X')
                    {
                        branchObject = Instantiate(leafPrefab, transform).GetComponent<LineRenderer>();
                    }
                    else
                    {
                        branchObject = Instantiate(branchPrefab, transform).GetComponent<LineRenderer>();
                    }
                    branchObject.SetPosition(0, initialBranchPosition);
                    branchObject.SetPosition(1, transform.position);
                    branchObject.startWidth = 5;
                    branchObject.endWidth = 5;
                    break;
                case 'X':
                    break;
                case '+':
                    transform.Rotate(Vector3.back * branchAngle);
                    break;
                case '-':
                    transform.Rotate(Vector3.forward * branchAngle);
                    break;
                case '*':
                    transform.Rotate(Vector3.up * 120);
                    break;
                case '/':
                    transform.Rotate(Vector3.down * 120);
                    break;
                case '[':
                    transformStack.Push(new TransformInfo() { position = transform.position, rotation = transform.rotation });
                    break;
                case ']':
                    TransformInfo ti = transformStack.Pop();
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
