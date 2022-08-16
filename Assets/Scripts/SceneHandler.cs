using UnityEngine;
using Valve.VR.Extras;
using System;

/*

    Script assigned to each of the VR controllers to manage clicking in the 3 dimensional space

*/

public class SceneHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;

    public String handedness;

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        //check not in the waiting phase
        ThreeDimensionalGroup script = GameObject.Find("TrialManager").GetComponent<ThreeDimensionalGroup>();

        if (!script.confirm_start ||(script.confirm_start && script.startWaitToggle))
        {
            //shape selection
            if (e.target.name.Contains("option_shape"))
            {
                e.target.gameObject.GetComponent<ThreeDimensionalShape>().OnMouseDown();
            }else{
                //confirmation selection
                if (e.target.parent.name == "ConfirmationPlanes")
                {
                    GameObject.Find("TrialManager").GetComponent<ThreeDimensionalGroup>().Confirm();
                }
            }
        }else{
            if (e.target.name.Contains("StartButton")){
                script.startWaitToggle = true;
            }
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        //check not in the waiting phase
        ThreeDimensionalGroup script = GameObject.Find("TrialManager").GetComponent<ThreeDimensionalGroup>();
        if (!script.confirm_start || (script.confirm_start && script.startWaitToggle))
        {
            if (e.target.name.Contains("option_shape"))
            {
                e.target.gameObject.GetComponent<ThreeDimensionalShape>().invertHandedness(handedness);
                e.target.gameObject.GetComponent<ThreeDimensionalShape>().LightUp();
            }else{
                //confirmation selection
                if (e.target.parent != null)
                {
                    if (e.target.parent.name == "ConfirmationPlanes")
                    {
                        GameObject.Find("TrialManager").GetComponent<ThreeDimensionalGroup>().invertHandedness(handedness);
                    }
                }
            }
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        //check not in the waiting phase
        ThreeDimensionalGroup script = GameObject.Find("TrialManager").GetComponent<ThreeDimensionalGroup>();
        if (!script.confirm_start || (script.confirm_start && script.startWaitToggle))
        {

            if (e.target.name.Contains("option_shape"))
            {
                e.target.gameObject.GetComponent<ThreeDimensionalShape>().invertHandedness(handedness);
                e.target.gameObject.GetComponent<ThreeDimensionalShape>().LightDown();
            }else{
                //confirmation selection
                if (e.target.parent != null)
                {
                    if (e.target.parent.name == "ConfirmationPlanes")
                    {
                        GameObject.Find("TrialManager").GetComponent<ThreeDimensionalGroup>().invertHandedness(handedness);
                    }
                }
            }
        }
    }
}