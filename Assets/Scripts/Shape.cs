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

    private void Start()
        {
            clickable = false;
            lightOn = false;
        }

        public void LightUp()
        {
            transform.localScale += new Vector3(50f, 50f, 50f);
        }

        public void LightDown()
        {
            transform.localScale -= new Vector3(50f, 50f, 50f);
        }

        //TODO Record which shapes have been clicked to know which are mistakes

        void OnMouseDown()
        {
            print("Clicked");
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
