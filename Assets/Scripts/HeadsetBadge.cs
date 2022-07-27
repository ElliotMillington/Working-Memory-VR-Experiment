using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Management;
using System.Collections;

namespace WorkingMemory
{
    
    public class HeadsetBadge : MonoBehaviour
    {
        private bool isPaused;

        public Text questionMark;
        public Text warningText;

        [HideInInspector]
        public PanelGroup script;

        public GameObject VRBadge;
        public GameObject ErrorMessage;
        
        private void Start() {
            script = GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>();

            if(script.headsetActive)
            {
                VRBadge.SetActive(false);
            }else{
                VRBadge.SetActive(true);
            }
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
            }
        }

        public IEnumerator showVRErrorText()
        {
            ErrorMessage.SetActive(true);
            VRBadge.SetActive(false);

            yield return new WaitForSeconds(5f);

            ErrorMessage.SetActive(false);
            VRBadge.SetActive(true);
            badgeMouseExit();
        }



    }
}
