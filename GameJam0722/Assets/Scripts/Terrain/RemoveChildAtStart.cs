using UnityEngine;

public class RemoveChildAtStart : MonoBehaviour
{
    private void Awake() {
        foreach (Transform child in GetComponentsInChildren<Transform>()) {
            if(transform != child) Destroy(child.gameObject);
        }
    }
}
