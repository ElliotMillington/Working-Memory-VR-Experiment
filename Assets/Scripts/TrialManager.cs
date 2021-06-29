using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;
using WorkingMemory;

public class TrialManager : MonoBehaviour
{
    //Shape Colour Variables
    public Mesh[] possibleShapes; //Array of potential shape meshes
    public Material[] possibleColours; //Array of potential colours
    public List<GameObject> targets; //Array of target objects
    public List<GameObject> shapeOptions; //Array of shape option objects

    public void BlockSetUp(Block block)
    {
        switch (block.settings.GetString("scene_name"))
        {
            case "Shapes_Colour":
                ShapeColourBlockSetUp(block);
                break;
            case "Visual_Search":
                VisualSearchBlockSetUp(block);
                break;
        }
    }

    public void TrialSetUp(Trial trial)
    {

    }

    public void ShapeColourBlockSetUp(Block block)
    {
        targets = GameObject.Find("Stand").GetChildren();
        shapeOptions = GameObject.Find("Board").GetChildren();

        foreach (GameObject shape in targets)
        {
            shape.GetComponent<Shape>().clickable = false;
        }
    }

    public void ShapeColourTrialSetUp(Trial trial)
    {
        //Randomising shape and colour option
        foreach (GameObject go in shapeOptions)
        {
            go.GetComponent<MeshFilter>().mesh = possibleShapes[Random.Range(0, possibleShapes.Length)];
            go.GetComponent<Renderer>().material = possibleColours[Random.Range(0, possibleColours.Length)];
            go.transform.Rotate(new Vector3(Random.Range(-180, 180), 0, 0));
            print(go.name);
        }

        //Selecting and modifying target shapes
        List<int> keyShapesIndices = HelperMethods.GenRandomInts(0, shapeOptions.Count - 1, targets.Count);
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].GetComponent<MeshFilter>().mesh = shapeOptions[keyShapesIndices[i]].GetComponent<MeshFilter>().mesh;
            targets[i].GetComponent<Renderer>().material = shapeOptions[keyShapesIndices[i]].GetComponent<Renderer>().material;
        }
    }

    public void VisualSearchBlockSetUp(Block block)
    {

    }

    public void VisualSearchTrialSetUp(Trial trial)
    {

    }
}
