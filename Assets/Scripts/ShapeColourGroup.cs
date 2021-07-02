using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UXF;

namespace WorkingMemory
{
    public class ShapeColourGroup : MonoBehaviour
    {
        //Shape Colour Variables
        GameObject targetStand;
        float targetZRange = 0.5f;

        GameObject optionDisplay;
        float optionXRange = 4.0f;
        float optionZRange = 4.0f;

        public Shape optionPrefab;
        public Button confirmButton;

        public Mesh[] possibleShapes; //Array of potential shape meshes
        public Material[] possibleColours; //Array of potential colours
        List<GameObject> targets; //Array of target objects
        List<GameObject> shapeOptions; //Array of shape option objects

        List<Shape> optionShapes;
        List<Shape> targetShapes;

        bool[] isTarget; //Records whether the shape is a target
        bool[] isSelected; //Records which shapes have been selected

        private DateTime trialStartTime;
        private DateTime trialEndTime;

        private void Start()
        {
            targetStand = GameObject.Find("Stand");
            optionDisplay = GameObject.Find("Display");
        }

        public IEnumerator CreateShapes(Trial trial)
        {
            float[] zPos;
            float[] xPos;

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
            switch (trial.settings.GetObject("option_distro"))
            {
                case "grid":
                    int nrow = Convert.ToInt32(Math.Ceiling(Mathf.Sqrt(optionNum)));
                    xPos = HelperMethods.Seq(nrow, -optionXRange, optionXRange);
                    zPos = HelperMethods.Seq(nrow, -optionZRange, optionZRange);

                    for (int i = 0; i < positions.Length; i++)
                    {
                        int zRow = i % nrow;
                        int xRow = i / nrow;
                        positions[i] = new Vector3(xPos[xRow], 0.1f, zPos[zRow]);
                    }
                    break;
                case "random":
                    //TODO Set up random positioning
                    break;
            }

            //Set up option shapes.
            optionShapes = new List<Shape>(optionNum);
            foreach(Vector3 pos in positions)
            {
                Shape newShape = Instantiate(optionPrefab);
                newShape.transform.parent = optionDisplay.transform;
                newShape.transform.localPosition = pos;
                newShape.transform.localScale = new Vector3(100, 100, 100);

                newShape.transform.Rotate(new Vector3(90, 0, UnityEngine.Random.Range(-180, 180)));
                newShape.GetComponent<MeshFilter>().mesh = possibleShapes[UnityEngine.Random.Range(0, possibleShapes.Length)];
                newShape.GetComponent<Renderer>().material = possibleColours[UnityEngine.Random.Range(0, possibleColours.Length)];

                newShape.group = this;
                optionShapes.Add(newShape);

                newShape.gameObject.SetActive(false);
            }

            //Set up target shapes
            int targetNum = trial.settings.GetInt("target_num");
            List<int> targetShapesIndex = HelperMethods.GenRandomInts(0, optionNum, targetNum);
            zPos = HelperMethods.Seq(targetNum, -targetZRange, targetZRange);

            targetShapes = new List<Shape>(targetNum);

            //Set up target shape objects
            for (int i = 0; i < targetNum; i++)
            {
                Shape newShape = Instantiate(optionPrefab);
                newShape.transform.parent = targetStand.transform;
                newShape.transform.localPosition = new Vector3(0, 1.2f, zPos[i]);
                newShape.transform.Rotate(new Vector3(0, -90, 90));
                newShape.transform.localScale = new Vector3(100, 100, 100);

                newShape.listPosition = targetShapesIndex[i];
                newShape.GetComponent<MeshFilter>().mesh = optionShapes[i].GetComponent<MeshFilter>().mesh;
                newShape.GetComponent<Renderer>().material = optionShapes[i].GetComponent<Renderer>().material;

                newShape.group = this;
                targetShapes.Add(newShape);

                isTarget[targetShapesIndex[i]] = true;
            }

            yield return new WaitForSeconds(trial.settings.GetFloat("delay_time"));

            foreach (Shape shape in targetShapes) shape.gameObject.SetActive(false);
            foreach (Shape shape in optionShapes)
            {
                shape.clickable = true;
                shape.gameObject.SetActive(true);
            }

            //Start timing the trial
            trialStartTime = System.DateTime.Now;
            //Show confirm button
            confirmButton.gameObject.SetActive(true);
        }

        public void RegisterSelect(int index, bool selected)
        {
            isSelected[index] = selected;
        }

        public void Confirm()
        {
            //If not in trial, do nothing
            if (!Session.instance.InTrial) return;

            trialEndTime = DateTime.Now;

            int mistakes = 0;
            for (int i = 0; i < isSelected.Length; i++)
            {
                if (isSelected[i] != isTarget[i])
                {
                    mistakes++;
                }
            }

            Trial trial = Session.instance.CurrentTrial;
            trial.result["Time"] = (trialStartTime - trialEndTime).Milliseconds;
            trial.result["Errors"] = mistakes;

            confirmButton.gameObject.SetActive(false);
            foreach (Transform child in targetStand.transform) Destroy(child.gameObject);
            foreach (Transform child in optionDisplay.transform) Destroy(child.gameObject);

            Session.instance.CurrentTrial.End();
        }
    }
}
