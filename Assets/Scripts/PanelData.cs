using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace WorkingMemory
{
    [System.Serializable]
    public class PanelData : MonoBehaviour
    {
        public int dimension;
        public int numberOfTrials;
        public int targetNum;

        public int threeDisplayNum;

        public int twoDisplayNum;

        public string optionDistro;
        public float shapeDisplayTime;

        public float targetToDisplayDelay;

        public bool confirmStart;
        public bool targetRand;
        public bool displayRand;


        // Following taken from the TrialManager
        public List<Texture> selectedTextures;

        public List<(Color, string)> selectedColours;

        public List<Mesh> selectedMeshes; 
        public List<Material> selectedMaterials;

        [HideInInspector]
        public List<Texture> allTextures;
        
        [HideInInspector]
        public List<(Color, string)> allColours;

        [HideInInspector]
        public List<Mesh> allMeshes; 

        [HideInInspector]
        public List<Material> allMaterials;

        public void populateNew() {

            GameObject trialManager = GameObject.Find("TrialManager");

            selectedColours = new List<(Color,string)>();
            allColours = new List<(Color,string)>();

            List<Color> colourList = new List<Color>(trialManager.GetComponent<TwoDimensionalGroup>().possibleColours);
            List<string> stringList = new List<string>(trialManager.GetComponent<TwoDimensionalGroup>().colorNames);
            for (int index = 0; index < colourList.Count; index++)
            {
                selectedColours.Add((colourList[index], stringList[index]));
                allColours.Add((colourList[index], stringList[index]));
            }
            

            allTextures = new List<Texture> (trialManager.GetComponent<TwoDimensionalGroup>().possibleShapes);
            selectedTextures = new List<Texture> (trialManager.GetComponent<TwoDimensionalGroup>().possibleShapes);

            allMaterials = new List<Material>(trialManager.GetComponent<ThreeDimensionalGroup>().possibleColours);
            selectedMaterials = new List<Material>(trialManager.GetComponent<ThreeDimensionalGroup>().possibleColours);

            allMeshes = new List<Mesh>(trialManager.GetComponent<ThreeDimensionalGroup>().possibleShapes);
            selectedMeshes = new List<Mesh>(trialManager.GetComponent<ThreeDimensionalGroup>().possibleShapes);

            // gather variable information from panel
            dimension = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().dimensionBadgeText.text[0].ToString());

            numberOfTrials = 0; // starts empty

            targetNum = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().targetNumDrop.options[this.gameObject.GetComponent<PanelObject>().targetNumDrop.value].text);
            threeDisplayNum = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().threeDisplayDrop.options[this.gameObject.GetComponent<PanelObject>().threeDisplayDrop.value].text);
            twoDisplayNum = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().twoDisplayDrop.options[this.gameObject.GetComponent<PanelObject>().twoDisplayDrop.value].text);
            optionDistro = this.gameObject.GetComponent<PanelObject>().threeLayout.options[this.gameObject.GetComponent<PanelObject>().threeLayout.value].text;

            shapeDisplayTime = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().displayTimeSlider.value);
            targetToDisplayDelay = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().displayDelaySlider.value);
            
            confirmStart = this.gameObject.GetComponent<PanelObject>().confirmStartToggle.isOn;
            targetRand = this.gameObject.GetComponent<PanelObject>().targetRandToggle.isOn;
            displayRand = this.gameObject.GetComponent<PanelObject>().displayRandToggle.isOn;
        }

        public void setTrialNum(Text textInput)
        {
            numberOfTrials = Convert.ToInt32(textInput.text);
            this.gameObject.GetComponent<PanelObject>().checkValidity();
        }

        public void setTargetNum(Dropdown targetInput)
        {
            targetNum = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().targetNumDrop.options[this.gameObject.GetComponent<PanelObject>().targetNumDrop.value].text);
        }

        public void set3dDisplayNum(Dropdown displayInput)
        {
            threeDisplayNum = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().threeDisplayDrop.options[this.gameObject.GetComponent<PanelObject>().threeDisplayDrop.value].text);
        }

        public void set2dDisplayNum(Dropdown displayInput)
        {
            twoDisplayNum = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().twoDisplayDrop.options[this.gameObject.GetComponent<PanelObject>().twoDisplayDrop.value].text);
        }

        public void setOptionDistro(Dropdown displayInput)
        {
            optionDistro = this.gameObject.GetComponent<PanelObject>().threeLayout.options[this.gameObject.GetComponent<PanelObject>().threeLayout.value].text;
        }

        public void colourReset()
        {
            selectedColours = allColours;
            selectedMaterials = allMaterials;
        }

        public void shapeReset()
        {
            selectedTextures = allTextures;
            selectedMeshes = allMeshes;
        }

        public void deselectAllShapes()
        {
            selectedTextures = new List<Texture>();
            selectedMeshes = new List<Mesh>();

        
            this.gameObject.GetComponent<PanelObject>().checkValidity();
        }

        public void updateColour(Toggle toggle, bool isAdded)
        {
            if (isAdded)
            {
                selectedColours.Add((toggle.GetComponent<ColourToggle>().colour, toggle.GetComponent<ColourToggle>().name));
                selectedMaterials.Add(toggle.GetComponent<ColourToggle>().material);
            }
            else{
                selectedColours.Remove((toggle.GetComponent<ColourToggle>().colour, toggle.GetComponent<ColourToggle>().name));
                selectedMaterials.Remove(toggle.GetComponent<ColourToggle>().material);
            }
            this.gameObject.GetComponent<PanelObject>().checkValidity();
        }

        public void updateShape(Toggle toggle, bool isAdded)
        {
            if (isAdded)
            {
                selectedTextures.Add(toggle.GetComponent<ShapeToggle>().texture);
                selectedMeshes.Add(toggle.GetComponent<ShapeToggle>().mesh);
            }
            else{
                selectedTextures.Remove(toggle.GetComponent<ShapeToggle>().texture);
                selectedMeshes.Remove(toggle.GetComponent<ShapeToggle>().mesh);
            }

            this.gameObject.GetComponent<PanelObject>().checkValidity();
        }

        public void updateSliderValue(string name)
        {
            switch(name)
            {
                case "display_time":
                    this.gameObject.GetComponent<PanelObject>().displaySliderValue.text = this.gameObject.GetComponent<PanelObject>().displayTimeSlider.value.ToString();
                    shapeDisplayTime = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().displayTimeSlider.value);
                    break;

                case "delay_time":
                    this.gameObject.GetComponent<PanelObject>().delaySliderValue.text = this.gameObject.GetComponent<PanelObject>().displayDelaySlider.value.ToString();
                    targetToDisplayDelay = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().displayDelaySlider.value);
                    break;
            }
        }
    }

}
