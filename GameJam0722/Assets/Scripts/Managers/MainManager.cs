using System;
using Terrain;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MainManager : Singleton<MainManager>
    {
        [SerializeField] private float durationLoading = 1f;
        
        private const string MainSceneName = "MainScene";

        private void Start()
        {
           // CACA();
            UIManager.instance.AskTransition(durationLoading, StartGame);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(MainSceneName);
        }

        public void CACA()
        {
            bool[,] caca = new bool[10,10];
            caca[5, 5] = true;
            caca[2, 3] = true;
            caca[4, 4] = true;
            caca[4, 2] = true;
            caca[4, 3] = true;
            caca[3, 5] = true;
            
            Pathfinder.GetPath(2, 2, 8, 8, caca);
        }
        
    }
}
