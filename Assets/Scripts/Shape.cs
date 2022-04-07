using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WorkingMemory {
    public class Shape : MonoBehaviour
    {
        public bool clickable;
        private bool lightOn;
        public int listPosition;
        public ShapeColourGroup group;
        private float clickedSize;

        public GameObject scriptObj;

        private bool leftHand = false;
        private bool rightHand = false;

        private bool selected = false;


        private void Start()
        {
            lightOn = false;
            Outline script = scriptObj.GetComponent<Outline>();
            Destroy(scriptObj.GetComponent<Outline>());

            if(transform.parent.name == "Display")
            {
                clickedSize = 50f;
            }
            else
            {
                clickedSize = 0.7f;
            }
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
                transform.localScale += new Vector3(clickedSize, clickedSize, clickedSize);
                lightOn = !lightOn;
            }
        }

        public void LightDown()
        {
            if (lightOn && (!leftHand && !rightHand) && !selected)
            {
                transform.localScale -= new Vector3(clickedSize, clickedSize, clickedSize);
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
                    group.RegisterSelect(listPosition, false);
                    selected = !selected;
                    invertOutline(selected);
                } else
                {
                    group.RegisterSelect(listPosition, true);
                    selected = !selected;
                    invertOutline(selected);
                }
            }
        }
    }
}
