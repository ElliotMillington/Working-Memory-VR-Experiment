using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ThreeDimensionalShape : MonoBehaviour
{
    public bool clickable;
    private bool lightOn;
    public int listPosition;
    public ThreeDimensionalGroup group;

    public GameObject scriptObj;

    private bool leftHand = false;
    private bool rightHand = false;

    public bool selected = false;

    public bool isTarget = false;

    public (Mesh, Material) meshMaterialCombo;

    private void Start()
    {
        lightOn = false;
        Outline script = scriptObj.GetComponent<Outline>();
        Destroy(scriptObj.GetComponent<Outline>());
    }

    public void setRandomRotation()
    {
        transform.rotation = UnityEngine.Random.rotation;
    }

    public void invertHandedness(String handedness)
    {
        if (handedness == "right")
        {
            leftHand = !leftHand;
        }
        else
        {
            rightHand = !rightHand;
        }
    }

    public void LightUp()
    {
        if (!lightOn && (leftHand || rightHand))
        {
            lightOn = !lightOn;
        }
    }

    public void LightDown()
    {
        if (lightOn && (!leftHand && !rightHand) && !selected)
        {
            lightOn = !lightOn;
        }
        
    }

    public void invertOutline(bool selected)
    {
        if (selected)
        {
            scriptObj.AddComponent<Outline>();
            scriptObj.GetComponent<Outline>().OutlineWidth = 5f;
        }
        else
        {
            Destroy(scriptObj.GetComponent<Outline>());
        }
    }

    public  void OnMouseDown()
    {
        if (clickable)
        {
            if (selected)
            {
                //already selected and need to unselect
                group.RegisterSelect(this, listPosition, false);
                selected = !selected;
                invertOutline(selected);
            } else
            {
                //not selected and need to select

                //only select if current number of selected does not exceed the total number of target shapes

                if (group.getSelectedSize() < group.targetNum)
                {
                    group.RegisterSelect(this, listPosition, true);
                    selected = !selected;
                    invertOutline(selected);
                }
            }
        }
    }
}

