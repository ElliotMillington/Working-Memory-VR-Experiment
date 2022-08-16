using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.XR;

/*

    This script tracks if the VR equiment can be found on system start.

    If headset not found then badge indicating this will be displayed and 3d options disabled.


*/

public class HeadsetBadge : MonoBehaviour
{
    private bool isPaused;

    public Text questionMark;
    public Text warningText;

    [HideInInspector]
    public PanelGroup script;

    public GameObject VRBadge;

    private void Start()
    {

        script = GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>();

        // check if VR equipment is present
        script.headsetActive = isPresent();

    }

    // detect if there are VR devices connected
    public bool isPresent()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
        foreach (var xrDisplay in xrDisplaySubsystems)
        {
            if (xrDisplay.running)
            {
                VRBadge.SetActive(false);
                return true;
            }
        }
        VRBadge.SetActive(true);
        return false;
    }

    public void badgeMouseOver()
    {
        questionMark.gameObject.SetActive(true);
        warningText.gameObject.SetActive(false);
    }

    public void badgeMouseExit()
    {
        questionMark.gameObject.SetActive(false);
        warningText.gameObject.SetActive(true);
    }
    
}

