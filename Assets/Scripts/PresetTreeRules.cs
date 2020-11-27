using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script acts as the Configuration File as it holds the parameters and rules for the L-Systems 

[System.Serializable]
public class PresetTreeRules
{
    public PresetTrees[] presetTrees = new PresetTrees[6];              // Array of 6 trees
    public StochasticTrees[] stochasticTrees = new StochasticTrees[2];  // Array of 2 trees


    // Returns the deterministic tree configurations based on the ID selected
    // Rules and Parameters are defined in this function
    public PresetTrees SelectPresetTree(int id)
    {
        switch (id)
        {
            case 0:
                presetTrees[0].treeID = 0;                             // ID for the tree
                presetTrees[0].axiom = 'F';                            // Axiom of the grammar
                presetTrees[0].angle = 25.7f;                          // Angle to be applied
                presetTrees[0].generations = 5;                        // Number of generations
                presetTrees[0].length = 1;                             // Length of the line renderer

                presetTrees[0].rules = new Dictionary<char, string>
                {
                    { 'F', "F[+F]F[-F]F" }                            // Production rule of the grammar
                };
                break;

            case 1:
                presetTrees[1].treeID = 1;
                presetTrees[1].axiom = 'F';
                presetTrees[1].angle = 20f;
                presetTrees[1].generations = 5;
                presetTrees[1].length = 4;

                presetTrees[1].rules = new Dictionary<char, string>
                {
                    { 'F', "F[+F]F[-F][F]" }
                };
                break;

            case 2:
                presetTrees[2].treeID = 2;
                presetTrees[2].axiom = 'F';
                presetTrees[2].angle = 22.5f;
                presetTrees[2].generations = 4;
                presetTrees[2].length = 4;

                presetTrees[2].rules = new Dictionary<char, string>
                {
                    { 'F', "FF-[-F+F+F]+[+F-F-F]" }
                };
                break;

            case 3:
                presetTrees[3].treeID = 3;
                presetTrees[3].axiom = 'X';
                presetTrees[3].angle = 20f;
                presetTrees[3].generations = 7;
                presetTrees[3].length = 1;

                presetTrees[3].rules = new Dictionary<char, string>
                {
                    {'X',"F[+X]F[-X]+X" },
                    {'F',"FF" }

                };
                break;

            case 4:
                presetTrees[4].treeID = 4;
                presetTrees[4].axiom = 'X';
                presetTrees[4].angle = 25.7f;
                presetTrees[4].generations = 7;
                presetTrees[4].length = 1;

                presetTrees[4].rules = new Dictionary<char, string>
                {
                    {'X',"F[+X][-X]FX" },
                    {'F',"FF" }
                };
                break;

            case 5:
                presetTrees[5].treeID = 5;
                presetTrees[5].axiom = 'X';
                presetTrees[5].angle = 22.5f;
                presetTrees[5].generations = 5;
                presetTrees[5].length = 3;

                presetTrees[5].rules = new Dictionary<char, string>
                {
                    {'X',"F-[[X]+X]+F[+FX]-X"},
                    {'F',"FF" }

                };
                break;

            


        }

        return presetTrees[id]; //Tree is returned
    }


    // Returns the stochastic tree configurations based on the ID selected 
    // Configurations are defined in this function
    public StochasticTrees SelectStocTree(int id)
    {
        switch (id)
        {
            case 0:
                stochasticTrees[0].treeID = 6;
                stochasticTrees[0].axiom = 'X';
                stochasticTrees[0].angle = 20f;
                stochasticTrees[0].generations = 5;
                stochasticTrees[0].length = 4;

                stochasticTrees[0].Stocrules = new Dictionary<char, StochasticRules>
                {

                    {'X', new StochasticRules{ subRule1 = "F[+F]F[-F]F", subRule2 = "F[+F]F[-F]F" } },  // Same rule for axiom is applied to both subrules, else it could lead in errors 
                    {'F', new StochasticRules { subRule1 = "F[+F]F", subRule2 = "F[-F]F" } }            // Different rules per key to achieve randomisation

                };

                break;

            case 1:
                stochasticTrees[1].treeID = 7;
                stochasticTrees[1].axiom = 'X';
                stochasticTrees[1].angle = 20f;
                stochasticTrees[1].generations = 5;
                stochasticTrees[1].length = 4;

                stochasticTrees[1].Stocrules = new Dictionary<char, StochasticRules>
                {
                     { 'X', new StochasticRules{ subRule1 = "[+FX]X[-FX][+F-FX]", subRule2 = "[+FX]X[-FX][+F-FX]"} },  // Same rule for axiom is applied to both subrules, else it could lead in errors 
                     { 'F', new StochasticRules{ subRule1 ="FF", subRule2 ="F" } }                                     // Different rules per key to achieve randomisation
                  
                };

                break;
        }
        return stochasticTrees[id];  // Tree is returned
    }

}

// Class for the Deterministic L-Systems, where there is only one rule per character.
// Dictionary can hold only one value per key

[System.Serializable]
public class PresetTrees 
{
    public int treeID;
    public char axiom;
    public float angle;
    public int generations;
    public float length;
    public Dictionary<char, string> rules;
}

// Class for the Stochastic L-Systems, where there are more than one rule per character
// A new class type is created to hold the value for the key in the dictionary
[System.Serializable]
public class StochasticTrees
{
    public int treeID;
    public char axiom;
    public float angle;
    public int generations;
    public float length;
    public Dictionary<char, StochasticRules> Stocrules;     // StochasticRules - Data type gives freedom to add two values per key

   
}

// The class having two string data types to hold the multiple rules of stochastic productions 
public class StochasticRules
{
    public string subRule1, subRule2;
}