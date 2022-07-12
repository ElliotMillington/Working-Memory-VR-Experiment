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
        public float delay_time;


        // Following taken from the TrialManager
        public List<Texture> selectedTextures;

        public List<Color> selectedColours;

        public List<Mesh> selectedMeshes; 
        public List<Material> selectedMaterials;

        private List<Texture> allTextures;
        
        private List<Color> allColours;

        private List<Mesh> allMeshes; 
        private List<Material> allMaterials;

        private void Start() {

            GameObject trialManager = GameObject.Find("TrialManager");

            allColours = selectedColours = new List<Color>(trialManager.GetComponent<TwoDimensionalGroup>().possibleColours);
            allTextures = selectedTextures = new List<Texture> (trialManager.GetComponent<TwoDimensionalGroup>().possibleShapes);
            allMaterials = selectedMaterials = new List<Material>(trialManager.GetComponent<ThreeDimensionalGroup>().possibleColours);
            allMeshes = selectedMeshes = new List<Mesh>(trialManager.GetComponent<ThreeDimensionalGroup>().possibleShapes);

            // gather variable information from panel
            dimension = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().dimensionBadgeText.text[0].ToString());

            numberOfTrials = 0; // starts empty

            targetNum = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().targetNumDrop.options[this.gameObject.GetComponent<PanelObject>().targetNumDrop.value].text);
            threeDisplayNum = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().threeDisplayDrop.options[this.gameObject.GetComponent<PanelObject>().threeDisplayDrop.value].text);
            twoDisplayNum = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().twoDisplayDrop.options[this.gameObject.GetComponent<PanelObject>().twoDisplayDrop.value].text);
            optionDistro = this.gameObject.GetComponent<PanelObject>().threeLayout.options[this.gameObject.GetComponent<PanelObject>().threeLayout.value].text;

            delay_time = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().delaySlider.value);
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
                selectedColours.Add(toggle.GetComponent<ColourToggle>().colour);
                selectedMaterials.Add(toggle.GetComponent<ColourToggle>().material);
            }
            else{
                selectedColours.Remove(toggle.GetComponent<ColourToggle>().colour);
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

        public void updateSliderValue()
        {   
            this.gameObject.GetComponent<PanelObject>().sliderValue.text = this.gameObject.GetComponent<PanelObject>().delaySlider.value.ToString();
            delay_time = Convert.ToInt32(this.gameObject.GetComponent<PanelObject>().delaySlider.value);
        }
    }

}
