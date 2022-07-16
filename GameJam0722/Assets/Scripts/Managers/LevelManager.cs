using System;
using System.Collections.Generic;
using Entities;
using Terrain;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;

        private void Awake()
        {
            if (instance is not null)
            {
                Destroy(this);
                return;
            }

            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        private void Start()
        {
            StartLevel();
        }

        public void StartLevel()
        {
            //LevelCreationManager.instance.LoadLevel();
            // GENERATE ENTITIES
            m_currentEntityIndex = 0;
            var timeline = new List<AbstractEntity>();
            timeline.Add(Character.instance);
            SetTimeline(timeline); // TODO ADD ENTITIES
            m_entities[m_currentEntityIndex].StartTurn();
        }

        #region Timeline
        
        public List<AbstractEntity> m_entities ;
        private int m_currentEntityIndex;

        public void SetTimeline(List<AbstractEntity> entities)
        {
            m_entities = new(entities);
            m_currentEntityIndex = 0;
        }

        public void EndTurn()
        {
            GetNextEntity().StartTurn();
        }
        
        private AbstractEntity GetNextEntity()
        {
            m_currentEntityIndex++;
            if (m_currentEntityIndex == m_entities.Count)
            {
                m_currentEntityIndex = 0;
            }

            return m_entities[m_currentEntityIndex];
        }
        
        #endregion

        #region End Game
        
        public void Victory()
        {
            UIManager.instance.Victory();
        }

        public void Defeat()
        {
            UIManager.instance.Defeat();
        }
        
        #endregion
        
    }
}
