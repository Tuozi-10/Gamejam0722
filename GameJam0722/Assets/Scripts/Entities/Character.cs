using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Terrain;
using UnityEngine;

namespace Entities
{
    public class Character : AbstractEntity
    {
        public static Character instance;

        public bool canPlay;
        
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

        public override void SetPath(List<Pathfinder.Node> path)
        {
            canPlay = false;
            base.SetPath(path);
        }

        public override void StartTurn()
        {
            Leurre.UpdateLeurres();
            base.StartTurn();
            if(UIManager.instance != null) UIManager.instance.AskTurn("Player Turn");
            canPlay = true;
        }

        public override void EndTurn()
        {
            base.EndTurn();

            if (TerrainManager.instance.diceTerrainlsit[pos.x, pos.y].diceData.diceEffectState == DiceEffectState.End)
            {
                LevelManager.instance.Victory();
            }

            var cells = TerrainManager.instance.diceTerrainlsit;
            foreach (var cell in cells)
            {
                if (cell.pos == pos && cell.diceData.diceEffectState == DiceEffectState.Collectible)
                {
                    cell.diceData.diceEffectState = DiceEffectState.None;

                    cell.collectible.transform.DOKill();
                    cell.collectible.transform.DOScale(1.25f, 0.35f);
                    cell.collectible.transform.DOScale(0f, 0.35f).SetDelay(0.35f);
                    cell.collectible.transform.DOLocalMoveY(3f, 0.55f);
                    cell.collectible.transform.DORotate(new Vector3(0, 480, 0), 0.7f, RotateMode.FastBeyond360);
                    
                    Destroy(cell.collectible, 0.95f);
                    
                    LevelManager.instance.collectibleCollected++;
                }
            }
            
        }
    }
}