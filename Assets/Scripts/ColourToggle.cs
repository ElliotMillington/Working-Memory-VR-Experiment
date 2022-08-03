using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void correctToggle(List<(Color, string)> selectedColours)
    {
        toggleObj = this.gameObject.GetComponent<Toggle>();
        if (!selectedColours.Contains((colour,name)))
        {
            toggleObj.isOn = false;
        }else
        {
            toggleObj.isOn = true;
        }
    }

}


