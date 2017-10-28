using UnityEngine;

public class ButtonManager : MonoBehaviour {
    
    public void SinglePlayerButton(string newGame) { /*SceneManager.LoadScene(newGame);*/ }
    public void MultiPlayerButton() { }
    public void SettingsButton() { }
    public void CreditButton() { }
    public void QuitButton() { Application.Quit(); }
}
