using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorkingMemory
{
    
    public class HeadsetBadge : MonoBehaviour
    {

        private bool isPulsing;
        private bool isIncreasing;

        private bool isPaused;

        public Text questionMark;
        public Text warningText;


        private float startWidth;
        private float startHeight;

        private float maxContraction = 10f; 

        private float expansionRate = 0.25f;

        public PanelGroup script;
        
        private void Start() {
            isPulsing = true;
            isPaused = false;
            startWidth = this.gameObject.GetComponent<RectTransform>().rect.width;
            startHeight = this.gameObject.GetComponent<RectTransform>().rect.height;

            script = GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>();
        }

        

        void Update()
        {
            if (isPulsing && !isPaused)
            {
                if (isIncreasing)
                {
                    if (this.gameObject.GetComponent<RectTransform>().rect.width > startWidth + maxContraction)
                    {
                        isIncreasing = false;
                    }else{
                        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.gameObject.GetComponent<RectTransform>().rect.width + expansionRate, this.gameObject.GetComponent<RectTransform>().rect.height + expansionRate);
                    }
                }else{
                    if (this.gameObject.GetComponent<RectTransform>().rect.width < startWidth - maxContraction)
                    {
                        isIncreasing = true;
                    }else{
                        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(this.gameObject.GetComponent<RectTransform>().rect.width - expansionRate, this.gameObject.GetComponent<RectTransform>().rect.height - expansionRate);
                    }
                }
            }

            if(script.headsetActive)
            {
                this.gameObject.SetActive(false);
            }else{
                this.gameObject.SetActive(true);
            }
        }


        public void badgeMouseOver()
        {
            isPulsing = false;
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(startWidth, startHeight);

            questionMark.gameObject.SetActive(true);
            warningText.gameObject.SetActive(false);
        }

        public void badgeMouseExit()
        {
            questionMark.gameObject.SetActive(false);
            warningText.gameObject.SetActive(true);
        }

        public void setPaused(bool value)
        {
            isPaused = value;
        }

        public void endApplication()
        {
            Application.Quit();
        }
        
    }

}
