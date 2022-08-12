using UnityEngine;
using UXF;
using UnityEngine.SceneManagement;

public class TrialManager : MonoBehaviour
{

    
    void Awake()
    {
        //This means that the object will persist between scenes.
        DontDestroyOnLoad(gameObject);
    }
    
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


    public void exitGame()
    {
        Application.Quit();
    }

}
