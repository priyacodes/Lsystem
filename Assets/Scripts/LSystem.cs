using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// The class used for integrating the UI with the configuration files of the L-Systems
// The data is fetched and the L-System sentence is generated in this script

[System.Serializable]
public class LSystem
{
    // Text fields to display the corresponding data from the configuration file
    public Text axiomDisplay;
    public Text generationDisplay;
    public Text angleDisplay;
    public Text lengthDisplay;
    public Text Rule1, Rule2, Rule3;

    // Gameobjects holding the Secondary and Tertiary Rule groups which are displayed/hidden based on number of rules in L-System
    public GameObject Rule2Group, Rule3Group;

    //Interactive elements of the UI 
    public Slider angleSlider;
    public Slider genSlider;
    public Slider lenSlider;
    
    [HideInInspector]
    public PresetTreeRules PST;             // Object of PresetTreeRules class

    // Selected object to which the parameters are assigned
    public PresetTrees SelectedPresetTree;      
    public StochasticTrees SelectedStocTree;

    public int ruleCount;       // Rule count to determine the display of Rule groups
    private int TreeID;         // Tree ID used to differentiate between Deterministic and stochastic trees
   
    public static string sentence;  // Static variable to store the generated L-System sentence string



    // Pick the Tree based on ID
    // Function called in the Generator script when the UI button is clicked
    public void AssignTree(int id)
    {
        TreeID = id;
        switch(id)
        {
            case 0:
                SelectedPresetTree = PST.SelectPresetTree(0);
                break;

            case 1:
                SelectedPresetTree = PST.SelectPresetTree(1);
                break;

            case 2:
                SelectedPresetTree = PST.SelectPresetTree(2);
                break;

            case 3:
                SelectedPresetTree = PST.SelectPresetTree(3);
                break;

            case 4:
                SelectedPresetTree = PST.SelectPresetTree(4);
                break;

            case 5:
                SelectedPresetTree = PST.SelectPresetTree(5);
                break;

            case 6:
                SelectedStocTree = PST.SelectStocTree(0);
                break;

            case 7:
                SelectedStocTree = PST.SelectStocTree(1);
                break;


        }

        // Based on ID finds which funtion to invoke

        if (id < 6)
        {
            GeneratePSentence();    // Generate sentence for deterministic trees
            AssignPUI();            // Update the UI 
        }

        else
        {
            AssignSUI();            // Generate sentence for stochastic trees
            GenerateSSentence();    // Update the UI 
        }
    }

    // Updates the angle value in UI when the slider value is changed by user interaction
    public void UpdateAngleUI()
    {
        angleDisplay.text = angleSlider.value.ToString("f1");

    }

    // Updates the Generation value in UI when the slider value is changed by user interaction
    // Sentence is regenerated
    public void UpdateGenerationUI()
    {
        generationDisplay.text = genSlider.value.ToString("f0");
        SelectedPresetTree.generations = SelectedStocTree.generations = int.Parse(genSlider.value.ToString());
        if (TreeID < 6)
            GeneratePSentence();
        else
            GenerateSSentence();
    }


    // Updates the length value in UI when the slider value is changed by user interaction
    public void UpdateLengthUI()
    {
        lengthDisplay.text = lenSlider.value.ToString("f1");

    }

    // UI is updated with values from Preset Trees class
    private void AssignPUI()
    {
        axiomDisplay.text = SelectedPresetTree.axiom.ToString();

        generationDisplay.text = SelectedPresetTree.generations.ToString();
        genSlider.value = genSlider.maxValue = SelectedPresetTree.generations;  // Config values are set as max cap values for the sliders

        angleDisplay.text = SelectedPresetTree.angle.ToString();
        angleSlider.value = SelectedPresetTree.angle;

        lengthDisplay.text = SelectedPresetTree.length.ToString("f1");
        lenSlider.value = lenSlider.maxValue = SelectedPresetTree.length;

        ruleCount = SelectedPresetTree.rules.Count;
        Rule1.text = SelectedPresetTree.rules['F'];
        if (SelectedPresetTree.rules.Count > 1)         // If there is more than one rule count, then turns on Rule2 group
        {
            Rule2.text = SelectedPresetTree.rules['X'];
            Rule2Group.SetActive(true);
        }

        else
        {
            Rule2Group.SetActive(false);
        }

        Rule3Group.SetActive(false);
      
    }

    // Generates the L-System sentence for deterministic trees
    public void GeneratePSentence()
    {        

        sentence = SelectedPresetTree.axiom.ToString();

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < SelectedPresetTree.generations; i++)
        {   
            // For each character in sentence, if the selected tree has a production for that character, 
            // then replace the string else use the same character
            // Append it to the sentence

            foreach (char c in sentence)        
            {   
                stringBuilder.Append(SelectedPresetTree.rules.ContainsKey(c) ? SelectedPresetTree.rules[c] : c.ToString());
            }

            sentence = stringBuilder.ToString();
            stringBuilder = new StringBuilder();
        }

        Debug.Log(sentence);
    }


    // UI is updated with values from Stochastic Trees class
    private void AssignSUI()
    {
        axiomDisplay.text = SelectedStocTree.axiom.ToString();

        generationDisplay.text = SelectedStocTree.generations.ToString();
        genSlider.value = genSlider.maxValue = SelectedStocTree.generations;

        angleDisplay.text = SelectedStocTree.angle.ToString();
        angleSlider.value = SelectedStocTree.angle;

        lengthDisplay.text = SelectedStocTree.length.ToString("f1");
        lenSlider.value = lenSlider.maxValue = SelectedStocTree.length;

        ruleCount = SelectedStocTree.Stocrules.Count;
        Rule1.text = SelectedStocTree.Stocrules['F'].subRule1;
        Rule3.text = SelectedStocTree.Stocrules['F'].subRule2;
        Rule2.text = SelectedStocTree.Stocrules['X'].subRule1;
        Rule2Group.SetActive(true);
        Rule3Group.SetActive(true);

    }

    // Generates the L-System sentence for stochastic trees
    public void GenerateSSentence()
    {

        sentence = SelectedStocTree.axiom.ToString();

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < SelectedStocTree.generations; i++)
        {
            foreach (char c in sentence)
            {

                // For each character in sentence, if the selected stochastic tree has a production for that character, 
                // then pick a random int between 0 and 1
                
                if (SelectedStocTree.Stocrules.ContainsKey(c))
                {
                    int choice = Random.Range(0, 2);    // Range is (0,2) because Unity doesn't generate the max value in range
                    
                    // If random choice is 0, append subrule1 else append subrule2
                    stringBuilder.Append(choice == 0 ? SelectedStocTree.Stocrules[c].subRule1 : SelectedStocTree.Stocrules[c].subRule2);
                }
                else
                {
                    stringBuilder.Append(c.ToString());
                }
              
            }

            sentence = stringBuilder.ToString();
            stringBuilder = new StringBuilder();
        }

        Debug.Log(sentence);
    }


}