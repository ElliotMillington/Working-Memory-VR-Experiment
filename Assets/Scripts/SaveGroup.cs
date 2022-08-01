using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace WorkingMemory
{
    
    public class SaveGroup : MonoBehaviour
    {
        public GameObject SavePrefab;
        private void Start() {

            int highestIndex = SaveScript.getHighestLoadoutNum();
            for (int i = 0; i < highestIndex; i++)
            {
                GameObject newPanel = (GameObject) PrefabUtility.InstantiatePrefab(SavePrefab, this.transform);
                newPanel.transform.SetSiblingIndex((this.transform.childCount)-2);
            }
        }

        public void createSavePanel()
        {
            GameObject newPanel = (GameObject) PrefabUtility.InstantiatePrefab(SavePrefab, this.transform);
            newPanel.transform.SetSiblingIndex((this.transform.childCount)-2); 
        }

        public void loadSave(int loadoutNum)
        {
           
            if (!File.Exists(SaveScript.getPath(loadoutNum))) return;
            
            // first delete all current panel objects to make room for new ones
            PanelGroup panelGroupScript = GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>();
            panelGroupScript.deleteAll();
            
           List<PanelData> loadedData = SaveScript.LoadDataCollection(loadoutNum);

           foreach (PanelData data in loadedData)
           {
                panelGroupScript.createPanel();
                GameObject newPanel = panelGroupScript.returnNewPanel();
                newPanel.GetComponent<PanelObject>().swapScripts(data);
           }
        }

    }

}
