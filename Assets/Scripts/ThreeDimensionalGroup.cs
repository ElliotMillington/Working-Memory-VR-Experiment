using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UXF;

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

        public ThreeDimensionalShape optionPrefab;
        public Button confirmButton;

        public Mesh[] possibleShapes; //Array of potential shape meshes
        public Material[] possibleColours; //Array of potential colours
        List<GameObject> targets; //Array of target objects
        List<GameObject> shapeOptions; //Array of shape option objects

        List<ThreeDimensionalShape> optionShapes;
        List<ThreeDimensionalShape> targetShapes;

        bool[] isTarget; //Records whether the shape is a target
        bool[] isSelected; //Records which shapes have been selected

        ArrayList targetIndexes = new ArrayList();
        ArrayList selectedIndexes = new ArrayList();

        private DateTime trialStartTime;
        private DateTime trialEndTime;

        private String option_string;

        private bool leftHand = false;
        private bool rightHand = false;

        public int targetNum; 

        private void Start()
        {
            targetStand = GameObject.Find("Stand");
            optionDisplay = GameObject.Find("Display");
            roomObj = GameObject.Find("CylinderRoom");
        }

        public IEnumerator CreateShapes(Trial trial)
        {
            yield return new WaitForSeconds(0.25f);

            int optionNum = trial.settings.GetInt("option_num");

            isTarget = new bool[optionNum];
            isSelected = new bool[optionNum];
            for (int i = 0; i < isSelected.Length; i++) {
                isTarget[i] = false;
                isSelected[i] = false;
            }

            //Generate option positions
            Vector3[] positions = new Vector3[optionNum];
            option_string = trial.settings.GetObject("option_distro").ToString();
            switch (option_string)
            {
                case "grid":
                    int nrow = Convert.ToInt32(Math.Ceiling(Mathf.Sqrt(optionNum)));
    
                    List<float> posStep = new List<float>(new float [] {0.25f,0f,-0.25f});
                    for (int i = 0; i < positions.Length; i++)
                    {
                        int row = i % nrow;
                        int column = i / nrow;
                        positions[i] = new Vector3(posStep[row], posStep[column], -0.1f);
                    }
                    break;
                case "circular":
                    for (var i = 0; i < positions.Length; i++)      
                    {
                        float radius = 0.008f;
                        var angle = i * Mathf.PI * 2 / positions.Length;
                        positions[i] = (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius) - new Vector3(0,0,-0.0035f);
                    }
                    break;
            };

            //Set up option shapes.
            optionShapes = new List<ThreeDimensionalShape>();
            List<int[]> selectedCombo = new List<int[]>();

            for (int i = 0; i < positions.Length; i++)
            {
                ThreeDimensionalShape newShape = Instantiate(optionPrefab);
                newShape.name = "option_shape" + i;
                optionShapes.Add(newShape);
                newShape.group = this;
                newShape.listPosition = i;

                //Set transform properties
                switch (option_string)
                {
                    case "grid":
                        newShape.transform.parent = optionDisplay.transform;
                        newShape.transform.localScale = new Vector3(100, 100, 100);
                        break;
                    case "circular":
                        newShape.transform.parent = roomObj.transform;
                        newShape.transform.localScale = new Vector3(1, 1, 1);
                        break;
                }
                newShape.transform.localPosition = positions[i];
                newShape.transform.Rotate(new Vector3(0, UnityEngine.Random.Range(-180, 180), 0));

                //Set mesh
                //First value is the mesh, the second value is the material
                int[] comboID = {UnityEngine.Random.Range(0, possibleShapes.Length), UnityEngine.Random.Range(0, possibleColours.Length)};
                //Ensures that no shape/colour combo is repeated
                while (selectedCombo.Contains(comboID))
                {
                    comboID = new int[]{UnityEngine.Random.Range(0, possibleShapes.Length), UnityEngine.Random.Range(0, possibleColours.Length)};
                    print("Duplicate detected");
                }
                selectedCombo.Add(comboID);

                Mesh randomMesh = possibleShapes[comboID[0]];
                newShape.GetComponent<MeshFilter>().mesh = randomMesh;
                newShape.GetComponent<MeshCollider>().sharedMesh = randomMesh;

                //Set material
                newShape.GetComponent<Renderer>().material = possibleColours[comboID[1]];

                //Hide shape until trial start
                newShape.clickable = true;
                newShape.gameObject.SetActive(false);
            }

            //Set up target shapes
            targetNum = trial.settings.GetInt("target_num");
            List<int> targetShapesIndex = HelperMethods.GenRandomInts(0, optionNum, targetNum);
            zPos = HelperMethods.Seq(targetNum, -targetZRange, targetZRange);

            targetShapes = new List<ThreeDimensionalShape>(targetNum);

            //Set up target shape objects
            for (int i = 0; i < targetNum; i++)
            {
                ThreeDimensionalShape newShape = Instantiate(optionPrefab);
                targetShapes.Add(newShape);
                newShape.group = this;

                int copyIndex = targetShapesIndex[i];
                newShape.listPosition = copyIndex;

                //Set transform properties
                newShape.transform.parent = targetStand.transform;
                newShape.transform.localPosition = new Vector3(0, 1.2f, zPos[i]);
                newShape.transform.localScale = new Vector3(100, 100, 100);

                //Set mesh
                newShape.GetComponent<MeshFilter>().mesh = optionShapes[copyIndex].GetComponent<MeshFilter>().mesh;
                newShape.GetComponent<Renderer>().material = optionShapes[copyIndex].GetComponent<Renderer>().material;

                newShape.clickable = false;
                isTarget[copyIndex] = true;

                targetIndexes.Add(copyIndex);
            }
            targetStand.SetActive(true);

            yield return new WaitForSeconds(trial.settings.GetFloat("delay_time"));

            foreach (ThreeDimensionalShape shape in targetShapes) shape.gameObject.SetActive(false);
            if (option_string != "circular") targetStand.SetActive(false);
            foreach (ThreeDimensionalShape shape in optionShapes)
            {
                shape.gameObject.SetActive(true);
            }

            //Start timing the trial
            trialStartTime = System.DateTime.Now;
            //Show confirm button
            //confirmButton.gameObject.SetActive(true);

            Debug.Log("Target Shapes: " + String.Join(" ", targetIndexes.ToArray()));
        }

        public void RegisterSelect(int index, bool selected)
        {
            isSelected[index] = selected;
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
            for (int i = 0; i < isSelected.Length; i++)
            {
                if (isSelected[i] != isTarget[i])
                {
                    mistakes++;
                }
            }

            Trial trial = Session.instance.CurrentTrial;
            trial.result["Total_Time_Milliseconds"] = (trialEndTime - trialStartTime).TotalMilliseconds;

            //confirmButton.gameObject.SetActive(false);
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
