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

    [SerializeField]
    private List<float> modifiers = new List<float>();

    public float GetValue() {
        float finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public float GetModifierValue() {
        float finalValue = 0.0f;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void SquishValue() {
        float finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        modifiers.ForEach(x => RemoveModifier(x));
        AddModifier(finalValue);
    }

    public void AddModifier(float modifier) {
        if (modifier != 0.0f) { modifiers.Add(modifier); }
    }

    public void RemoveModifier(float modifier) {
        if (modifier != 0.0f) { modifiers.Remove(modifier); }
    }
}
