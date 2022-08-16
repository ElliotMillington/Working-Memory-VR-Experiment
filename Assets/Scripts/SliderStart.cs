using UnityEngine.UI;
using UnityEngine;

/*

    Assigned to all slider in the GUI panel to ensure the text on slider reflects the value of the slider

*/ 

public class SliderStart : MonoBehaviour
{
    public Text handleValue;

    void Start()
    {
        Slider sliserObj = this.gameObject.GetComponent<Slider>();
        handleValue.text = sliserObj.value.ToString();
    }

}
