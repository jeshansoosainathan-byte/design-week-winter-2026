using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MainMenuScript : MonoBehaviour
{

    [SerializeField] GameObject defaultSelected;
        

   public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);


    }

    public void QuitGame()
    {

        Debug.Log("Exiting game...");
        Application.Quit();

    }

    public void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelected);
        }
    }


}
