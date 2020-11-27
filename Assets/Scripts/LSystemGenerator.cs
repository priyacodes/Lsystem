using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using UnityEngine.UI;

// Script responsible to integrate all classes and generate the L-Systems
public class LSystemGenerator : MonoBehaviour
{
  
    public LSystem lSystem;     // Object of Lsystem class to call its functions
    public Toggle colorToggle;  // Toggle option to change color mode
    
    private float angle;
    private int factor = 2;              // Value to apply when translating, to match the tree sizes 
    private float length = 1f;           // Length of lines drawn
    private float variance = 10f;        // Variance in angle to achieve the required L-System
    private bool nodeWriting = false;    // Bool to check if the L-System is node rewriting or not

    public GameObject Tree = null;
    public GameObject treeParent;       // Empty prefab to hold the generated line renderers
    public GameObject branch;           // Prefabs 
    public GameObject leaf;

    [SerializeField]
    private Material brown, green, gray;       
    private LineRenderer branchL, leafL;


    private Stack<TransformData> transformStack;
    private string currentString = string.Empty;    // String holding the L-system sentence 
    private Vector3 initialPosition = Vector3.zero;


    private void Start()
    {
        transformStack = new Stack<TransformData>();
        branchL = branch.GetComponent<LineRenderer>();
        leafL = leaf.GetComponent<LineRenderer>();

        // Start the app in Color mode
        branchL.material = brown;
        leafL.material = green;

    }

    // Switching between Color and Grayscale mode. Called in the Toggle event in inspector
    public void ChangeColorMode()
    {
        bool grayed;
        grayed = Grayscale.grayscale = colorToggle.isOn;
    
        if(grayed)
        {
            branchL.material = gray;
            leafL.material = gray;
        }
        else
        {
            branchL.material = brown;
            leafL.material = green;
        }
       
    }

    // Function is called in the OnClick event of the UI buttons in the inspector
    public void SelectTree(int id)
    {
        lSystem.AssignTree(id); // Fetches the tree grammars based on the ID selected
        if (id < 6)
        { 
            angle = lSystem.SelectedPresetTree.angle;
            length = lSystem.SelectedPresetTree.length;
          
        }
        else
        {
            angle = lSystem.SelectedStocTree.angle;
            length = lSystem.SelectedStocTree.length;
        }
    
        nodeWriting = CheckWritingType();
        currentString = LSystem.sentence;   // Sentence is assiged to another variable
        GenerateTree();
    }

    // If rule count is greater than 1, then it is node rewriting type else, edge rewriting type
    private bool CheckWritingType()
    {
        if(lSystem.ruleCount > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update the angle and regenerate tree
    public void UpdateAngle()
    {   
        lSystem.UpdateAngleUI();
        angle = lSystem.angleSlider.value;
        GenerateTree();
    }

    // Update the generations and regenerate tree
    public void UpdateGenerations()
    {
        if (currentString != String.Empty)
        {
            lSystem.UpdateGenerationUI();
            currentString = LSystem.sentence;
            GenerateTree();
        }
    }

    // Update the length and regenerate tree
    public void UpdateLength()
    {
        lSystem.UpdateLengthUI();
        length = float.Parse(lSystem.lenSlider.value.ToString());
        GenerateTree();
    }
   
    // Function responsible for generating the L-System model 
    private void GenerateTree()
    {   
        // Destroy any existing tree
         Destroy(Tree);     
        
        //Resets the position and rotation
        transform.position = Vector3.zero;
        transform.rotation = new Quaternion(0f, 0f, 0f,0f);

        // Instantiate new object
        Tree = Instantiate(treeParent);
      

        for (int i = 0; i < currentString.Length; i++)
        {
            switch (currentString[i])
            {
                case 'F':
                    initialPosition = transform.position;                  // Stores the current position and moves the gameobject upwards in Y-axis
                    transform.Translate(Vector3.up * length * factor);
                    GameObject newLine;
                    
                    // Create branch or leaf based on the character 
                    if (nodeWriting == true)
                    {
                        newLine = currentString[(i + 1) % currentString.Length] == 'X' || currentString[(i + 2) % currentString.Length] == 'F' &&  currentString[(i + 4) % currentString.Length] == 'X' || currentString[(i + 4) % currentString.Length] == ']' || currentString[(i + 1) % currentString.Length] == ']' ? Instantiate(leaf) : Instantiate(branch);
                    }
                    else
                    {
                        newLine = currentString[i % currentString.Length] == 'F' && currentString[(i + 1) % currentString.Length] == ']' && (currentString[(i - 1) % currentString.Length] == '+' || currentString[(i - 1) % currentString.Length] == '-') ? Instantiate(leaf) : Instantiate(branch);
                    }
                    newLine.transform.SetParent(Tree.transform);
                    
                    // Line is drawn between the initial position and the moved position
                    newLine.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                    newLine.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                
                    break;

                case 'X':   // Do nothing
                    break;

                case '+':
                    transform.Rotate(Vector3.forward * angle * (1 + variance / 100));   // Rotate in the positive Z-axis
                    break;

                case '-':
                    transform.Rotate(Vector3.back * angle * (1 + variance / 100));      // Rotate in the negative Z-axis
                    break;

               
                case '[':
                    transformStack.Push(new TransformData()             // Push the transform data to the stack
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;

                case ']':
                    TransformData ti = transformStack.Pop();        // Pop the transform data and assign it to the object
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;

                default:
                    Debug.LogError("Invalid L-system");
                    break;
            }
        }


    }


  
}