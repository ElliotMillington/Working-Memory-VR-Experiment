using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorkingMemory {
    public class Shape : MonoBehaviour
    {
        public bool clickable;
        private bool lightOn;
        public int listPosition;
        public ShapeColourGroup group;

        private float clickedSize = 200f;

        private void Start()
        {
            lightOn = false;
        }

        public void LightUp()
        {
            transform.localScale += new Vector3(clickedSize, clickedSize, clickedSize);
        }

        public void LightDown()
        {
            transform.localScale -= new Vector3(clickedSize, clickedSize, clickedSize);
        }

        void OnMouseDown()
        {
            if (clickable)
            {
                if (lightOn)
                {
                    LightDown();
                    lightOn = false;
                    group.RegisterSelect(listPosition, false);
                } else
                {
                    LightUp();
                    lightOn = true;
                    group.RegisterSelect(listPosition, true);
                }
                //cubeGroup.RegisterClick(id);
            }
        }
    }
}
