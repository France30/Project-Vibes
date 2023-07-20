using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelTransition : MonoBehaviour
{
    [SerializeField] private int _transitionToLevel = 1;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>())
        {
            Time.timeScale = 0;
            LevelManager.Instance.LoadLevelSelect(_transitionToLevel);
            LevelManager.Instance.AddLevel(_transitionToLevel);

            SaveSystem.SaveUnlockedLevels();
        }
    }
}
