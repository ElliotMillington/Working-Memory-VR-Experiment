using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UXF;

namespace WorkingMemory
{
    public class TwoDimensionalGroup : MonoBehaviour
    {
        public Texture[] possibleShapes; //Array of potential shape meshes
        public Color[] possibleColours;
        public string[] colorNames;

        bool[] isTarget; //Records whether the shape is a target
        bool[] isSelected; //Records which shapes have been selected

        public int targetNum;

        List<TwoDimensionalShape> optionShapes;
        List<TwoDimensionalShape> targetShapes;

        ArrayList targetIndexes = new ArrayList();
        ArrayList selectedIndexes = new ArrayList();

        public GameObject targetShapePrefab;
        public GameObject optionShapePrefab;

        [HideInInspector]
        public GameObject displayGridContainer;
        [HideInInspector]
        public GameObject targetGridContainer;
        [HideInInspector]
        public GameObject displayGrid; 
        [HideInInspector]
        public GameObject targetGrid; 

        
        public Trial currentTrial;

        private DateTime trialStartTime;
        private DateTime trialEndTime;
            

        public IEnumerator CreateShapes(Trial trial)
        {

            targetGrid = GameObject.FindGameObjectWithTag("targetGrid");
            targetGridContainer = GameObject.FindGameObjectWithTag("targetContainer");
            displayGrid = GameObject.FindGameObjectWithTag("displayGrid");
            displayGridContainer = GameObject.FindGameObjectWithTag("displayContainer");

            targetGrid.SetActive(false);
            targetGridContainer.SetActive(false);
            displayGrid.SetActive(false);
            displayGridContainer.SetActive(false);
        
            currentTrial = trial;

            yield return new WaitForSeconds(0.25f);

            int optionNum = trial.settings.GetInt("option_num");

            isTarget = new bool[optionNum];
            isSelected = new bool[optionNum];
            for (int i = 0; i < isSelected.Length; i++)
            {
                isTarget[i] = false;
                isSelected[i] = false;
            }

            optionShapes = new List<TwoDimensionalShape>();
            List<int[]> selectedCombo = new List<int[]>();


            for (int i = 0; i < optionNum; i++)
            {
                GameObject newShape = (GameObject) PrefabUtility.InstantiatePrefab(optionShapePrefab);
                newShape.transform.SetParent(displayGrid.transform, true);
                newShape.name = "option_shape" + i;
                optionShapes.Add(newShape.GetComponent<TwoDimensionalShape>());
                newShape.GetComponent<TwoDimensionalShape>().group = this;
                newShape.GetComponent<TwoDimensionalShape>().listPosition = i;

                //Set texture
                //First value is the texture, the second value is the colour
                int[] comboID = {UnityEngine.Random.Range(0, possibleShapes.Length), UnityEngine.Random.Range(0, possibleColours.Length)};
                //Ensures that no shape/colour combo is repeated
                while (selectedCombo.Contains(comboID))
                {
                    comboID = new int[]{UnityEngine.Random.Range(0, possibleShapes.Length), UnityEngine.Random.Range(0, possibleColours.Length)};
                    print("Duplicate detected");
                }
                selectedCombo.Add(comboID);

                Texture randomTexture = possibleShapes[comboID[0]];
                newShape.transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = randomTexture;

                //Set texture
                newShape.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = possibleColours[comboID[1]];
            }
            // set grid to be invisible
            displayGridContainer.SetActive(false);


            //Set up target shapes
            targetNum = trial.settings.GetInt("target_num");
            List<int> targetShapesIndex = HelperMethods.GenRandomInts(0, optionNum, targetNum);

            targetShapes = new List<TwoDimensionalShape>(targetNum);

            //Set up target shape objects
            for (int i = 0; i < targetNum; i++)
            {
                GameObject newShape = (GameObject) PrefabUtility.InstantiatePrefab(targetShapePrefab);
                newShape.transform.parent = targetGrid.transform;
                targetShapes.Add(newShape.GetComponent<TwoDimensionalShape>());
                newShape.GetComponent<TwoDimensionalShape>().group = this;

                int copyIndex = targetShapesIndex[i];
                newShape.GetComponent<TwoDimensionalShape>().listPosition = copyIndex;

                newShape.GetComponent<RawImage>().texture = optionShapes[copyIndex].transform.GetChild(0).gameObject.GetComponent<RawImage>().texture;
                newShape.GetComponent<RawImage>().color = optionShapes[copyIndex].transform.GetChild(0).gameObject.gameObject.GetComponent<RawImage>().color;

                isTarget[copyIndex] = true;

                targetIndexes.Add(copyIndex);
            }
            targetGridContainer.SetActive(true);

            yield return new WaitForSeconds(trial.settings.GetFloat("delay_time"));
            targetGridContainer.SetActive(false);


            displayGridContainer.SetActive(true);
            trialStartTime = System.DateTime.Now;

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

        public void Confirm()
        {
            //If not in trial, do nothing
            if (!currentTrial.session.InTrial) return;

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

            currentTrial.result["Total_Time_Milliseconds"] = (trialEndTime - trialStartTime).TotalMilliseconds;

            foreach (Transform child in targetGrid.transform) Destroy(child.gameObject);
            foreach (Transform child in displayGrid.transform) Destroy(child.gameObject);
            

            print("Trial took " + trialTime + " seconds. " + mistakes + " mistakes were made.");
            Session.instance.CurrentTrial.End();
            Session.instance.Invoke("BeginNextTrialSafe", 5);
        }

        public int getSelectedSize()
        {
            return selectedIndexes.Count;
        }

    }
}
