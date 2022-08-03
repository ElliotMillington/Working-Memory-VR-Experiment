using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;


public class SavePanel : MonoBehaviour
{
    [HideInInspector]
    public int position;

    public Text titleText;
    public Text bannerText;

    [HideInInspector]
    public SaveGroup group;

    public bool fileExists;

    public GameObject saveButton;
    public GameObject loadButton;
    public GameObject deleteButton;

    public Color invalidSave;
    public Color validSave;

    public GameObject invalidText;
    
    private void Start() {
        position = this.gameObject.transform.GetSiblingIndex() + 1;
        titleText.text = "Load " + position;
        group = this.gameObject.GetComponentInParent<SaveGroup>();

        checkFile();
    }

    public void checkFile()
    {
        fileExists = File.Exists(group.getPath(position));

        bannerText.gameObject.SetActive(!fileExists);
        saveButton.gameObject.SetActive(!fileExists);
        loadButton.gameObject.SetActive(fileExists);

        deleteButton.GetComponent<Button>().interactable = fileExists;
    }

    public void deleteSave()
    {
        File.Delete(group.getPath(position));
        pointerExit();
        checkFile();
    }


    public void signalLoad()
    {
        SaveData loadedData = loadPanelData();
        PanelGroup panelGroupScript = GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>();
        StartCoroutine(panelGroupScript.loadPanalDataCollection(loadedData));
    }

    public SaveData loadPanelData()
    {
        using StreamReader reader = new StreamReader(group.getPath(position));
        string json = reader.ReadToEnd();
        
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        return data;
    }

    public void savePanelData(List<PanelData> panelDataCollection)
    {
        SaveData dataObj = new SaveData(panelDataCollection);

        string json = JsonUtility.ToJson(dataObj);       
        
        using StreamWriter writer = new StreamWriter(group.getPath(position));
        writer.Write(json);
    }


    public List<PanelData> collectAllData()
    {
        List<PanelData> newData = new List<PanelData>();

        foreach (PanelObject panelObj in GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>().panelGroup)
        {
            newData.Add(panelObj.dataObject);
        }

        return newData;
    }

    public void pointerEnter()
    {
        if (GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>().checkAllValid())
        {
            //valid
            saveButton.GetComponent<Image>().color = validSave;
        }else{
            //invalid
            saveButton.GetComponent<Image>().color = invalidSave;
            invalidText.SetActive(true);
        }
        
    }

    public void pointerExit()
    {
        saveButton.GetComponent<Image>().color = new Color(1,1,1,1);
        invalidText.SetActive(false);
    }

    public void pointerClick()
    {
        if (GameObject.FindGameObjectWithTag("panelGroup").GetComponent<PanelGroup>().checkAllValid())
        {
            savePanelData(collectAllData());
            checkFile();
        }
    }

}

