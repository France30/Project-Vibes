using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            // referencing blocker in the event where the instance is called while OnDestroy
            if (_isDestroyed)
                return null;

            if (!_instance)
                _instance = FindObjectOfType<T>();

            if (!_instance)
            {
                Debug.Log((typeof(T)).Name);
                T instance = Resources.Load<T>("System/" + (typeof(T)).Name);
                _instance = Instantiate(instance);
            }

            return _instance;
        }
    }

    private static bool _isDestroyed = false;

    [SerializeField] protected bool _isPersist = false;

    protected virtual void Awake()
    {
        _isDestroyed = false;

        if (_instance == null) _instance = this as T;

        if (_instance != null)
        {
            if (_instance != this as T)
                Destroy(this.gameObject);
        }

        if (_isPersist) DontDestroyOnLoad(this.gameObject);

    }

    protected virtual void OnDestroy()
    {
        if (_isPersist) return;

        _isDestroyed = true;
        _instance = null;
    }

}