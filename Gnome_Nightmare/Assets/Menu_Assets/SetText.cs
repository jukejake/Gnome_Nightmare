using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour {

    public GameObject ToChange;

    public void SliderToText() {
        ToChange.GetComponent<Text>().text = this.GetComponent<Slider>().value.ToString();
    }
    public void SliderTimeLimit() {
        ToChange.GetComponent<Text>().text = (this.GetComponent<Slider>().value.ToString() + " MIN");
    }
    public void SliderScoreLimit() {
        ToChange.GetComponent<Text>().text = (this.GetComponent<Slider>().value.ToString() + " Points");
    }
    public void SliderVolumeLimit() {
        ToChange.GetComponent<Text>().text = (this.GetComponent<Slider>().value.ToString() + " Vol");
    }
}
