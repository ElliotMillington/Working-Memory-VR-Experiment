using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorkingMemory
{

    public class ShapeToggle : MonoBehaviour
    {
        public Mesh mesh;
        public Texture texture;
        public string name;

        public GameObject dataObj;

        [HideInInspector]
        public Toggle toggleObj;

        private void Start() {
            toggleObj = this.gameObject.GetComponent<Toggle>();
        }

        public void correctToggle()
        {
            if (dataObj.GetComponent<PanelData>().selectedTextures.Contains(texture))
            {
                toggleObj.isOn = true;
            }else{
                toggleObj.isOn = false;
            }
        }
    }
}
