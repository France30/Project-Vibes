using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New EnemyPermaDeath SO", menuName = "PersistentData/EnemyPermaDeath" )]
public class EnemyPermaDeathSO : ScriptableObject
{
	[SerializeField] private string _id = "0";

	private ObjectWithPersistentData _enemyPermaDeathVariant = ObjectWithPersistentData.enemy;
	private bool _isAlive = true;
	private GateEvent _gateEvent;


	public void InitializeEnemyPermaDeath(EnemyBase enemy, GateEvent gateEvent)
	{
		if(gateEvent != null)
			_gateEvent = gateEvent;

		_isAlive = SavePersistentData.LoadPersistentFlag(_enemyPermaDeathVariant, _id);
		enemy.gameObject.SetActive(_isAlive);

		if (enemy.gameObject.activeSelf)
			enemy.OnEnemyDeath += EnablePermaDeath;
		else
			_gateEvent?.OpenGateImmediately();
	}

	private void EnablePermaDeath()
	{
		_isAlive = false;
		_gateEvent?.PlayOpenGateAnim();
	}

	public void SavePermaDeathState()
    {
		SavePersistentData.SavePersistentFlag(_enemyPermaDeathVariant, _id, _isAlive);
	}

	private void OnEnable()
	{
#if UNITY_EDITOR
		EditorApplication.playModeStateChanged += ResetFlag;
#endif
	}

	private void OnDisable()
	{
#if UNITY_EDITOR
		EditorApplication.playModeStateChanged -= ResetFlag;
#endif
	}

#if UNITY_EDITOR
	private void ResetFlag(PlayModeStateChange playModeStateChange)
	{
		if (playModeStateChange != PlayModeStateChange.ExitingPlayMode) return;

		SavePersistentData.ClearPersistentFlagData(_enemyPermaDeathVariant, _id);
	}
#endif
}
