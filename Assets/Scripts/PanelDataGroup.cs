using System.Collections.Generic;
using UnityEngine;

namespace WorkingMemory
{

    [System.Serializable]
    public class PanelDataGroup : MonoBehaviour
    {
        public List<PanelData> panelDataCollection;

        public void collectAllData()
        {
            List<PanelData> newData = new List<PanelData>();

            foreach (PanelObject panelObj in GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>().panelGroup)
            {
                newData.Add(panelObj.dataObject);
            }
        }

    }

}
