using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorkingMemory
{
    public class ColourToggle : MonoBehaviour
    {
        public Color colour;
        public Material material;
        public string name;

        public GameObject dataObj;

        [HideInInspector]
        public Toggle toggleObj;

        private void Start() {
            toggleObj = this.gameObject.GetComponent<Toggle>();
        }

        public void correctToggle()
        {
            if (dataObj.GetComponent<PanelData>().selectedColours.Contains(colour))
            {
                toggleObj.isOn = true;
            }else{
                toggleObj.isOn = false;
            }
        }

    }

}
