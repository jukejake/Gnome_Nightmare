using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {
    
    public void SinglePlayerButton(string newGame) { /*SceneManager.LoadScene(newGame);*/ }
    public void MultiPlayerButton() { }
    public void SettingsButton() { }
    public void CreditButton() { }
    public void QuitButton() { Application.Quit(); }
}
