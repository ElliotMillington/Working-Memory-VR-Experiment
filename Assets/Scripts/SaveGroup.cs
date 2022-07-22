using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SaveGroup : MonoBehaviour
{
    public GameObject SavePrefab;

    public void createSavePanel()
    {
        GameObject newPanel = (GameObject) PrefabUtility.InstantiatePrefab(SavePrefab, this.transform);
        newPanel.transform.SetSiblingIndex((this.transform.childCount)-2);
    }
}
