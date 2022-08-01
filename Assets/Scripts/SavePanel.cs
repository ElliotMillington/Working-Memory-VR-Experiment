using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace WorkingMemory
{
    public class SavePanel : MonoBehaviour
    {
        [HideInInspector]
        public int position;

        public Text titleText;
        public Text bannerText;

        public SaveGroup group;
        
        private void Start() {
            position = this.gameObject.transform.GetSiblingIndex() + 1;
            titleText.text = "Load " + position;
            group = this.gameObject.GetComponentInParent<SaveGroup>();
        }

        public void saveLoudout()
        {
            List<PanelData> data = SaveScript.collectAllData();
            SaveScript.SaveDataCollection(data, this.position);
        }

        public void signalLoad()
        {
            group.loadSave(position);
        }

    }
}
