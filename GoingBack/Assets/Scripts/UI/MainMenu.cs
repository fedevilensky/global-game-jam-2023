using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void StartButtonClicked()
   {
      SceneManager.LoadScene(1);
   }
   
   public void ExitButtonClicked()
   {
      Application.Quit();
   }
}
