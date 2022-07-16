using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FishSpawner : MonoBehaviour {
    [SerializeField] private Transform leftTransform = null;
    [SerializeField] private Transform rightTransform = null;
    [Space]
    [SerializeField] private GameObject fishPrefab = null;
    [SerializeField] private float spawnDelay = 5;
    [SerializeField] private float fishSpeed = .5f;
    [SerializeField] private float fishDistance = 25f;

    private void Start() => StartCoroutine(SpawnFish());

    
    private IEnumerator SpawnFish() {
        while (true) {
            for (int i = 0; i < Random.Range(1,5); i++) {
                GameObject fish = Instantiate(fishPrefab, new Vector3(leftTransform.position.x, -2, Random.Range(rightTransform.position.z, leftTransform.position.z)), Quaternion.identity);
                fish.transform.DOLocalMove(new Vector3(fish.transform.position.x + fishDistance, transform.position.y, transform.position.z), Random.Range(fishSpeed - .25f, fishSpeed + .25f)).OnComplete(() => Destroy(fish));
                yield return new WaitForSeconds(.35f);
            }
            
            yield return new WaitForSeconds(Random.Range(spawnDelay, spawnDelay + 15));
        }
    }
    
    
    private void OnDrawGizmos() {
        Gizmos.DrawLine(leftTransform.position, rightTransform.position);
        Gizmos.DrawLine(new Vector3(leftTransform.position.x, leftTransform.position.y, (leftTransform.position.z + rightTransform.position.z) / 2), new Vector3(leftTransform.position.x + fishDistance, leftTransform.position.y, (leftTransform.position.z + rightTransform.position.z) / 2));
    }
}
