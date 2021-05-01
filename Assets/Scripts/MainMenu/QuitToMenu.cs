using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class QuitToMenu : MonoBehaviour
    {
        public void MenuQuit()
        {
            List<GameObject> ToDestroy = new List<GameObject>(); 
            var canvasSingleton = FindObjectsOfType<CanvasSingleton>();
            foreach (var canvas in canvasSingleton)
            {
                ToDestroy.Add(canvas.gameObject);
            }

            var singleton = FindObjectsOfType<Singleton>();
            foreach (var singleton1 in singleton)
            {
                ToDestroy.Add(singleton1.gameObject);
            }

            var manager = FindObjectOfType<Manager>();
            Destroy(manager.gameObject);

            for (int i = ToDestroy.Count - 1; i >= 0; i--)
            {
                var destroy = ToDestroy[i];
                ToDestroy.RemoveAt(i);
                Destroy(destroy);
            }
            
            SceneManager.LoadScene(0);
        }
    }
}
