using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T instance = null;

    /// <summary>
    /// Initialize the instance
    /// </summary>
    private void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this as T;
            Init();
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Method called when this is initialized
    /// </summary>
    protected virtual void Init() {
    }
}