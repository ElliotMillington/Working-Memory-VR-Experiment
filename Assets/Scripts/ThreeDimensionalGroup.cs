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

        GameObject targetGrid;

        GameObject displayGrid;

        public ThreeDimensionalShape optionPrefab;

        public GameObject displayGridPrefab;

        //passed into from the inspector (all meshes and materials)
        public Mesh[] possibleShapes; //Array of potential shape meshes
        public Material[] possibleColours; //Array of potential colours


        List<ThreeDimensionalShape> optionShapes;
        List<ThreeDimensionalShape> targetShapes;


        // passed for the trial from the panel
        List<Mesh> selectedMeshes;
        List<Material> selectedMaterials;

        public List<int> selectedIndexes;
        public List<int> targetIndexes;

        private DateTime trialStartTime;
        private DateTime trialEndTime;

        private String option_string;

        private bool leftHand = false;
        private bool rightHand = false;

        public int targetNum; 

        public bool startWaitToggle;

        public bool getWaitBool()
        {
            return startWaitToggle;
        }

        public IEnumerator CreateShapes(Trial trial)
        {
            yield return new WaitForSeconds(0.25f);

            //TODO: if trial number is 1 then need to yield return new WaitUntil(getWaitBool);
            Debug.Log(trial.number);

            targetStand = GameObject.FindGameObjectWithTag("stand");
            optionDisplay = GameObject.FindGameObjectWithTag("display");
            roomObj = GameObject.FindGameObjectWithTag("room");

            targetGrid = GameObject.FindGameObjectWithTag("targetGrid");
            displayGrid = GameObject.FindGameObjectWithTag("displayGrid");

            int optionNum = trial.settings.GetInt("option_num");


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

            
            //Generate option positions
            Vector3[] positions = new Vector3[optionNum];
            option_string = trial.settings.GetObject("option_distro").ToString().ToLower();
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


            //Set up option shapes.
            optionShapes = new List<ThreeDimensionalShape>();
            List<(Mesh, Material)> selectedCombinations = new List<(Mesh, Material)>();

            int i = 0;
            while (optionShapes.Count < optionNum)
            {
                //new shape created, renamed and placed into list for grouping
                ThreeDimensionalShape newShape = Instantiate(optionPrefab);
                newShape.name = "option_shape" + i;
                optionShapes.Add(newShape);
                newShape.group = this;
                newShape.listPosition = i;



                //Set transform properties
                // TODO: remove/edit as part of placement
                switch (option_string)
                {
                    case "grid":
                        newShape.transform.SetParent(optionDisplay.transform, true);
                        newShape.transform.localScale = new Vector3(100, 100, 100);
                        break;
                    case "circular":
                        newShape.transform.SetParent(roomObj.transform, true);
                        Debug.Log(newShape.transform.parent.name);
                        newShape.transform.localScale = new Vector3(1, 1, 1);
                        break;
                }
                newShape.transform.localPosition = positions[i];
                newShape.transform.Rotate(new Vector3(0, UnityEngine.Random.Range(-180, 180), 0));


                // pop random combination from the list and add to selected List for later
                int removeIndex = UnityEngine.Random.Range(0, possibleCombinations.Count);
                (Mesh, Material) combo = possibleCombinations[removeIndex];
                possibleCombinations.RemoveAt(removeIndex);
                newShape.meshMaterialCombo = combo;

                // add the removed options to add to display later
                selectedCombinations.Add(combo);

                //assign mesh and material to newShape
                newShape.GetComponent<MeshFilter>().mesh = combo.Item1;
                newShape.GetComponent<MeshCollider>().sharedMesh = combo.Item1;

                //Set material
                newShape.GetComponent<Renderer>().material = combo.Item2;

                
                //Hide shape until trial start
                newShape.clickable = true;
                newShape.gameObject.SetActive(false);
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
                    GameObject newDisplayObj = (GameObject) PrefabUtility.InstantiatePrefab(displayGridPrefab, targetGrid.transform);
                    targetShapes.Add(newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>());
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().group = this;

                    //save its mesh and material, and index
                    (Mesh, Material) targetCombo  = optionShapes[possibleTargetIndex].meshMaterialCombo;
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().listPosition = possibleTargetIndex;

                    //Set mesh
                    newDisplayObj.GetComponentInChildren<MeshFilter>().mesh = targetCombo.Item1;
                    newDisplayObj.GetComponentInChildren<Renderer>().material = targetCombo.Item2;

                    //set a random rotation for the child shape
                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().setRandomRotation();

                    newDisplayObj.GetComponentInChildren<ThreeDimensionalShape>().clickable = false;
                }
            }

            targetStand.SetActive(true);

            yield return new WaitForSeconds(trial.settings.GetFloat("delay_time"));

            foreach (ThreeDimensionalShape shape in targetShapes) shape.gameObject.SetActive(false);
            foreach (ThreeDimensionalShape shape in optionShapes)
            {
                shape.gameObject.SetActive(true);
            }

            //Start timing the trial
            trialStartTime = System.DateTime.Now;
            //Show confirm button
        }

        public void RegisterSelect(int index, bool selected)
        {
            Debug.Log("Shape " + index + " was " + (selected==true? "selected.": "deselected."));

            if (selected==true)
            {
                selectedIndexes.Add(index);
            }
            else
            {
                selectedIndexes.Remove(index);
            }
        }

        public int getSelectedSize()
        {
            return selectedIndexes.Count;
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

            trialEndTime = System.DateTime.Now;
            double trialTime = (trialEndTime - trialStartTime).TotalSeconds;

            Debug.Log("Selected shapes at end: " + String.Join(" ", selectedIndexes.ToArray()));

            //reset 
            selectedIndexes.Clear();
            targetIndexes.Clear();
            

            int mistakes = 0;
            foreach (ThreeDimensionalShape shape in optionShapes)
            {
                //if selected but not target
                if (shape.selected && !shape.isTarget)
                {
                    mistakes++;
                }
            }

            Trial trial = Session.instance.CurrentTrial;
            trial.result["Total_Time_Milliseconds"] = (trialEndTime - trialStartTime).TotalMilliseconds;

            foreach (Transform child in targetStand.transform) Destroy(child.gameObject);
            if (option_string != "grid")
            {
                foreach (Transform child in roomObj.transform) if (child.name != "ConfirmationPlanes") Destroy(child.gameObject);
            }else
            {
                foreach (Transform child in optionDisplay.transform) Destroy(child.gameObject);
            }

            print("Trial took " + trialTime + " seconds. " + mistakes + " mistakes were made.");
            Session.instance.CurrentTrial.End();
            Session.instance.Invoke("BeginNextTrialSafe", 5);
        }
    }

}
