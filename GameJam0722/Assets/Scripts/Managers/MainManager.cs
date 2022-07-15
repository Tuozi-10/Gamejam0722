using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MainManager : MonoBehaviour
    {
        public static MainManager instance;

        private const string MainSceneName = "MainScene";
        
        private void Awake()
        {
            if (instance is not null)
            {
                Destroy(this);
                return;
            }

            instance = this;
            StartGame();
        }

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            SceneManager.LoadScene(MainSceneName);
        }
        
    }
}
