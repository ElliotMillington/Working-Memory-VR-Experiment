using UnityEngine.UI;
using UnityEngine;

public class SliderStart : MonoBehaviour
{
    public Text handleValue;

    void Start()
    {
        Slider sliserObj = this.gameObject.GetComponent<Slider>();
        handleValue.text = sliserObj.value.ToString();
    }

}
