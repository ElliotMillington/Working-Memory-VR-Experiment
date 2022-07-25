using UnityEngine;
using UnityEngine.UI;

public class SavePanel : MonoBehaviour
{
    [HideInInspector]
    public int position;

    public Text titleText;
    public Text bannerText;
    
    private void Start() {
        position = this.gameObject.transform.GetSiblingIndex() + 1;
        titleText.text = "Load " + position;
    }

}
