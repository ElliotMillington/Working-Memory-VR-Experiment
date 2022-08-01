using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace WorkingMemory
{

    
    public static class SaveScript
    {
       
        public static List<PanelData> collectAllData()
        {
            List<PanelData> newData = new List<PanelData>();

            foreach (PanelObject panelObj in GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>().panelGroup)
            {
                newData.Add(panelObj.dataObject);
            }

            return newData;
        }

        public static void SaveDataCollection(List<PanelData> panelDataCollection, int loadoutNum)
        {
            if (!File.Exists(getPartialPath()))
            {
                //create load out directory
                Directory.CreateDirectory(getPartialPath());
            }

            BinaryFormatter formatter = new BinaryFormatter();
            string path = getPath(loadoutNum);
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, panelDataCollection);
            stream.Close();
        }

        public static string getPath(int loadoutNum)
        {
            return getPartialPath() + "/save" + loadoutNum + ".save";
        }

        public static string getPartialPath()
        {
            return Application.persistentDataPath + "/loadouts";
        }

        public static int getHighestLoadoutNum()
        {
            // does not even have a save file folder so no save files
            if (!File.Exists(getPartialPath()))
            {
                //TODO:
                //zero will not be a valid loadout number
                return 0;
            }

            DirectoryInfo saveDirectory = new DirectoryInfo(getPartialPath());
            FileInfo[] directoryFiles = saveDirectory.GetFiles();

            int highestIndex = 0;
            foreach(FileInfo file in directoryFiles)
            {
                int fileInteger = Convert.ToInt32(file.Name[4]);
                if (fileInteger > highestIndex)
                {
                    highestIndex = fileInteger;
                }
            }
            return highestIndex;
        }

        public static List<PanelData> LoadDataCollection(int loadoutNum)
        {
            string path = getPath(loadoutNum);
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);


                List<PanelData> panelDataCollection =  (List<PanelData>) formatter.Deserialize(stream);
                stream.Close();
                return panelDataCollection;
            }else
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }
        }

        

    }

    

}
