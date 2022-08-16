using UnityEngine;
using UnityEngine.UI;

/*

    This script is assigned to all two dimensional shapes, both target and display.

    Holds functions which manage the selection of the display shapes.

*/

public class TwoDimensionalShape : MonoBehaviour
{
    [HideInInspector]
    public TwoDimensionalGroup group;
    [HideInInspector]
    public int listPosition;

    public bool selected = false;

    public bool isTarget = false;
    public RawImage selectedImage;

    public (Texture, (Color, string)) textureColourCombo;

    public  void OnMouseDown()
    {
        if (selected)
        {
            //already selected and need to unselect
            selected = !selected;

            //remove from selected
            group.selectedShapes.Remove(this);

            // need to make its alpha zero
            Color selectedImageColor = selectedImage.color;
            selectedImageColor.a = 0;
            selectedImage.color = selectedImageColor;
            
        } else
        {
            //not selected and need to select

            //only select if current number of selected does not exceed the total number of target shapes

            if (group.getSelectedSize() < group.targetNum)
            {
                selected = !selected;

                group.selectedShapes.Add(this);

                //need to make selected shape visible visible
                Color selectedImageColor = selectedImage.color;
                selectedImageColor.a = 0.5f;
                selectedImage.color = selectedImageColor;
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

