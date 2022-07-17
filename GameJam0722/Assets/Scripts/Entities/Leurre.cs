using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Entities
{
    public class Leurre : MonoBehaviour
    {
        public static List<Leurre> leurres = new List<Leurre>();

        public int radiusDetection = 3;
        
        public static void UpdateLeurres()
        {
            for (var i = leurres.Count - 1; i >= 0; i--)
            {
                var leurre = leurres[i];
                leurre.StartTurn();
            }
        }
        
        public int turnsDuration = 1;
        public (int pox, int poxy) pos;

        public void Init(int x, int y)
        {
            leurres.Add(this);
            transform.DOKill();
            transform.DORotate(new Vector3(0,360,0), 0.4f, RotateMode.FastBeyond360);
            transform.localScale = Vector3.zero;
            transform.DOScale(1.2f,0.25f).OnComplete(()=> transform.DOScale(1,0.15f));
            pos = (x, y);
        }
        
        public void StartTurn()
        {
            turnsDuration--;
            if(turnsDuration <= 0) Die();
        }

        public void Die()
        {
            transform.DOKill();
            transform.DORotate(new Vector3(0,180,0), 0.425f,  RotateMode.FastBeyond360);
            transform.DOScale(0,0.3f).SetEase(Ease.InBack).OnComplete(()=> Destroy(gameObject));
            leurres.Remove(this);
        }
    }
}