using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public class WorldNotification : MonoBehaviour
{
    [SerializeField] private string _worldName;

    private TextMeshProUGUI _worldTextUI;
    private Player _player;

    private static bool _didNotificationTrigger = false;


    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnLevelLoad += ResetNotificationFlag;
        LevelManager.Instance.OnLoadFromSave += DisableObjectsOfWorldNotification;
    }

    private void OnDisable()
    {
        if (LevelManager.Instance == null) return;

        LevelManager.Instance.OnLevelLoad -= ResetNotificationFlag;
        LevelManager.Instance.OnLoadFromSave -= DisableObjectsOfWorldNotification;
    }

    private void Update()
    {
        if (_worldTextUI == null) return;

        if(_player != null)
        {
            GameUIManager.Instance.FadeInNotificationText();
        }

        if(_player == null)
        {
            if(_worldTextUI.alpha <= 0f)
            {
                _worldTextUI = null;
                DisableObjectsOfWorldNotification();
                return;
            }

            GameUIManager.Instance.FadeOutNotificationText();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            if (_didNotificationTrigger) return;

            _didNotificationTrigger = true;

            _worldTextUI = GameUIManager.Instance.Notification;
            _worldTextUI.text = _worldName;
            _player = player;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            _player = null;
        }
    }

    private void ResetNotificationFlag()
    {
        _didNotificationTrigger = false;
    }

    private void DisableObjectsOfWorldNotification()
    {
        WorldNotification[] worldNotification = FindObjectsOfType<WorldNotification>();
        Utilities.DisableAllObjectsOfType<WorldNotification>(worldNotification);
    }
}
