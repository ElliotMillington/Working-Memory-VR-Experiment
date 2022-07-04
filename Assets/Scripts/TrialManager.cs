using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;
using WorkingMemory;

public class TrialManager : MonoBehaviour
{
    public void BlockSetUp(Block block)
    {
        switch (block.settings.GetString("scene_type"))
        {
            case "Three_Dimensional":
                break;
            case "Two_Dimensional":
                break;
        }
    }

    //Select trial set up based on block type
    public void TrialSetUp(Trial trial)
    {
        StopAllCoroutines();
        print("Started trial");

        switch (trial.block.settings.GetString("scene_type"))
        {
            case "Three_Dimensional":
                ThreeDimensionalGroup threeShapeManager = gameObject.GetComponent<ThreeDimensionalGroup>();
                StartCoroutine(threeShapeManager.CreateShapes(trial));
                break;
            case "Two_Dimensional":
                TwoDimensionalGroup twoShapeManager = gameObject.GetComponent<TwoDimensionalGroup>();
                StartCoroutine(twoShapeManager.CreateShapes(trial));
                break;
        }
    }

    public void TrialCleanUp(Trial trial)
    {
        Debug.Log("Trial Ended");
    }


    //Shape Colour block methods
    public void ThreeDimensionalBlockSetUp(Block block)
    {
        
        
    }

    public void ThreeDimensionalCleanUp(Trial trial)
    {

    }

    //Visual Search block methods
    public void TwoDimensionalBlockSetUp(Block block)
    {

    }

    public void TwoDimensionalTrialSetUp(Trial trial)
    {

    }

    public void TwoDimensionalCleanUp(Trial trial)
    {

    }
}
