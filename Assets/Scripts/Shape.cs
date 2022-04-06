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
        private float clickedSize;

        public GameObject scriptObj;


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

        public void LightUp()
        {
            transform.localScale += new Vector3(clickedSize, clickedSize, clickedSize);
            scriptObj.AddComponent<Outline>();
            scriptObj.GetComponent<Outline>().OutlineWidth = 5f;
        }

        public void LightDown()
        {
            transform.localScale -= new Vector3(clickedSize, clickedSize, clickedSize);
            Destroy(scriptObj.GetComponent<Outline>());
        }

        public  void OnMouseDown()
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
