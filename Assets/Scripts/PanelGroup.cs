using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PanelGroup : MonoBehaviour
{
    
    private List<PanelObject> panelGroup;

    public GameObject PanelPrefab;
    public GameObject moveSign;
    public GameObject deleteSign;

    private bool move;
    private bool delete;

    private void Start() {
        move = false;
        delete = false;

        panelGroup = new List<PanelObject>(this.gameObject.GetComponentsInChildren<PanelObject>());
        this.enforceMove("enforce");
        enforceTitle();
    }

    private void Update()
    {
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
        }
        else
        {
            deleteSign.SetActive(false);
        }

    }

    public void confirm()
    {
        //turn group of panels into parsable information
        string ap = "";
        foreach(PanelObject x in panelGroup)
        {
            ap = ap + "|" + x.getDimension();
        }
        Debug.Log(ap);
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
        newPanel.transform.SetSiblingIndex((this.transform.GetChildCount())-2);

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
    }

}
