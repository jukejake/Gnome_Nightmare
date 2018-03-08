using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class ID_Table : SerializedMonoBehaviour {

    public static ID_Table instance;
    private void Awake() { instance = this; DontDestroyOnLoad(this.gameObject); }

    [OdinSerialize]
    public Dictionary<int, GameObject> IDTable;
}