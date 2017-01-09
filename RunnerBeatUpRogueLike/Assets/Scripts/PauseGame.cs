using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour {

    public GameObject pauseCanvas;
    public Sprite pauseButtonSprite, playButtonSprite;
    public Image pauseButton;
	
	public void PauseUnpause ()
    {
        if (!GameManager.instance.isPaused)
        {
            GameManager.instance.isPaused = true;
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            pauseButton.sprite = playButtonSprite;
        }
        else
        {
            GameManager.instance.isPaused = false;
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);
            pauseButton.sprite = pauseButtonSprite;
        }
    }    

    public void GoBackToTown()
    {
        PauseUnpause();
        PlayerInfo.instance.SetState(PlayerInfo.PlayerState.EnemiesAttackingTown);
    }

}
