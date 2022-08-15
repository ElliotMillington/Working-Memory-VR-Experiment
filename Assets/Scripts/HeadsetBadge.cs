using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;


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

