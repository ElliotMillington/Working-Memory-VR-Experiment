using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;


public class SaveGroup : MonoBehaviour
{
    public GameObject SavePrefab;
    private void Start()
    {
        //TODO: get highest index of saved files and create that many save panels
        int highest = getHighestLoadoutNum();
        
        if (highest > 0 )
        {
            for (int counter = 1; counter <= highest; counter++)
            {
                createSavePanel();
            }
        }
        
    }

    public void createSavePanel()
    {
        GameObject newPanel = (GameObject) Instantiate(SavePrefab, this.transform);
        newPanel.transform.SetSiblingIndex((this.transform.childCount)-2); 
    }

    public string getPath(int loadoutNum)
    {
        return getPartialPath() + Path.AltDirectorySeparatorChar + "save" + loadoutNum + ".json";
    }

    public string getPartialPath()
    {
        return Application.persistentDataPath + Path.AltDirectorySeparatorChar + "loadouts";
    }

    public int getHighestLoadoutNum()
    {
        // does not even have a save file folder so no save files
        if (!Directory.Exists(getPartialPath()))
        {
            return 0;
        }

        DirectoryInfo saveDirectory = new DirectoryInfo(getPartialPath());
        FileInfo[] directoryFiles = saveDirectory.GetFiles();

        int highestIndex = 0;
        foreach(FileInfo file in directoryFiles)
        {
            if (file.Extension == ".json")
            {
                int fileInteger = Convert.ToInt32(file.Name.Substring(4,file.Name.Length-9));
                if (fileInteger > highestIndex)
                {
                    highestIndex = fileInteger;
                }
            }
        }
        return highestIndex;
    }
    
    
}
