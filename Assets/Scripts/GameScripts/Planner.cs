using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner : MonoBehaviour {


    GeneralPlan<string> WinRoot = new GeneralPlan<string>("Win");
    Stack<GeneralPlan<string>> DFS = new Stack<GeneralPlan<string>>();
    public List<string> decompose = new List<string>();
    // Use this for initialization
    void Start()
    {
        // Make a Tree structure
        WinRoot.AddChild("AvoidEnemy");
        WinRoot.AddChild("ScrewOver");
        WinRoot.AddChild("Move1");

        GeneralPlan<string> AvoidEnemy = WinRoot.GetChild(1);
        GeneralPlan<string> ScrewOver = WinRoot.GetChild(2);
        GeneralPlan<string> Move = WinRoot.GetChild(3);

        AvoidEnemy.AddChild("Method1");
        AvoidEnemy.AddChild("Method2");

        GeneralPlan<string> Method1 = AvoidEnemy.GetChild(1);
        GeneralPlan<string> Method2 = AvoidEnemy.GetChild(2);

        // Method1 Arc
        Method1.AddChild("Idle1");
        GeneralPlan<string> Idle = Method1.GetChild(1);

        Idle.AddChild("MoveToEnclave1");
        GeneralPlan<string> MoveToEnclave = Idle.GetChild(1);

        MoveToEnclave.AddChild("Idle2");


        //Method2 Arc

        Method2.AddChild("StepBack");
        GeneralPlan<string> stepBack = Method2.GetChild(1);
        stepBack.AddChild("MoveToEnclave2");
        GeneralPlan<string> MoveToEnclave1 = stepBack.GetChild(1);
        MoveToEnclave1.AddChild("Idle3");

        //Screwover
        ScrewOver.AddChild("Teleport");
        GeneralPlan<string> Teleport = ScrewOver.GetChild(1);

        Teleport.AddChild("Move2");
        transverse();
    }

    // returns me the last node of the graph
    public string transverse()
    {
        DFS.Push(WinRoot);
        GeneralPlan<string> S;
        string returnPlan = "";
        List<string> visited = new List<string>();
        while (DFS.Count > 0)
        {
            S = DFS.Peek();
            DFS.Pop();


            if (!visited.Contains(S.data) && !decompose.Contains(S.data))
            {
               // print(S.data);
                returnPlan = S.data;
                visited.Add(S.data);
            }


            // Add its children 
            if (S.children.Count > 0)
            {
                for (int i = 1; i <= S.children.Count; i++)
                {
                    DFS.Push(S.GetChild(i));
                }

            }


        }
        return returnPlan;
    }

    // If i reach the top, i restart the whole thing again.
    public void emptyDecomposition()
    {
        decompose.Clear();
    }
}
