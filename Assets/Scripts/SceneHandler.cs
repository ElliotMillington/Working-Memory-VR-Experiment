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

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        //check not in the waiting phase
        WorkingMemory.ThreeDimensionalGroup script = GameObject.Find("TrialManager").GetComponent<WorkingMemory.ThreeDimensionalGroup>();
        if (!(script.confirm_start && script.startWaitToggle))
        {
            //shape selection
            if (e.target.name.Contains("option_shape"))
            {
                e.target.gameObject.GetComponent<WorkingMemory.ThreeDimensionalShape>().OnMouseDown();
            }else{
                //confirmation selection
                if (e.target.parent.name == "ConfirmationPlanes")
                {
                    GameObject.Find("TrialManager").GetComponent<WorkingMemory.ThreeDimensionalGroup>().Confirm();
                }
            }
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        //check not in the waiting phase
        WorkingMemory.ThreeDimensionalGroup script = GameObject.Find("TrialManager").GetComponent<WorkingMemory.ThreeDimensionalGroup>();
        if (!(script.confirm_start && script.startWaitToggle))
        {
            if (e.target.name.Contains("option_shape"))
            {
                e.target.gameObject.GetComponent<WorkingMemory.ThreeDimensionalShape>().invertHandedness(handedness);
                e.target.gameObject.GetComponent<WorkingMemory.ThreeDimensionalShape>().LightUp();
            }else{
                //confirmation selection
                if (e.target.parent != null)
                {
                    if (e.target.parent.name == "ConfirmationPlanes")
                    {
                        GameObject.Find("TrialManager").GetComponent<WorkingMemory.ThreeDimensionalGroup>().invertHandedness(handedness);
                    }
                }
            }
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        //check not in the waiting phase
        WorkingMemory.ThreeDimensionalGroup script = GameObject.Find("TrialManager").GetComponent<WorkingMemory.ThreeDimensionalGroup>();
        if (!(script.confirm_start && script.startWaitToggle))
        {

            if (e.target.name.Contains("option_shape"))
            {
                e.target.gameObject.GetComponent<WorkingMemory.ThreeDimensionalShape>().invertHandedness(handedness);
                e.target.gameObject.GetComponent<WorkingMemory.ThreeDimensionalShape>().LightDown();
            }else{
                //confirmation selection
                if (e.target.parent != null)
                {
                    if (e.target.parent.name == "ConfirmationPlanes")
                    {
                        GameObject.Find("TrialManager").GetComponent<WorkingMemory.ThreeDimensionalGroup>().invertHandedness(handedness);
                    }
                }
            }
        }
    }
}