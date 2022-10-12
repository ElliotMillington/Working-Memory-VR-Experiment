using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*

    This script acts upon the collection of panel object scripts.

*/

public class PanelGroup : MonoBehaviour
{
    [HideInInspector]
    public List<PanelObject> panelGroup;

    public GameObject PanelPrefab;
    public GameObject moveSign;
    public GameObject deleteSign;

    public GameObject duplicateSign;

    public GameObject confirmSign;

    public GameObject confirmText;

    public GameObject UXFRig;

    private bool move;
    private bool delete;

    private bool duplicate;

    public bool allValid;

    public Color confirmValidColor;

    public Color confirmInvalidColor;

    public Texture confirmOriginalTexture;

    public Texture confirmInvalidTexture;

    public Color whiteColour;

    public bool headsetActive = false;

    public GameObject VRErrorBadge;

    public HeadsetBadge headsetScript;

    public bool headsetOverwrite;

    private void Start() {
        move = false;
        delete = false;
        duplicate = false;
        
        allValid = false;

        panelGroup = new List<PanelObject>(this.gameObject.GetComponentsInChildren<PanelObject>());
        this.enforceMove("enforce");
        enforceTitle();
    }


    public void enforceMove(string statement)
    {
        if (statement == "toggle")
        {
            move = !move;
        }

        foreach (PanelObject obj in panelGroup)
        {
            obj.setDelete(delete);
        }

        if(move)
        {
            foreach (PanelObject panel in panelGroup)
            {
                panel.checkButtons();
            }
        }else{
            foreach (PanelObject panel in panelGroup)
            {
                panel.negateMovement();
            }
        }

        if (panelGroup.Count > 1)
        {
            moveSign.SetActive(true);
        }
        else
        {
            moveSign.SetActive(false);
        }

        if (panelGroup.Count > 0)
        {
            deleteSign.SetActive(true);
            confirmSign.SetActive(true);
            duplicateSign.SetActive(true);
        }
        else
        {
            deleteSign.SetActive(false);
            confirmSign.SetActive(false);
            duplicateSign.SetActive(false);
        }

        
        foreach (PanelObject panel in panelGroup)
        {
            panel.duplicateButton.SetActive(duplicate);
        }
    

    }

    public void enforceTitle()
    {
        foreach (PanelObject panel in panelGroup)
        {
            panel.panelTitle.text = "Block " + (panel.transform.GetSiblingIndex() + 1);
        }
    }

    public void toogleDelete()
    {
        delete = !delete;
        foreach (PanelObject obj in panelGroup)
        {
            obj.setDelete(delete);
        }
    }

    public void toogleDuplicate()
    {
        duplicate = !duplicate;
        foreach (PanelObject obj in panelGroup)
        {
            obj.setDuplicate(duplicate);
        }
    }

    public void createPanel()
    {
        GameObject newPanel = (GameObject) Instantiate(PanelPrefab, this.transform);
        newPanel.GetComponent<PanelData>().populateNew();
        newPanel.transform.localScale = new Vector3(1,1,1);
        newPanel.transform.SetSiblingIndex((this.transform.childCount)-2);

        panelGroup.Add(newPanel.GetComponent<PanelObject>());

        enforceMove("enforce");
        newPanel.GetComponent<PanelObject>().setDelete(delete);
        enforceTitle();
    }

    public GameObject returnNewPanel()
    {
        return this.gameObject.transform.GetChild((this.transform.childCount)-2).gameObject;
    }

    public void createPositionalPanel(string prefabName, int pos)
    {
        GameObject newPanel = (GameObject) Instantiate(PanelPrefab, this.transform);
        newPanel.transform.localScale = new Vector3(1,1,1);
        newPanel.GetComponent<PanelData>().populateNew();
        newPanel.transform.SetSiblingIndex(pos);

        panelGroup[pos] = newPanel.GetComponent<PanelObject>();

        enforceMove("enforce");
        newPanel.GetComponent<PanelObject>().setDelete(delete);
        enforceTitle();
    }

    public void switchIndexes(int index1, int index2)
    {
        PanelObject elem1 = panelGroup[index1];
        PanelObject elem2 = panelGroup[index2];

        panelGroup[index1] = elem2;
        panelGroup[index2] = elem1;

        enforceTitle();
    }

    public void removeIndex(int index)
    {
        PanelObject removedPanel = panelGroup[index];
        panelGroup.RemoveAt(index);

        //move it out of the way
        removedPanel.gameObject.SetActive(false);
        removedPanel.gameObject.transform.parent = moveSign.transform;
        Destroy(removedPanel.gameObject);

        enforceTitle();
        enforceMove("enforce");
        allValid = checkAllValid();
    }

    public bool checkAllValid()
    {
        if(panelGroup.Count ==0) return false;

        foreach (PanelObject obj in panelGroup){
            obj.checkValidity("groupScript");
            if (!obj.isValid)
            {
                return false;
            }
        }
        return true;
    }

    public void confirmMouseOver()
    {
        if (allValid)
        {
            //colour is green
            confirmSign.GetComponent<RawImage>().color = confirmValidColor;
            confirmSign.GetComponent<RawImage>().texture = confirmOriginalTexture;

        }else{
            // colour is red and show text
            confirmText.SetActive(true);
            confirmSign.GetComponent<RawImage>().color = confirmInvalidColor;
            confirmSign.GetComponent<RawImage>().texture = confirmInvalidTexture;
        }
    }

    public void confirmMouseLeave()
    {
        confirmSign.GetComponent<RawImage>().color = whiteColour;
        confirmSign.GetComponent<RawImage>().texture = confirmOriginalTexture;
        confirmText.SetActive(false);
    }

    public void confirmMouseDown()
    {
        if (allValid)
        {
            UXFRig.SetActive(true);
        }
    }


    // create panel at a specific point
    public void duplicationAtIndex(int index, PanelObject newObj)
    {
        panelGroup.Insert(index+1, newObj);
        enforceTitle();
        enforceMove("enforce");
    }

    // This function is used in testing the allow the use of 3d scenes when VR is not detected
    public void toggleVROverwite()
    {
        headsetOverwrite = !headsetOverwrite;

        if (headsetOverwrite)
        {
            headsetActive = true;
            VRErrorBadge.SetActive(false);
        }else{
            headsetActive = false;
            VRErrorBadge.SetActive(true);
        }
    }

    public void deleteAll()
    {
        foreach(PanelObject panelObj in panelGroup)
        {
            Destroy(panelObj.gameObject);
        }
        panelGroup = new List<PanelObject>();
        enforceMove("enforce");
    }

    // used in saving to load the object returned form the SaveData script
    public IEnumerator loadPanalDataCollection(SaveData dataObj)
    {
        //delete what was there before
        deleteAll();

        List<PanelObject> panelObjCollection = new List<PanelObject>();
        for (int counter = 0; counter < dataObj.panelDataCount; counter++)
        {
            createPanel();
            GameObject createdPanel = returnNewPanel();

            int dimension = dataObj.dimensionList[counter];
            int numberOfTrials = dataObj.numberOfTrialsList[counter];
            int targetNum = dataObj.targetNumList[counter];
            int threeDisplayNum = dataObj.threeDisplayNumList[counter];
            int twoDisplayNum = dataObj.twoDisplayNumList[counter];
            string optionDistro = dataObj.optionDistroList[counter];
            float shapeDisplayTime = dataObj.shapeDisplayTimeList[counter];
            float targetToDisplayDelay = dataObj.targetToDisplayDelayList[counter];
            bool confirmStart = dataObj.confirmStartList[counter];
            bool targetRand = dataObj.targetRandList[counter];
            bool displayRand = dataObj.displayRandList[counter];

            PanelData panelDataScript = createdPanel.GetComponent<PanelData>();

            panelObjCollection.Add(createdPanel.GetComponent<PanelObject>());
            List<(Color,string)> selectedColours = panelDataScript.convertFromIndices(dataObj.selectedColoursIndexes[counter], panelDataScript.allColours);
            List<Texture> selectedTextures = panelDataScript.convertFromIndices(dataObj.selectedTexturesIndexes[counter], panelDataScript.allTextures);

            StartCoroutine(createdPanel.GetComponent<PanelObject>().swapScripts(dimension,numberOfTrials,targetNum,twoDisplayNum,threeDisplayNum,optionDistro,selectedColours,selectedTextures,shapeDisplayTime,targetToDisplayDelay,confirmStart,targetRand,displayRand));
        }

        GameObject.FindGameObjectWithTag("Save_Panel").SetActive(false);

        foreach (PanelObject panelObjScript in panelObjCollection)
        {
            panelObjScript.checkValidity("groupScript");
        }

        yield return new WaitForSeconds(0.5f); 
        enforceTitle();
        
    }


}
