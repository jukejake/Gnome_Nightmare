using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour {

    public GameObject dropDown;
    public List<string> ExcludedIfContains;

    private void Start() {
        LoadScenesToDropDown(dropDown);
    }

    public void LoadSceneFromString(string newGame) { SceneManager.LoadScene(newGame); }

    public void LoadSceneFromDropDown(GameObject dropDown) {
        int SceneNum = dropDown.GetComponent<Dropdown>().value;
        //Debug.Log(dropDown.GetComponent<Dropdown>().options[SceneNum].text);
        GameObject.Find("GameTimer").GetComponent<GameTimer>().TurnOn();
        SceneManager.LoadScene(dropDown.GetComponent<Dropdown>().options[SceneNum].text);
    }

    public void LoadScenesToDropDown(GameObject dropDown) {
        dropDown.GetComponent<Dropdown>().ClearOptions();
        List<string> SceneNames = new List<string>();
        //Debug.Log(SceneManager.sceneCountInBuildSettings);
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++){
            var path = SceneUtility.GetScenePathByBuildIndex(i);
            var sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);

            for (int j = 0; j < ExcludedIfContains.Count; j++) {
                if (sceneName.Contains(ExcludedIfContains[j])) { }
                else { SceneNames.Add(sceneName); }
            }
            if (ExcludedIfContains.Count == 0) { SceneNames.Add(sceneName); }

            //Debug.Log(sceneName);
        }
        dropDown.GetComponent<Dropdown>().AddOptions(SceneNames);
    }
}
