using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public bool clickable;
    private bool lightOn;

    private void Start()
    {
        clickable = true;
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

    void OnMouseDown()
    {
        if (clickable)
        {
            if (lightOn)
            {
                LightDown();
                lightOn = false;
            } else
            {
                LightUp();
                lightOn = true;
            }
            //cubeGroup.RegisterClick(id);
        }
    }
}
