using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(Canvas))]
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        [SerializeField] private CanvasGroup m_canvasTransition;
        [SerializeField] private CanvasGroup m_cvgTurn;
        [SerializeField] private TMP_Text m_textTurn;
        [SerializeField] private float m_durationTextTurn = 1.25f;
        
        private void Awake()
        {
            if (instance is not null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        public void Victory()
        {
            
        }

        public void Defeat()
        {
            
        }

        public void AskTransition(float initialDelay= 0f,Action callback = null)
        {
            m_canvasTransition.DOKill();
            
            m_canvasTransition.interactable = m_canvasTransition.blocksRaycasts = true;
            
            m_canvasTransition.DOFade(1, 0.25f).SetDelay(initialDelay).OnComplete(()=>
            {
                m_canvasTransition.interactable = m_canvasTransition.blocksRaycasts = false;
                callback?.Invoke();
            });

            m_canvasTransition.DOFade(0, 0.5f).SetDelay(initialDelay + 0.25f);
        }
        
        public void AskTurn(string text)
        {
            m_cvgTurn.DOKill();

            m_textTurn.text = text;
            m_cvgTurn.DOFade(1, 0.25f);
            m_cvgTurn.DOFade(0, 0.3f).SetDelay( m_durationTextTurn);
        }
        
    }
}