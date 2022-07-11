using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorkingMemory
{
    [System.Serializable]
    public class PanelData : MonoBehaviour
    {
        public int dimension;
        public int targetNum;
        public int optionNum;

        public string optionDistro;
        public float delay_time;

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

            Debug.Log(selectedTextures.Count + " " + selectedMeshes.Count);
        }

    }

}
