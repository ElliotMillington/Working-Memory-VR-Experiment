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
    public GameObject ErrorMessage;

    public bool showingError;

    private void Start() {

        script = GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>();

        // check if VR equipment is present
        script.headsetActive = isPresent();


        if (script.headsetActive)
        { 
        VRBadge.SetActive(false);
        ErrorMessage.SetActive(false);
        }
        else
        {
            VRBadge.SetActive(true);
            ErrorMessage.SetActive(false);
        }

        StartCoroutine(checkVRLoop());
    }

    public IEnumerator checkVRLoop()
    {
        yield return new WaitForSeconds(5.0f);

        // check is present
        script.headsetActive = isPresent();

        Debug.Log("Checking...");

        //check again
        StartCoroutine(checkVRLoop());
    }

    public bool isPresent()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
        foreach (var xrDisplay in xrDisplaySubsystems)
        {
            if (xrDisplay.running)
            {
                if (!showingError) VRBadge.SetActive(false);
                return true;
            }
        }
        if (!showingError) VRBadge.SetActive(true);
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

    public void endApplication()
    {
        Application.Quit();
    }
    

    public void initialiseVR()
    {
        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        XRGeneralSettings.Instance.Manager.StartSubsystems();

        if (!XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            StartCoroutine(showVRErrorText());
        }else{
            script.headsetActive = true;
            script.checkAllValid();
        }
    }

    public IEnumerator showVRErrorText()
    {
        ErrorMessage.SetActive(true);
        VRBadge.SetActive(false);

        showingError = true;

        yield return new WaitForSeconds(5f);

        ErrorMessage.SetActive(false);
        if(!script.headsetOverwrite){
            VRBadge.SetActive(true);
        }
        showingError = false;
        badgeMouseExit();
    }


}

