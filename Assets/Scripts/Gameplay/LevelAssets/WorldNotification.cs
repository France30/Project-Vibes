using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public class WorldNotification : MonoBehaviour
{
    [SerializeField] private string _worldName;

    private TextMeshProUGUI _worldTextNotif;
    private Player _player;

    private static bool _didNotificationTrigger = false;


    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnLevelLoad += ResetNotificationFlag;
    }

    private void OnDisable()
    {
        if (LevelManager.Instance == null) return;

        LevelManager.Instance.OnLevelLoad -= ResetNotificationFlag;
    }

    private void Update()
    {
        if (_worldTextNotif == null) return;

        if(_player != null)
        {
            GameUIManager.Instance.FadeInNotificationText();
        }

        if(_player == null)
        {
            if(_worldTextNotif.alpha <= 0f)
            {
                _worldTextNotif = null;
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

            _worldTextNotif = GameUIManager.Instance.TextNotif;
            _worldTextNotif.text = _worldName;
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
}
