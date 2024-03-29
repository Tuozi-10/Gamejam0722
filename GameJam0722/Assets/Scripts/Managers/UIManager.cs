﻿using System;
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
        [SerializeField] private TextMeshProUGUI textToTurnNeeded = null;
        private Material fadeMat;
        [Space]
        [SerializeField] private CanvasGroup cvgTitle;
        [SerializeField] private TMP_Text txtTitle;
        [Space, SerializeField] private float m_fadeMaterialCubeSpeed = 0.5f;

        [SerializeField] private RectTransform menu;
        [SerializeField] private RectTransform bottom;
        
        
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
            //m_cvgTurn.DOFade(1, 0.25f);
            //m_cvgTurn.DOFade(0, 0.3f).SetDelay( m_durationTextTurn);
        }
        
        public void ShowRandomCanvas() {
            randomTerrain.DOKill();
            randomTerrain.DOFade(1, 0.5f).SetDelay(2.5f);
        }
        
        public void HideRandomCanvas() {
            randomTerrain.DOKill();
            randomTerrain.DOFade(0, 0.5f);
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

        public void SetTextToTurnNeeded(int numberNeeded) {
            int textToShowForTurn = 3 - numberNeeded;
            textToTurnNeeded.text = textToShowForTurn.ToString();
        }

        /// <summary>
        /// switch between two colors for tiles
        /// </summary>
        /// <param name="id"></param>
        private void FadeColorAnimation(int id, bool fade)
        {
            List<DiceTerrain> diceTerrainList = new List<DiceTerrain>(TerrainManager.instance.GetDiceWithSameValue(id == 0? TerrainManager.instance.RandomWallDice : TerrainManager.instance.RandomHoleDice));
            
            if (diceTerrainList.Count == 0) return;
            
            if (fade) {
                try
                {

                    fadeMat = new Material(diceColorData.DiceMaterialData[diceTerrainList[0].diceData.diceValue]);
                    fadeMat.DOColor(diceColorData.DiceColorLightData[diceTerrainList[0].diceData.diceValue - 1],
                        m_fadeMaterialCubeSpeed).SetLoops(-1, LoopType.Yoyo);
                }
                catch
                {           }
                
            }
            else fadeMat.DOKill();

            foreach (DiceTerrain dice in diceTerrainList) {
                dice.ObjectMesh.sharedMaterial = fade ? fadeMat : diceColorData.DiceMaterialData[dice.diceData.diceValue];
            }
        }

        public void FadeDice(int id) => FadeColorAnimation(id, true);
        public void UnFadeDice(int id) => FadeColorAnimation(id, false);

        public void DisplayMenu()
        {
            if (menu.anchoredPosition.x < 350)
            {
                menu.DOKill();
                menu.DOAnchorPosX(500, 0.35f).SetEase(Ease.OutBack);
            }
            else
            {
                menu.DOKill();
                menu.DOAnchorPosX(0, 0.35f).SetEase(Ease.InBack);   
            }
        }

        public void ScaleUpBottom()
        {
            bottom.DOKill();
            bottom.DOAnchorPosY(40, 0.35f);
            bottom.DOScale(1.3f, 0.35f);
        }

        public void ScaleDownBottom()
        {
            bottom.DOKill();
            bottom.DOAnchorPosY(0, 0.35f);
            bottom.DOScale(1f, 0.35f);
        }

        public void RestartLevel() => LevelManager.instance.ReloadLevel();
        public void LoadLevel(int id) => LevelManager.instance.LoadLevel(id);
        public void Quit() => Application.Quit();

    }
}