//Based off of a youtube video
//https://www.youtube.com/watch?v=e8GmfoaOB4Y
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {

    [SerializeField]
	private float baseValue;

    private List<float> modifiers = new List<float>();

    public float getValue() {
        float finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(float modifier) {
        if (modifier != 0.0f) { modifiers.Add(modifier); }
    }
    public void RemoveModifier(float modifier) {
        if (modifier != 0.0f) { modifiers.Remove(modifier); }
    }
}
