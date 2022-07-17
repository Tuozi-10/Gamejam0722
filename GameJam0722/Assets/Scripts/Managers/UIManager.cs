using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers
{
    [RequireComponent(typeof(Canvas))]
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private CanvasGroup m_canvasTransition;
        [SerializeField] private CanvasGroup m_cvgTurn;
        [SerializeField] private CanvasGroup randomTerrain;
        [Space]
        [SerializeField] private TMP_Text m_textTurn;
        [SerializeField] private float m_durationTextTurn = 1.25f;
        [Space] 
        [SerializeField] private Image upTerrainImage = null;
        [SerializeField] private RectTransform upTerrainParent = null;
        [SerializeField] private Image downTerrainImage = null;
        [SerializeField] private RectTransform downTerrainParent = null;
        [SerializeField] private DiceTerrainMaterialSO diceColorData = null;
        private Material fadeMat;
        [Space]
        [SerializeField] private CanvasGroup cvgTitle;
        [SerializeField] private TMP_Text txtTitle;
        public void Victory()
        {
            cvgTitle.DOKill();
            cvgTitle.DOFade(1, 0.35f);
            cvgTitle.DOFade(0, 0.35f).SetDelay(2f);
            txtTitle.text = "Victoire!";
        }

        public void Defeat()
        {
            cvgTitle.DOKill();
            cvgTitle.DOFade(1, 0.35f);
            cvgTitle.DOFade(0, 0.35f).SetDelay(2f);
            txtTitle.text = "Défaite...";

        }

        /// <summary>
        /// Make a transition
        /// </summary>
        /// <param name="initialDelay"></param>
        /// <param name="callback"></param>
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
        
        /// <summary>
        /// Show which entity turn it is
        /// </summary>
        /// <param name="text"></param>
        public void AskTurn(string text) {
            if (LevelManager.instance.IsLoadingLevel) return;
            m_cvgTurn.DOKill();

            m_textTurn.text = text;
            m_cvgTurn.DOFade(1, 0.25f);
            m_cvgTurn.DOFade(0, 0.3f).SetDelay( m_durationTextTurn);
        }
        
        public void StartNewIsland() {
            randomTerrain.DOFade(1, 0.5f).SetDelay(1);
        }

        public void SetRandomDiceUIColor() {
            upTerrainImage.color = diceColorData.DiceMaterialData[Random.Range(0, diceColorData.DiceMaterialData.Count - 1)].color;
            downTerrainImage.color = diceColorData.DiceMaterialData[Random.Range(0, diceColorData.DiceMaterialData.Count - 1)].color;
        }

        public void SetDiceUIColor(int colorIdUp, int colorIdDown) {
            upTerrainImage.color = diceColorData.DiceMaterialData[colorIdUp].color;
            downTerrainImage.color = diceColorData.DiceMaterialData[colorIdDown].color;

            upTerrainParent.DOScale(1.25f, 0.25f).OnComplete(() => upTerrainParent.DOScale(1f, 0.35f));
            downTerrainParent.DOScale(1.25f, 0.25f).OnComplete(() => downTerrainParent.DOScale(1f, 0.35f));
        }

        /// <summary>
        /// switch between two colors for tiles
        /// </summary>
        /// <param name="id"></param>
        private void FadeColorAnimation(int id, bool fade) {
            List<DiceTerrain> diceTerrainList = new List<DiceTerrain>(TerrainManager.instance.GetDiceWithSameValue(id == 0? TerrainManager.instance.RandomWallDice : TerrainManager.instance.RandomHoleDice));
            
            if (diceTerrainList.Count == 0) return;
            
            if (fade) {
                fadeMat = new Material(diceColorData.DiceMaterialData[diceTerrainList[0].diceData.diceValue]);
                fadeMat.DOColor(diceColorData.DiceColorLightData[diceTerrainList[0].diceData.diceValue - 1], 1.25f).SetLoops(-1, LoopType.Yoyo);
            }
            else fadeMat.DOKill();

            foreach (DiceTerrain dice in diceTerrainList) {
                dice.ObjectMesh.sharedMaterial = fade ? fadeMat : diceColorData.DiceMaterialData[dice.diceData.diceValue];
            }
        }

        public void FadeDice(int id) => FadeColorAnimation(id, true);
        public void UnFadeDice(int id) => FadeColorAnimation(id, false);
    }
}