using System.Collections.Generic;
using Managers;
using Terrain;

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
                    Destroy(cell.collectible);
                    LevelManager.instance.collectibleCollected++;
                }
            }
            
        }
    }
}