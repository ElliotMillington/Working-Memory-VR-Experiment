using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PanelObject : MonoBehaviour
{

    public GameObject up;
    public GameObject down;

    public Transform panel;

    public string prefabType;

    private void Start() {
        PanelGroup script = this.transform.GetComponentInParent<PanelGroup>();
        prefabType = PrefabUtility.GetCorrespondingObjectFromSource(this).name;
    }
    
    void moveDown()
    {
        if (this.panel.parent.GetChildCount() != this.panel.GetSiblingIndex()+2)
        {
            int currentIndex = this.panel.GetSiblingIndex();
            int nextIndex = currentIndex+1;

            PanelObject replacedSibling = this.panel.parent.GetChild(nextIndex).GetComponent<PanelObject>();

            this.panel.SetSiblingIndex(nextIndex);

            this.checkButtons();
            replacedSibling.checkButtons();

            this.transform.parent.GetComponent<PanelGroup>().switchIndexes(currentIndex, nextIndex);
        }
    }

    public void moveUp()
    {
        if (this.panel.GetSiblingIndex() != 0)
        {
            int currentIndex = this.panel.GetSiblingIndex();
            int nextIndex = currentIndex-1;

            PanelObject replacedSibling = this.panel.parent.GetChild(nextIndex).GetComponent<PanelObject>();

            this.panel.SetSiblingIndex(nextIndex);

            this.checkButtons();
            replacedSibling.checkButtons();

            this.transform.parent.GetComponent<PanelGroup>().switchIndexes(currentIndex, nextIndex);
        }
    }

    public void checkButtons()
    {
        //if top
        if(this.panel.GetSiblingIndex() == 0)
        {
            this.up.SetActive(false);
        }else
        {
            this.up.SetActive(true);
        }

        //if the bottom
        if(this.panel.parent.GetChildCount() == this.panel.GetSiblingIndex()+2)
        {
            this.down.SetActive(false);
        }else
        {
            this.down.SetActive(true);
        }
    }

    public void negateMovement()
    {
        up.SetActive(false);
        down.SetActive(false);
    }

    public void invertDimension()
    {
        PanelGroup script = this.transform.GetComponentInParent<PanelGroup>();
        var prefabGameObject = PrefabUtility.GetCorrespondingObjectFromSource(this);
        
        string invertedPrefabType;
        if (prefabType == "TwoDimButton")
        {
            invertedPrefabType = "3D";
            script.createPositionalPanel(invertedPrefabType, this.transform.GetSiblingIndex());
        } 
        else
        {
            invertedPrefabType = "2D";
            script.createPositionalPanel(invertedPrefabType, this.transform.GetSiblingIndex());
        }
        Destroy(gameObject);
    }

    public string getType()
    {
        return prefabType;
    }
}
