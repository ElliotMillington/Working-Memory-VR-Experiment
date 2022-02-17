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

        if (e.target.name.Contains("option_shape") && e.target.parent.name == "Display")
        {

            int optionNum = getShapeIndex(e);
            GameObject.Find("option_shape" + optionNum).GetComponent<WorkingMemory.Shape>().OnMouseDown();

        }

    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.name.Contains("option_shape") && e.target.parent.name == "Display")
        {
            int optionNum = getShapeIndex(e);
            GameObject.Find("option_shape" + optionNum).GetComponent<WorkingMemory.Shape>().LightUp();
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name.Contains("option_shape") && e.target.parent.name == "Display")
        {
            int optionNum = getShapeIndex(e);
            GameObject.Find("option_shape" + optionNum).GetComponent<WorkingMemory.Shape>().LightDown();
        }
    }
}
