using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*

    This script is assigned to each shape toggle object in the shape panel option in a GUI panel.

    Its purpose is to store the texture and mesh, corresponding to a given 'shape' which can be added or removed from a trial freely.

*/

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

    public void correctToggle(List<Texture> selectedTextures)
    {
        toggleObj = this.gameObject.GetComponent<Toggle>();
        if (!selectedTextures.Contains(texture))
        {
            toggleObj.isOn = false;
        }else
        {
            toggleObj.isOn = true;
        }
    }
}

