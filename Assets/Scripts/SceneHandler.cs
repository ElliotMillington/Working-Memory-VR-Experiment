using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;
using System;

public class SceneHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;

    public String handedness;

    private int getShapeIndex(PointerEventArgs e)
    {
        int num = int.Parse(e.target.name.Remove(e.target.name.IndexOf("option_shape"), "option_shape".Length));
        return num;
    }

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        //shape selection
        if (e.target.name.Contains("option_shape") && (e.target.parent.name == "Display" || e.target.parent.name== "CylinderRoom"))
        {

            int optionNum = getShapeIndex(e);
            GameObject.Find("option_shape" + optionNum).GetComponent<WorkingMemory.ThreeDimensionalShape>().OnMouseDown();
        }

        //confirmation selection
        if (e.target.parent.name == "ConfirmationPlanes")
        {
            GameObject.Find("TrialManager").GetComponent<WorkingMemory.ThreeDimensionalGroup>().Confirm();
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.name.Contains("option_shape") && (e.target.parent.name == "Display" || e.target.parent.name == "CylinderRoom"))
        {
            int optionNum = getShapeIndex(e);
            GameObject.Find("option_shape" + optionNum).GetComponent<WorkingMemory.ThreeDimensionalShape>().invertHandedness(handedness);
            GameObject.Find("option_shape" + optionNum).GetComponent<WorkingMemory.ThreeDimensionalShape>().LightUp();
        }

        //confirmation selection
        if (e.target.parent.name == "ConfirmationPlanes")
        {
            GameObject.Find("TrialManager").GetComponent<WorkingMemory.ThreeDimensionalGroup>().invertHandedness(handedness);
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name.Contains("option_shape") && (e.target.parent.name == "Display" || e.target.parent.name == "CylinderRoom"))
        {
            int optionNum = getShapeIndex(e);
            GameObject.Find("option_shape" + optionNum).GetComponent<WorkingMemory.ThreeDimensionalShape>().invertHandedness(handedness);
            GameObject.Find("option_shape" + optionNum).GetComponent<WorkingMemory.ThreeDimensionalShape>().LightDown();
        }

        //confirmation selection
        if (e.target.parent.name == "ConfirmationPlanes")
        {
            GameObject.Find("TrialManager").GetComponent<WorkingMemory.ThreeDimensionalGroup>().invertHandedness(handedness);
        }
    }
}