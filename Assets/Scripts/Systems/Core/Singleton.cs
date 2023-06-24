using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            // referencing blocker in the event where the instance is called while OnApplicationQuit
            if (_isApplicationQuit)
                return null;

            if (!_instance)
                _instance = FindObjectOfType<T>();

            if (!_instance)
            {
                Debug.Log((typeof(T)).Name);
                T instance = Resources.Load<T>("Managers/" + typeof(T).Name);
                _instance = Instantiate(instance);
            }

            return _instance;
        }
    }

    private static bool _isApplicationQuit = false;
    [SerializeField] protected bool _isPersist = false;


    protected virtual void Awake()
    {
        if (_instance == null) _instance = this as T;

        if (_instance != null)
        {
            if (_instance != this as T)
                Destroy(this.gameObject);
        }

        if (_isPersist) DontDestroyOnLoad(this.gameObject);
    }

    protected virtual void OnApplicationQuit()
    {
        _isApplicationQuit = true;
    }
}