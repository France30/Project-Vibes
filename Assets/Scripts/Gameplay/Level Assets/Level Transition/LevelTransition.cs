using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelTransition : MonoBehaviour
{
	[SerializeField] private int _transitionToLevel = 1;
	[SerializeField] private string _playerSpawnAreaId;
	[SerializeField] private bool _saveOnTransition = false;


	private void Awake()
	{
		GetComponent<BoxCollider2D>().isTrigger = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.GetComponent<Player>())
		{
			Time.timeScale = 0;
			LevelManager.Instance.AddLevel(_transitionToLevel);
			if (_saveOnTransition)
			{
				SaveSystem.ClearCheckpointData();
			}
			LevelManager.Instance.LoadLevelTransition(_transitionToLevel, _playerSpawnAreaId, _saveOnTransition);
		}
	}
}
