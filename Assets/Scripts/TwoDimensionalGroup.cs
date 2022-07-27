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

        bool[] targetList; //Records whether the shape is a target
        bool[] selectedList; //Records which shapes have been selected

        public int targetNum;

        List<TwoDimensionalShape> optionShapes;
        List<TwoDimensionalShape> targetShapes;

        List<Texture> selectedTextures;
        List<Color> selectedColours;

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

        [HideInInspector]
        public GameObject coverObj;

        [HideInInspector]
        public GameObject startButton;

        [HideInInspector]
        public GameObject confirmButton;

        private DateTime trialStartTime;
        private DateTime trialEndTime;

        private bool confirm_start;

        private bool startWaitToggle = false;
            

        public IEnumerator CreateShapes(Trial trial)
        {
            coverObj = GameObject.Find("coverPanel");
            targetGrid = GameObject.FindGameObjectWithTag("targetGrid");
            targetGridContainer = GameObject.FindGameObjectWithTag("targetContainer");
            displayGrid = GameObject.FindGameObjectWithTag("displayGrid");
            displayGridContainer = GameObject.FindGameObjectWithTag("displayContainer");
            startButton = GameObject.FindGameObjectWithTag("startButton");
            confirmButton = GameObject.FindGameObjectWithTag("confirmButton");

            coverObj.SetActive(false);
            targetGridContainer.SetActive(false);
            displayGridContainer.SetActive(false);  
            confirmButton.SetActive(false); 

            confirm_start = trial.settings.GetBool("confirm_start");
            int optionNum = trial.settings.GetInt("option_num");

            if (confirm_start && trial.numberInBlock == 1)
            {
                startButton.GetComponent<Button>().onClick.AddListener(delegate{toggleWait();});
                yield return new WaitUntil(getWaitBool);
            }
            startButton.SetActive(false);

            confirmButton.GetComponent<Button>().onClick.AddListener(delegate{Confirm();});

            //meshes and materials passed by the user
            selectedTextures = (List<Texture>)trial.settings.GetObject("selected_textures");
            selectedColours = (List<Color>)trial.settings.GetObject("selected_colours");

            List <(Texture, Color)> possibleCombinations = new List <(Texture,Color)>();
            foreach(Texture textureItem in selectedTextures)
            {
                foreach(Color colourItem in selectedColours)
                {
                    // will hold all possible tuples of mesh and material
                    possibleCombinations.Add((textureItem, colourItem));
                }
            }

            targetList = new bool[optionNum];
            selectedList = new bool[optionNum];
            for (int i = 0; i < selectedList.Length; i++)
            {
                targetList[i] = false;
                selectedList[i] = false;
            }

            optionShapes = new List<TwoDimensionalShape>();

            for (int i = 0; i < optionNum; i++)
            {
                GameObject newShape = (GameObject) PrefabUtility.InstantiatePrefab(optionShapePrefab);
                newShape.transform.SetParent(displayGrid.transform, true);
                newShape.name = "option_shape" + i;
                optionShapes.Add(newShape.GetComponent<TwoDimensionalShape>());
                newShape.GetComponent<TwoDimensionalShape>().group = this;
                newShape.GetComponent<TwoDimensionalShape>().listPosition = i;

                int removeIndex = UnityEngine.Random.Range(0, possibleCombinations.Count);
                (Texture, Color) combo = possibleCombinations[removeIndex];
                possibleCombinations.RemoveAt(removeIndex);
                newShape.GetComponentInChildren<TwoDimensionalShape>().textureColourCombo = combo;

                //set texture
                newShape.transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = combo.Item1;;

                //Set colour
                newShape.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = combo.Item2;
            }
            // set grid to be invisible
            displayGridContainer.SetActive(false);


            //Set up target shapes
            targetNum = trial.settings.GetInt("target_num");
            targetShapes = new List<TwoDimensionalShape>();

            //Set up target shape objects
            while (targetShapes.Count < targetNum)
            {
                // chose a random shape in options to become target (i.e targetList == true)
                int possibleTargetIndex = UnityEngine.Random.Range(0, optionShapes.Count);
                if (!optionShapes[possibleTargetIndex].isTarget)
                {
                    //make the option shape a target
                    optionShapes[possibleTargetIndex].isTarget = true;

                    GameObject newTargetObj = (GameObject) PrefabUtility.InstantiatePrefab(targetShapePrefab);
                    newTargetObj.transform.parent = targetGrid.transform;
                    targetShapes.Add(newTargetObj.GetComponent<TwoDimensionalShape>());
                    newTargetObj.GetComponent<TwoDimensionalShape>().group = this;

                    //save its texture and colour, and index
                    (Texture, Color) targetCombo  = optionShapes[possibleTargetIndex].textureColourCombo;
                    newTargetObj.GetComponent<TwoDimensionalShape>().listPosition = possibleTargetIndex;

                    //Set texture and colours
                    newTargetObj.GetComponent<RawImage>().texture = targetCombo.Item1;
                    newTargetObj.GetComponent<RawImage>().color = targetCombo.Item2;
                }
            }

            //show target shapes for the specified time
            targetGridContainer.SetActive(true);
            yield return new WaitForSeconds(trial.settings.GetFloat("delay_time"));
            targetGridContainer.SetActive(false);

            // show the shapes to tbe selected
            displayGridContainer.SetActive(true);
            confirmButton.SetActive(true);

            //record time for later
            trialStartTime = System.DateTime.Now;
        }

        
        public void Confirm()
        {
            //If not in trial, do nothing
            if (!Session.instance.InTrial) return;

            trialEndTime = System.DateTime.Now;
            double trialTime = (trialEndTime - trialStartTime).TotalSeconds;
    
            int mistakes = 0;
            foreach (TwoDimensionalShape shape in optionShapes)
            {
                //if selected but not target
                if (shape.selected && !shape.isTarget)
                {
                    mistakes++;
                }
            }

            Trial trial = Session.instance.CurrentTrial;
            trial.result["Total_Time_Milliseconds"] = (trialEndTime - trialStartTime).TotalMilliseconds;

            foreach (Transform child in targetGrid.transform) Destroy(child.gameObject);
            foreach (Transform child in displayGrid.transform) Destroy(child.gameObject);

            print("Trial took " + trialTime + " seconds. " + mistakes + " mistakes were made.");
            Session.instance.CurrentTrial.End();
            Session.instance.Invoke("BeginNextTrialSafe", 5);

            coverObj.SetActive(true);
            targetGrid.SetActive(true);
            targetGridContainer.SetActive(true);
            displayGrid.SetActive(true);
            displayGridContainer.SetActive(true);
            startButton.SetActive(true);
            confirmButton.SetActive(true);
        }

        public int getSelectedSize()
        {
            int counter = 0;
            foreach (TwoDimensionalShape shape in optionShapes)
            {
                if (shape.selected) counter++;
            }
            return counter;
        }

        public bool getWaitBool()
        {
            return startWaitToggle;
        }

        public void toggleWait()
        {
            startWaitToggle = !startWaitToggle;
        }

    }
}
