using System.Collections.Generic;
using DG.Tweening;
using Entities;
using UnityEngine;

namespace Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        private List<AbstractEntity> m_entities;
        public List<AbstractEntity> Entities => m_entities;

        
        [SerializeField] private List<LevelSO> levelList = new List<LevelSO>();
        public List<LevelSO> LevelList => levelList;
        private int levelIndex = 0;
        public int LevelIndex => levelIndex;
        
        
        private int m_currentEntityIndex;
        private bool isLoadingNewLevel = false;
        public bool IsLoadingLevel => isLoadingNewLevel;

        public int collectibleCollected;
        
        private void Start() {
            UIManager.instance.ShowRandomCanvas();
            GenerateNewLevel();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.LeftAlt)) {
                LoadNextLevel();
            }
        }

        #region Level Loader
        /// <summary>
        /// Reload the same level
        /// </summary>
        public void ReloadLevel() 
        {
            CleanLevel();
            StartCoroutine(LevelCreationManager.instance.DestroyActuallevel(levelList[levelIndex-1]));
            isLoadingNewLevel = true;
            Character.instance.transform.DOLocalMove(new Vector3(-25, 0, -25), 2.5f);

            collectibleCollected = 0;
        }

        /// <summary>
        /// Load the next level on the list
        /// </summary>
        public void LoadNextLevel()
        {
            CleanLevel();
            StartCoroutine(LevelCreationManager.instance.DestroyActuallevel(levelList[levelIndex]));
            isLoadingNewLevel = true;
            Character.instance.transform.DOLocalMove(new Vector3(-25, 0, -25), 2.5f);
            levelIndex++;
            collectibleCollected = 0;
        }
        #endregion Level Loader

        public void CleanLevel()
        {
            foreach (var entity in m_entities)
            {
                if(entity is Character) continue;
                entity.Pivot.DOLocalMoveY(1, 0.225f).OnComplete(()=>entity.Pivot.DOLocalMoveY(-1.5f, 0.35f));
                entity.transform.DOScale(1.25f, 0.225f).OnComplete(() => entity.transform.DOScale(0, 0.25f).OnComplete(
                    () =>
                    {
                        LevelManager.instance.RemoveEntity(LevelManager.instance.Entities.IndexOf(entity));
                    }));
            }
        }
        
        /// <summary>
        /// Generate a new level
        /// </summary>
        public void GenerateNewLevel() {
            LevelCreationManager.instance.LoadLevel(levelList[levelIndex]);
            isLoadingNewLevel = true;
            levelIndex++;
        }
        
        /// <summary>
        /// Get entities for the level and initialize them
        /// </summary>
        public void GenerateEntities() {
            m_currentEntityIndex = 0;
            var timeline = new List<AbstractEntity>();
            timeline.Add(Character.instance);

            foreach (AbstractEntity ent in TerrainManager.instance.enemyEntity) {
                timeline.Add(ent);
            }
            
            SetTimeline(timeline); // TODO ADD ENTITIES
            m_entities[m_currentEntityIndex].StartTurn();
            
            isLoadingNewLevel = false;
        }

        public void RemoveEntity(int index) => m_entities.RemoveAt(index);
        public void RemoveEntity(AbstractEntity entity) => m_entities.Remove(entity);
        
        #region Timeline
        /// <summary>
        /// Set the timeline in order for the entity to move
        /// </summary>
        /// <param name="entities"></param>
        private void SetTimeline(List<AbstractEntity> entities)
        {
            m_entities = new List<AbstractEntity>(entities);
            m_currentEntityIndex = 0;
        }

        /// <summary>
        /// End the turn of an entity
        /// </summary>
        public void EndTurn() 
        {
            if (isLoadingNewLevel) return;
            if(m_currentEntityIndex == m_entities.Count - 1) TerrainManager.instance.CheckIfChange();
            else GetNextEntity()?.StartTurn();
        }
        
        /// <summary>
        /// Get the next entity in the list
        /// </summary>
        /// <returns></returns>
        public AbstractEntity GetNextEntity()
        {
            m_currentEntityIndex++;
            if (m_currentEntityIndex >= m_entities.Count)
            {
                m_currentEntityIndex = 0;
            }
            return m_entities[m_currentEntityIndex];
        }
        
        #endregion

        #region End Game
        
        /// <summary>
        /// If the player win
        /// </summary>
        public void Victory()
        {
            if (m_entities[0] is Character)
            {
                UIManager.instance.Victory();
                LevelManager.instance.LoadNextLevel();
            }
        }

        /// <summary>
        /// If the player die
        /// </summary>
        public void Defeat() => UIManager.instance.Defeat();

        #endregion

        public LevelSO GetActivScene() => levelList[levelIndex - 1];

    }
}
