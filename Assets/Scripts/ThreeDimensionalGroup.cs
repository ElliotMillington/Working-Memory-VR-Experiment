using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UXF;
using UnityEditor;


public class ThreeDimensionalGroup : MonoBehaviour
{

    float MAXIMUM_CIRCULAR_HEIGHT = -0.004f;
    float MINIMUM_CIRCULAR_HEIGHT = 0.006f;

    float RADIUS = 0.008f;

    //Shape Colour Variables
    GameObject targetStand;
    GameObject optionDisplay;

    GameObject roomObj;

    GameObject confirmationObj;

    public Material confirmationPlaneDefault;
    public Material confirmationPlaneReady;


    GameObject targetGrid;

    GameObject displayGrid;

    public ThreeDimensionalShape optionPrefab;


    

    public GameObject targetGridPrefab;

    public GameObject displayGridPrefab;

    //passed into from the inspector (all meshes and materials)
    public Mesh[] possibleShapes; //Array of potential shape meshes
    public Material[] possibleColours; //Array of potential colours


    List<ThreeDimensionalShape> optionShapes;
    List<ThreeDimensionalShape> targetShapes;


    // passed for the trial from the panel
    List<Mesh> selectedMeshes;
    List<Material> selectedMaterials;
    
    public List<ThreeDimensionalShape> selectedShapes;

    public List<int> selectedIndexes;

    private DateTime trialStartTime;
    private DateTime trialEndTime;

    private String option_string;

    private bool leftHand = false;
    private bool rightHand = false;

    public bool confirm_start;
    private bool display_random;
    private bool target_random;

    

    public int targetNum; 

    public bool startWaitToggle;

    public bool getWaitBool()
    {
        return startWaitToggle;
    }

    public IEnumerator CreateShapes(Trial trial)
    {
        yield return new WaitForSeconds(0.25f);

        confirm_start = trial.settings.GetBool("confirm_start");

        targetStand = GameObject.FindGameObjectWithTag("stand");
        optionDisplay = GameObject.FindGameObjectWithTag("display");
        roomObj = GameObject.FindGameObjectWithTag("room");
        confirmationObj = GameObject.FindGameObjectWithTag("confirmation_plane");

        targetGrid = GameObject.FindGameObjectWithTag("targetGrid");
        displayGrid = GameObject.FindGameObjectWithTag("displayGrid");
        GameObject startButton = GameObject.FindGameObjectWithTag("startButton");
        GameObject startText = GameObject.FindGameObjectWithTag("textHolder");

        int optionNum = trial.settings.GetInt("option_num");
        option_string = trial.settings.GetObject("option_distro").ToString().ToLower();
        display_random = trial.settings.GetBool("display_random");
        target_random = trial.settings.GetBool("target_random");

        if (confirm_start && trial.numberInBlock == 1)
        {
            yield return new WaitUntil(getWaitBool);
            startText.SetActive(false);
            startButton.SetActive(false);
        }
        else{
            if (startButton != null) startButton.SetActive(false);
            if (startText != null) startText.SetActive(false);
        }

        //meshes and materials passed by the user
        selectedMeshes = (List<Mesh>)trial.settings.GetObject("selected_meshes");
        selectedMaterials = (List<Material>)trial.settings.GetObject("selected_materials");

        List <(Mesh, Material)> possibleCombinations = new List <(Mesh,Material)>();
        foreach(Mesh meshItem in selectedMeshes)
        {
            foreach(Material materialItem in selectedMaterials)
            {
                // will hold all possible tuples of mesh and material
                possibleCombinations.Add((meshItem, materialItem));
            }
        }

        //Set up option shapes.
        optionShapes = new List<ThreeDimensionalShape>();
        List<(Mesh, Material)> selectedCombinations = new List<(Mesh, Material)>();
        int optionNumIndex = 0;

        switch (option_string)
        {
            case "grid":

                while (optionShapes.Count < optionNum)
                {
                    //new shape created, renamed and placed into list for grouping
                    
                    GameObject newDisplayObj = (GameObject) PrefabUtility.InstantiatePrefab(displayGridPrefab, displayGrid.transform);
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().name = "option_shape" + optionNumIndex;
                    optionShapes.Add(newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>());
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().group = this;
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().listPosition = optionNumIndex;

                    if (display_random)
                    {
                        newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().gameObject.transform.Rotate(new Vector3(UnityEngine.Random.Range(360f, 0f), 0f , 0f), Space.World);   
                    }
                
                    // pop random combination from the list and add to selected List for later
                    int combinationIndex = UnityEngine.Random.Range(0, possibleCombinations.Count);
                    (Mesh, Material) combo = possibleCombinations[combinationIndex];
                    possibleCombinations.RemoveAt(combinationIndex);
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().meshMaterialCombo = combo;

                    // add the removed options to add to display later
                    selectedCombinations.Add(combo);

                    //assign mesh and material to newShape
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().GetComponent<MeshFilter>().mesh = combo.Item1;
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().GetComponent<MeshCollider>().sharedMesh = combo.Item1;

                    //Set material
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().GetComponent<Renderer>().material = combo.Item2;
                    
                    //Hide shape until trial start
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().clickable = true;
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().gameObject.SetActive(false);

                    optionNumIndex++;
                }
                break;

            case "circular":

                List<(Vector3, Vector3)> positionsAndRotation = new List<(Vector3, Vector3)>();
                int SQRT_OPTION = (int) Mathf.Sqrt((float)optionNum);

                // gnerate heights of each ring
                List<double> heights = new List<double>();
                for (var heightIndex = 0; heightIndex < SQRT_OPTION; heightIndex++)
                {
                    float fraction = (heightIndex * 1.0f)/((SQRT_OPTION * 1.0f)-1);
                    double value = Mathf.Lerp(MINIMUM_CIRCULAR_HEIGHT, MAXIMUM_CIRCULAR_HEIGHT, fraction);
                    heights.Add(value);
                }

                //for each layer
                foreach(float height in heights)
                {
                    bool isAlternate = heights.IndexOf(height) % 2 == 0;
                    // for each shape in layer
                    for (var y = 0; y < SQRT_OPTION; y++)      
                    {
                        var angle = (isAlternate ? y * Mathf.PI * 2 / SQRT_OPTION : (y * Mathf.PI * 2 / SQRT_OPTION) + (360/SQRT_OPTION*2));

                        // TODO: make shape face the center
                        Vector3 rotation = new Vector3(0,0,0);
                        if (display_random)
                        {
                            rotation = new Vector3(UnityEngine.Random.Range(-180, 180), UnityEngine.Random.Range(-180, 180), UnityEngine.Random.Range(-180, 180));
                        }else{
                            if (Mathf.Sin(angle) > 0 && Mathf.Cos(angle) < 0){
                                rotation = new Vector3(360 * Mathf.Sin(angle) + angle,0,0);
                            }else if (Mathf.Sin(angle) > 0 && Mathf.Cos(angle) > 0) {
                                rotation = new Vector3(0,0,0);
                            }else if (Mathf.Sin(angle) < 0 && Mathf.Cos(angle) < 0) {
                                rotation = new Vector3(0,0,0);
                            }else if (Mathf.Sin(angle) < 0 && Mathf.Cos(angle) > 0) {
                                rotation = new Vector3(0,0,0);
                            }
                        }
                        
                        Vector3 position = (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * RADIUS) - new Vector3(0,0, height);

                        positionsAndRotation.Add((position, rotation));
                    }
                }

                // create shapes
                while (optionShapes.Count < optionNum)
                {
                    ThreeDimensionalShape newDisplayObj = Instantiate(optionPrefab);
                    newDisplayObj.transform.SetParent(roomObj.transform, true);

                    newDisplayObj.transform.localScale = new Vector3(1, 1, 1);
                    newDisplayObj.name = "option_shape" + optionNumIndex;
                    optionShapes.Add(newDisplayObj);
                    newDisplayObj.group = this;
                    newDisplayObj.listPosition = optionNumIndex;

                    // pop random combination from the list and add to selected List for later
                    int combinationIndex = UnityEngine.Random.Range(0, possibleCombinations.Count);
                    (Mesh, Material) combo = possibleCombinations[combinationIndex];
                    possibleCombinations.RemoveAt(combinationIndex);
                    newDisplayObj.meshMaterialCombo = combo;

                    //TODO: Set positioning
                    int positionsAndRotationIndex = UnityEngine.Random.Range(0, positionsAndRotation.Count);
                    (Vector3, Vector3) positionAndRotation = positionsAndRotation[positionsAndRotationIndex];
                    positionsAndRotation.RemoveAt(positionsAndRotationIndex);

                    newDisplayObj.transform.localPosition = positionAndRotation.Item1;
                    newDisplayObj.transform.Rotate(positionAndRotation.Item2);


                    // add the removed options to add to display later
                    selectedCombinations.Add(combo);

                    //assign mesh and material to newShape
                    newDisplayObj.GetComponent<MeshFilter>().mesh = combo.Item1;
                    newDisplayObj.GetComponent<MeshCollider>().sharedMesh = combo.Item1;

                    //Set material
                    newDisplayObj.GetComponent<Renderer>().material = combo.Item2;
                    
                    //Hide shape until trial start
                    newDisplayObj.clickable = true;
                    newDisplayObj.gameObject.SetActive(false);

                    optionNumIndex++;


                }

                break;

            default:
                Debug.LogError(option_string + " is not a recognised shape layout.");
                break;
        }   
        


        //Set up target shapes
        targetNum = trial.settings.GetInt("target_num");
        targetShapes = new List<ThreeDimensionalShape>();

        //Set up target shape objects
        while (targetShapes.Count < targetNum)
        {

            // chose a random shape in options to become target (i.e isTarget == true)
            int possibleTargetIndex = UnityEngine.Random.Range(0, optionShapes.Count);
            if (!optionShapes[possibleTargetIndex].isTarget)
            {
                //make the option shape a target
                optionShapes[possibleTargetIndex].isTarget = true;

                //create display shape
                GameObject newTargetObj = (GameObject) PrefabUtility.InstantiatePrefab(targetGridPrefab, targetGrid.transform);
                targetShapes.Add(newTargetObj.GetComponentInChildren<ThreeDimensionalShape>());
                newTargetObj.GetComponentInChildren<ThreeDimensionalShape>().group = this;

                //save its mesh and material, and index
                (Mesh, Material) targetCombo  = optionShapes[possibleTargetIndex].meshMaterialCombo;
                newTargetObj.GetComponentInChildren<ThreeDimensionalShape>().listPosition = possibleTargetIndex;

                //Set mesh
                newTargetObj.GetComponentInChildren<MeshFilter>().mesh = targetCombo.Item1;
                newTargetObj.GetComponentInChildren<Renderer>().material = targetCombo.Item2;

                //set a random rotation for the child shape
                if (target_random) newTargetObj.GetComponentInChildren<ThreeDimensionalShape>().setRandomRotation();

                newTargetObj.GetComponentInChildren<ThreeDimensionalShape>().clickable = false;
            }
        }






        targetStand.SetActive(true);

        yield return new WaitForSeconds(trial.settings.GetFloat("shape_display_time"));

        foreach (ThreeDimensionalShape shape in targetShapes) shape.gameObject.SetActive(false);

        //make time in between taking away target and the display of the shapes
        yield return new WaitForSeconds(trial.settings.GetFloat("delay_time"));

        foreach (ThreeDimensionalShape shape in optionShapes) shape.gameObject.SetActive(true);
        
        //Start timing the trial
        trialStartTime = System.DateTime.Now;
        //Show confirm button
    }

    public void RegisterSelect(ThreeDimensionalShape shape, int index, bool selected)
    {
        if (selected==true)
        {
            selectedIndexes.Add(index);
            selectedShapes.Add(shape);
        }
        else
        {
            selectedIndexes.Remove(index);
            selectedShapes.Remove(shape);
        }

        //make ceiling green
        if (selectedIndexes.Count == targetNum)
        {
            //make green
            confirmationObj.GetComponentInChildren<Renderer>().material = confirmationPlaneReady;
        }else
        {
            //else make grey
            confirmationObj.GetComponentInChildren<Renderer>().material = confirmationPlaneDefault;
        }
    }

    public int getSelectedSize()
    {
        return selectedShapes.Count;
    }

    public void invertHandedness(String handedness)
    {
        if (handedness == "right")
        {
            leftHand = !leftHand;
        }
        else
        {
            rightHand = !rightHand;
        }
    }

    public void Confirm()
    {
        //If not in trial, do nothing
        if (!Session.instance.InTrial) return;

        //need to have the right number to confirm
        if(getSelectedSize() != targetNum) return;

        trialEndTime = System.DateTime.Now;
        double trialTime = (trialEndTime - trialStartTime).TotalSeconds;
    
        List<ThreeDimensionalShape> wronglySelected = new List<ThreeDimensionalShape>();
        List<ThreeDimensionalShape> correctlySelected = new List<ThreeDimensionalShape>();
        foreach (ThreeDimensionalShape shape in selectedShapes)
        {
            //if selected but not target
            if (shape.isTarget)
            {
                correctlySelected.Add(shape);
            }else{
                wronglySelected.Add(shape);
            }
        }

        Trial trial = Session.instance.CurrentTrial;

        //Store one way
        double total_time = (trialEndTime - trialStartTime).TotalMilliseconds;
        trial.result["Total_User_Time_Milliseconds"] = total_time;

        string target_shapes = shapesToString(targetShapes);
        trial.result["Target_Shapes"] = target_shapes;

        string selected_shapes = shapesToString(selectedShapes);
        trial.result["Participant_Selected_Shapes"] = selected_shapes;

        string correct_shapes = shapesToString(correctlySelected);
        trial.result["Correctly_Selected_Shapes"] = correct_shapes;

        string incorrect_shapes = shapesToString(wronglySelected);
        trial.result["Incorrectly_Selected_Shapes"] = incorrect_shapes;

        trial.result["All_Correct"] =  (wronglySelected.Count == 0);

        //save another way 
        trial.settings.SetValue("total_time", total_time);
        trial.settings.SetValue("target_shapes", target_shapes);
        trial.settings.SetValue("selected_shapes", selected_shapes);
        trial.settings.SetValue("correct_shapes", correct_shapes);
        trial.settings.SetValue("incorrect_shapes", incorrect_shapes);
        trial.settings.SetValue("all_correct", wronglySelected.Count==0);

        trial.settings.SetValue("dimension", 3);     
        trial.settings.SetValue("layout", option_string);  



        foreach (Transform child in targetGrid.transform) Destroy(child.gameObject);

        //TODO: Need to change this for circular
        if (option_string != "grid")
        {
            foreach (Transform child in roomObj.transform) if (child.name != "ConfirmationPlanes") Destroy(child.gameObject);
        }else
        {
            foreach (Transform child in displayGrid.transform) Destroy(child.gameObject);
        }

        //reset 
        selectedIndexes.Clear();
        selectedShapes.Clear();

        int numberOfBlocks = Session.instance.blocks.Count;
        //if last trial in the block
        if (Session.instance.CurrentTrial == Session.instance.CurrentBlock.lastTrial && Session.instance.blocks[numberOfBlocks-1] == Session.instance.CurrentBlock)
        {
            //if last trial in the block and this is the last block
            Session.instance.CurrentTrial.End();

            //move to GUI Scene
            Session.instance.End();
            GameObject.Find("SessionManager").GetComponent<SessionManager>().moveToGUI(GameObject.Find("TrialManager"), GameObject.Find("[UXF_Rig]"));
        }else{
            //else there are more trials in the block to work through
            Session.instance.CurrentTrial.End();
            Session.instance.Invoke("BeginNextTrialSafe", 5);
        }
    }

    public string shapesToString(List<ThreeDimensionalShape> shapes)
    {
        List<string> strings = new List<string>();
        foreach (ThreeDimensionalShape shape in shapes)
        {
            char[] whole_shape_name = shape.meshMaterialCombo.Item1.name.ToCharArray();
            char first_letter_shape = whole_shape_name[0].ToString().ToUpper()[0];
            whole_shape_name[0] = first_letter_shape;

            char[] whole_colour_name = shape.meshMaterialCombo.Item2.name.ToCharArray();
            char first_letter_colour = whole_colour_name[0].ToString().ToUpper()[0];
            whole_colour_name[0] = first_letter_colour;

            string colourString = new string(whole_colour_name);
            string shapeString = new string(whole_shape_name);

            string newString = colourString + " " + shapeString;
            strings.Add(newString); 
        }

        return String.Join(" | ", strings);
    }

}


