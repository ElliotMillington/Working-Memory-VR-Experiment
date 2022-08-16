using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*

    This script is assigned to each colour toggle object in the colour panel option in a GUI panel.

    Its purpose is to store the colour and materials, corresponding to a given 'colour' which can be added or removed from a trial freely.

*/

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


