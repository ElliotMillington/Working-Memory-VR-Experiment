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

        public void correctToggle(PanelData switchPanel)
        {
            toggleObj = this.gameObject.GetComponent<Toggle>();
            if (!switchPanel.selectedColours.Contains(colour))
            {
                toggleObj.isOn = false;
            }else
            {
                toggleObj.isOn = true;
            }
        }

    }

}
