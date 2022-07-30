using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UXF;
using UnityEngine.EventSystems;

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
        List<(Color,string)> selectedColours;

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

        [HideInInspector]
        public List<TwoDimensionalShape> selectedShapes;
            

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

            
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener( (eventData) => { Confirm(); } );
            confirmButton.GetComponent<EventTrigger>().triggers.Add(entry);

            //meshes and materials passed by the user
            selectedTextures = (List<Texture>)trial.settings.GetObject("selected_textures");

            //TODO:
            selectedColours = (List<(Color,string)>)trial.settings.GetObject("selected_colours");

            List <(Texture, (Color, string))> possibleCombinations = new List <(Texture,(Color, string))>();
            foreach(Texture textureItem in selectedTextures)
            {
                foreach((Color colourItem, string colourName) in selectedColours)
                {
                    // will hold all possible tuples of mesh and material
                    possibleCombinations.Add((textureItem, (colourItem, colourName)));
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
                (Texture, (Color, string)) combo = possibleCombinations[removeIndex];
                possibleCombinations.RemoveAt(removeIndex);
                newShape.GetComponentInChildren<TwoDimensionalShape>().textureColourCombo = combo;

                //set texture
                newShape.transform.GetChild(0).gameObject.GetComponent<RawImage>().texture = combo.Item1;

                //Set colour
                newShape.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = combo.Item2.Item1;

                Debug.Log(combo.Item1.name + " " + combo.Item2.Item2);
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
                    (Texture, (Color, string)) targetCombo  = optionShapes[possibleTargetIndex].textureColourCombo;
                    newTargetObj.GetComponent<TwoDimensionalShape>().listPosition = possibleTargetIndex;

                    //Set texture and colours
                    newTargetObj.GetComponent<RawImage>().texture = targetCombo.Item1;
                    newTargetObj.GetComponent<RawImage>().color = targetCombo.Item2.Item1;
                }
            }

            //show target shapes for the specified time
            targetGridContainer.SetActive(true);
            yield return new WaitForSeconds(trial.settings.GetFloat("shape_display_time"));
            targetGridContainer.SetActive(false);

            //make time in between taking away target and the display of the shapes
            yield return new WaitForSeconds(trial.settings.GetFloat("delay_time"));

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
    
            // not really measuring mistakes
            int mistakes = 0;
            foreach (TwoDimensionalShape shape in optionShapes)
            {
                //if selected but not target
                if (shape.selected && !shape.isTarget)
                {
                    mistakes++;
                }
            }

            //TODO: if mistakes are greater than 0
            // remember selectedShapes which has textureColourCombo which has name of colour

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
            return selectedShapes.Count;
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
