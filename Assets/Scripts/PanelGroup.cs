using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace WorkingMemory
{
public class PanelGroup : MonoBehaviour
{
    [HideInInspector]
    public List<PanelObject> panelGroup;

    public GameObject PanelPrefab;
    public GameObject moveSign;
    public GameObject deleteSign;

    public GameObject confirmSign;

    public GameObject confirmText;

    public GameObject UXFRig;
    public GameObject UXFPanel;

    private bool move;
    private bool delete;

    public bool allValid;

    public Color confirmValidColor;

    public Color confirmInvalidColor;

    public Texture confirmOriginalTexture;

    public Texture confirmInvalidTexture;

    public Color whiteColour;

    private void Start() {
        move = false;
        delete = false;

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
        }
        else
        {
            deleteSign.SetActive(false);
            confirmSign.SetActive(false);
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

    public void createPanel()
    {
        GameObject newPanel = (GameObject) PrefabUtility.InstantiatePrefab(PanelPrefab, this.transform);
        newPanel.transform.localScale = new Vector3(1,1,1);
        newPanel.transform.SetSiblingIndex((this.transform.childCount)-2);

        panelGroup.Add(newPanel.GetComponent<PanelObject>());

        enforceMove("enforce");
        newPanel.GetComponent<PanelObject>().setDelete(delete);
        enforceTitle();
    }

    public void createPositionalPanel(string prefabName, int pos)
    {
        GameObject newPanel = (GameObject) PrefabUtility.InstantiatePrefab(PanelPrefab, this.transform);
        newPanel.transform.localScale = new Vector3(1,1,1);
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
        foreach (PanelObject obj in panelGroup){
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
            UXFPanel.SetActive(true);
            UXFRig.SetActive(true);
        }
    }
}

}
