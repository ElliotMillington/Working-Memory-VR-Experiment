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
            case "Shapes_Colours":
                break;
            case "Visual_Search":
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
            case "Shapes_Colours":
                ShapeColourGroup shapeManager = gameObject.GetComponent<ShapeColourGroup>();
                StartCoroutine(shapeManager.CreateShapes(trial));
                break;
            case "Visual_Search":
                VisualSearchCleanUp(trial);
                break;
        }
    }

    public void TrialCleanUp(Trial trial)
    {
        Debug.Log("Trial Ended");
    }


    //Shape Colour block methods
    public void ShapeColourBlockSetUp(Block block)
    {
        
        
    }

    public void ShapeColourCleanUp(Trial trial)
    {

    }

    //Visual Search block methods
    public void VisualSearchBlockSetUp(Block block)
    {

    }

    public void VisualSearchTrialSetUp(Trial trial)
    {

    }

    public void VisualSearchCleanUp(Trial trial)
    {

    }
}
