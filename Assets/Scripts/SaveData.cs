using UnityEngine;
using System.Collections.Generic;
using System;


public class SaveData
{
    public int panelDataCount;
    public int[] dimensionList;
    public int[] numberOfTrialsList;
    public int[] targetNumList;

    public int[] threeDisplayNumList;

    public int[] twoDisplayNumList;

    public string[] optionDistroList;
    public float[] shapeDisplayTimeList;

    public float[] targetToDisplayDelayList;

    public bool[] confirmStartList;
    public bool[] targetRandList;
    public bool[] displayRandList;

    public string[] selectedTexturesIndexes;

    public string[] selectedColoursIndexes;

    public string[] selectedMeshesIndexes; 
    public string[] selectedMaterialsIndexes;


    public SaveData(List<PanelData> scriptCollection)
    {
        this.panelDataCount = scriptCollection.Count;
        
        List<int> dimensionList = new List<int>();
        List<int> numberOfTrialsList= new List<int>();
        List<int> targetNumList= new List<int>();
        List<int> threeDisplayNumList= new List<int>();
        List<int> twoDisplayNumList= new List<int>();
        List<string> optionDistroList= new List<string>();

        List<float> shapeDisplayTimeList= new List<float>();
        List<float> targetToDisplayDelayList = new List<float>();
        List<bool> confirmStartList = new List<bool>();
        List<bool> targetRandList = new List<bool>();
        List<bool> displayRandList = new List<bool>();

        List<List<int>> selectedTexturesIndexes = new List<List<int>>();
        List<List<int>> selectedColoursIndexes = new List<List<int>>();
        List<List<int>> selectedMeshesIndexes = new List<List<int>>();
        List<List<int>> selectedMaterialsIndexes = new List<List<int>>();

        foreach(PanelData datascript in scriptCollection)
        {
            dimensionList.Add(datascript.dimension);
            numberOfTrialsList.Add(datascript.numberOfTrials);
            targetNumList.Add(datascript.targetNum);
            threeDisplayNumList.Add(datascript.threeDisplayNum);
            twoDisplayNumList.Add(datascript.twoDisplayNum);
            optionDistroList.Add(datascript.optionDistro);
            shapeDisplayTimeList.Add(datascript.shapeDisplayTime);
            targetToDisplayDelayList.Add(datascript.targetToDisplayDelay);
            confirmStartList.Add(datascript.confirmStart);
            targetRandList.Add(datascript.targetRand);
            displayRandList.Add(datascript.displayRand);

            selectedTexturesIndexes.Add(datascript.convertToIndices(datascript.allTextures, datascript.selectedTextures));
            selectedColoursIndexes.Add(datascript.convertToIndices(datascript.allColours, datascript.selectedColours));
            selectedMeshesIndexes.Add(datascript.convertToIndices(datascript.allMeshes, datascript.selectedMeshes));
            selectedMaterialsIndexes.Add(datascript.convertToIndices(datascript.allMaterials, datascript.selectedMaterials));
        }

        this.dimensionList = dimensionList.ToArray();
        this.numberOfTrialsList = numberOfTrialsList.ToArray();
        this.targetNumList = targetNumList.ToArray();
        this.threeDisplayNumList = threeDisplayNumList.ToArray();
        this.twoDisplayNumList = twoDisplayNumList.ToArray();
        this.optionDistroList = optionDistroList.ToArray();
        this.shapeDisplayTimeList = shapeDisplayTimeList.ToArray();
        this.targetToDisplayDelayList = targetToDisplayDelayList.ToArray();
        this.confirmStartList = confirmStartList.ToArray();
        this.targetRandList = targetRandList.ToArray();
        this.displayRandList = displayRandList.ToArray();

        this.selectedColoursIndexes = flattenList(selectedColoursIndexes);
        this.selectedMaterialsIndexes = flattenList(selectedMaterialsIndexes);
        this.selectedMeshesIndexes = flattenList(selectedMeshesIndexes);
        this.selectedTexturesIndexes = flattenList(selectedTexturesIndexes);

    }

    public string[] flattenList(List<List<int>> twoDimensionalList)
    {
        List<string> stringList = new List<string>();
        foreach(List<int> elem in twoDimensionalList)
        {
            stringList.Add(String.Join(" | ", elem));
        }
        return stringList.ToArray();
    }
}
