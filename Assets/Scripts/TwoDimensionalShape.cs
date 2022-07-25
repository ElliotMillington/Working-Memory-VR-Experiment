using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorkingMemory
{
    public class TwoDimensionalShape : MonoBehaviour
    {
        [HideInInspector]
        public TwoDimensionalGroup group;
        [HideInInspector]
        public int listPosition;

        private bool selected = false;

        public RawImage buttonShape;

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
                group.RegisterSelect(listPosition, false);
                selected = !selected;

                Color currColour = buttonShape.color;
                currColour.a = 1;
                buttonShape.color = currColour;
                
            } else
            {
                //not selected and need to select

                //only select if current number of selected does not exceed the total number of target shapes

                if (group.getSelectedSize() < group.targetNum)
                {
                    group.RegisterSelect(listPosition, true);
                    selected = !selected;

                    Color currColour = buttonShape.color;
                    currColour.a = 0.5f;
                    buttonShape.color = currColour;
                }
            }
        }
    }
}
