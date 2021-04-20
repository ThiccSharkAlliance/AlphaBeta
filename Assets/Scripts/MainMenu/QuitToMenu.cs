using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class QuitToMenu : MonoBehaviour
    {
        public void MenuQuit()
        {
            SceneManager.LoadScene(0);
        }
    }
}
