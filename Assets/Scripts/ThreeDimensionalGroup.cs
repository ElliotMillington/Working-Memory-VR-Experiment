using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UXF;
using UnityEditor;

namespace WorkingMemory
{
    public class ThreeDimensionalGroup : MonoBehaviour
    {
        //Shape Colour Variables
        GameObject targetStand;
        float targetZRange = 0.5f;

        float[] zPos;

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

            /*
            //Generate option positions
            Vector3[] positions = new Vector3[optionNum];
            switch (option_string)
            {
                case "grid":
                    int nrow = Convert.ToInt32(Math.Ceiling(Mathf.Sqrt(optionNum)));
    
                    List<float> posStep = new List<float>(new float [] {0.25f,0f,-0.25f});
                    for (int x = 0; x < positions.Length; x++)
                    {
                        int row = x % nrow;
                        int column = x / nrow;
                        positions[x] = new Vector3(posStep[row], posStep[column], -0.1f);
                    }
                    break;
                case "circular":
                    for (var y = 0; y < positions.Length; y++)      
                    {
                        float radius = 0.008f;
                        var angle = y * Mathf.PI * 2 / positions.Length;
                        positions[y] = (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius) - new Vector3(0,0,-0.0035f);
                    }
                    break;
            };
            */


            //Set up option shapes.
            optionShapes = new List<ThreeDimensionalShape>();
            List<(Mesh, Material)> selectedCombinations = new List<(Mesh, Material)>();

            int i = 0;
            while (optionShapes.Count < optionNum)
            {
                //new shape created, renamed and placed into list for grouping
                
                GameObject newDisplayObj = (GameObject) PrefabUtility.InstantiatePrefab(displayGridPrefab, displayGrid.transform);
                newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().name = "option_shape" + i;
                optionShapes.Add(newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>());
                newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().group = this;
                newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().listPosition = i;

                if (display_random && option_string == "grid")
                {
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().gameObject.transform.Rotate(new Vector3(UnityEngine.Random.Range(360f, 0f), 0f , 0f), Space.World);   
                }
            

                
                
          

                //Set transform properties
                // TODO: remove/edit as part of placement

                /*
                switch (option_string)
                {
                    case "grid":
                        newShape.transform.SetParent(optionDisplay.transform, true);
                        newShape.transform.localScale = new Vector3(100, 100, 100);
                        break;
                    case "circular":
                        newShape.transform.SetParent(roomObj.transform, true);
                        newShape.transform.localScale = new Vector3(1, 1, 1);
                        break;
                }
                
                newShape.transform.localPosition = positions[i];
                newShape.transform.Rotate(new Vector3(0, UnityEngine.Random.Range(-180, 180), 0));

                */


                // pop random combination from the list and add to selected List for later
                int removeIndex = UnityEngine.Random.Range(0, possibleCombinations.Count);
                (Mesh, Material) combo = possibleCombinations[removeIndex];
                possibleCombinations.RemoveAt(removeIndex);
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

                i++;
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

            //save another way 
            trial.settings.SetValue("total_time", total_time);
            trial.settings.SetValue("target_shapes", target_shapes);
            trial.settings.SetValue("selected_shapes", selected_shapes);
            trial.settings.SetValue("correct_shapes", correct_shapes);
            trial.settings.SetValue("incorrect_shapes", incorrect_shapes);  

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

}
