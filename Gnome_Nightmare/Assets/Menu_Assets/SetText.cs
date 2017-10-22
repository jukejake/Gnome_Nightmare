using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetText : MonoBehaviour {
    
    public void SliderToText(GameObject TextToChange) {
        TextToChange.GetComponent<Text>().text = this.GetComponent<Slider>().value.ToString();
    }
    public void SliderTimeLimit(GameObject TextToChange) {
        TextToChange.GetComponent<Text>().text = (this.GetComponent<Slider>().value.ToString() + " MIN");
        GameObject.Find("GameTimer").GetComponent<GameTimer>().SetTimeLimit(this.GetComponent<Slider>().value);
    }
    public void SliderScoreLimit(GameObject TextToChange) {
        TextToChange.GetComponent<Text>().text = (this.GetComponent<Slider>().value.ToString() + " Points");
    }
    public void SliderVolumeLimit(GameObject TextToChange) {
        TextToChange.GetComponent<Text>().text = (this.GetComponent<Slider>().value.ToString() + " Vol");
    }
}
