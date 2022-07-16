using System;
using System.Collections.Generic;
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

        private void Start() => GenerateNewLevel();

        private void Update() {
            if (Input.GetKeyDown(KeyCode.LeftAlt)) {
                StartCoroutine(LevelCreationManager.instance.DestroyActuallevel(levelList[levelIndex]));
                isLoadingNewLevel = true;
                levelIndex++;
            }
        }


        /// <summary>
        /// Generate a new level
        /// </summary>
        public void GenerateNewLevel() {
            LevelCreationManager.instance.LoadLevel(levelList[levelIndex]);
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
        public void EndTurn() {
            if (isLoadingNewLevel) return;
            if(m_currentEntityIndex == m_entities.Count - 1) StartCoroutine(TerrainManager.instance.ChangeHeightEvent());
            else GetNextEntity().StartTurn();
        }
        
        /// <summary>
        /// Get the next entity in the list
        /// </summary>
        /// <returns></returns>
        public AbstractEntity GetNextEntity()
        {
            m_currentEntityIndex++;
            if (m_currentEntityIndex == m_entities.Count) {
                m_currentEntityIndex = 0;
            }

            return m_entities[m_currentEntityIndex];
        }
        
        #endregion

        
        #region End Game
        
        /// <summary>
        /// If the player win
        /// </summary>
        public void Victory() => UIManager.instance.Victory();

        /// <summary>
        /// If the player die
        /// </summary>
        public void Defeat() => UIManager.instance.Defeat();

        #endregion
        
    }
}
