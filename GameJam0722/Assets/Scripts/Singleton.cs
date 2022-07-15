using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this as T;
            Init();
        }
        else Destroy(this.gameObject);
    }

    /// <summary>
    /// Method called when this is initialized
    /// </summary>
    protected virtual void Init() {
    }
}
