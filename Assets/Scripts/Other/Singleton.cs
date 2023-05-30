using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static bool _isApplicationQuit = false;

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_isApplicationQuit)
                return null;

            if(_instance == null)
            {
                //Try to find an existing type of object in the scene
                _instance = FindObjectOfType<T>();
                //If we did not find that object in the scene
                if(_instance == null)
                {
                    //Create a new gameobject of that type 
                    GameObject obj = new GameObject();
                    //and rename it according to its type
                    obj.name = typeof(T).Name;
                    //and add that type as a component and assign it as our _instance
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        //if there is no _instance yet,
        if(_instance == null)
        {
            //assign ourselves as the _instance
            _instance = this as T;
            //prevent this object from being destroyed
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //if there is already an _instance
            Destroy(this.gameObject);
        }

    }

    private void OnDestroy()
    {
        _isApplicationQuit = true;
    }
}
