using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoDimensionalShape : MonoBehaviour
{
    [HideInInspector]
    public TwoDimensionalGroup group;
    [HideInInspector]
    public int listPosition;

    public bool selected = false;

    public bool isTarget = false;

    public RawImage buttonShape;

    public (Texture, (Color, string)) textureColourCombo;

    private void Start() {
        Color currColour = buttonShape.color;
        currColour.a = 1;
        buttonShape.color = currColour;
    }

    
    public  void OnMouseDown()
    {
        if (selected)
        {
            //already selected and need to unselect
            selected = !selected;

            //remove from selected
            group.selectedShapes.Remove(this);

            Color currColour = buttonShape.color;
            currColour.a = 1;
            buttonShape.color = currColour;
            
        } else
        {
            //not selected and need to select

            //only select if current number of selected does not exceed the total number of target shapes

            if (group.getSelectedSize() < group.targetNum)
            {
                selected = !selected;

                group.selectedShapes.Add(this);

                Color currColour = buttonShape.color;
                currColour.a = 0.5f;
                buttonShape.color = currColour;
            }
        }
        if (group.getSelectedSize() == group.targetNum)
        {
            group.confirmButton.GetComponent<Image>().color = group.confirmButtonReadyColour;
        }else{
            group.confirmButton.GetComponent<Image>().color = group.confirmButtonDefaultColour;
        }
    }
}

