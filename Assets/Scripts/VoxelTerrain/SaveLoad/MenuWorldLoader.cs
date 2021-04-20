using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelTerrain.SaveLoad
{
    public class MenuWorldLoader : MonoBehaviour
    {
        [SerializeField] private Button _newGame;
        [SerializeField] private GameObject _playfabField;
        [SerializeField] private GameObject _input;
        [SerializeField] private List<TMP_Text> _buttonTexts;
        [SerializeField] private List<GameObject> _buttons;
        [SerializeField] private List<GameObject> _deleteButtons;
        
        private List<string> _directories = new List<string>();
        private string _worldName;
        private void OnEnable()
        {
            Refresh();
        }

        private void Refresh()
        {
            _directories.Clear();

            for (int i = 0; i < 5; i++)
            {
                _buttons[i].SetActive(false);
                _deleteButtons[i].SetActive(false);
            }
            
            var activeButtons = 0;
            
            var directories = Directory.GetDirectories(Application.persistentDataPath + "/" + "Worlds" + "/");

            foreach (var directory in directories)
            {
                var str = directory.Replace(Application.persistentDataPath + "/" + "Worlds" + "/", "");
                if (str == "MainMenu") continue;
                _directories.Add(str);
            }

            for (int i = 0; i < 5; i++)
            {
                if (_directories.Count == i) break;

                _buttonTexts[i].text = _directories[i];
                _buttons[i].SetActive(true);
                _deleteButtons[i].SetActive(true);
                activeButtons++;
            }

            if (activeButtons == 5)
            {
                _newGame.gameObject.SetActive(false);
                _input.SetActive(false);
            }
        }

        public void SetWorldName(TMP_InputField worldName) => _worldName = worldName.text;

        public void SetWorldName(TMP_Text worldName) => _worldName = worldName.text;

        public void SaveWorldName()
        {
            if (string.IsNullOrEmpty(_worldName)) return;
            
            var _chunkDirectory = Application.persistentDataPath + "/" + "Worlds" + "/" + _worldName + "/";

            if (!Directory.Exists(_chunkDirectory)) Directory.CreateDirectory(_chunkDirectory);

            var activeWorldDirectory = Application.persistentDataPath + "/" + "Active_World" + "/";
            
            if (!Directory.Exists(activeWorldDirectory)) Directory.CreateDirectory(activeWorldDirectory);
            
            var fullPath = activeWorldDirectory + "activeWorld" + ".json";

            File.WriteAllText(fullPath, _worldName);
            
            SetAllInactive();
        }

        private void SetAllInactive()
        {
            foreach (var button in _buttons)
            {
                button.SetActive(false);
            }
            
            _newGame.gameObject.SetActive(false);
            _input.SetActive(false);
            
            SetPlayfabActive();
        }

        private void SetPlayfabActive()
        {
            _playfabField.SetActive(true);
        }

        public void DeleteWorld(TMP_Text name)
        {
            var worldName = name.text;
            
            var _chunkDirectory = Application.persistentDataPath + "/" + "Worlds" + "/" + worldName + "/";

            if (Directory.Exists(_chunkDirectory)) Directory.Delete(_chunkDirectory);

            var activeWorldDirectory = Application.persistentDataPath + "/" + "Active_World" + "/";

            if (!Directory.Exists(activeWorldDirectory)) return;
            
            var fullPath = activeWorldDirectory + "activeWorld" + ".json";

            File.WriteAllText(fullPath, "");
            
            Refresh();
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}
