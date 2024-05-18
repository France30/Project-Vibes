using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New EnemyPermaDeath SO", menuName = "PersistentData/EnemyPermaDeath" )]
public class EnemyPermaDeathSO : ScriptableObject
{
	[SerializeField] private string _id = "0";

	private ObjectWithPersistentData _enemyPermaDeathVariant = ObjectWithPersistentData.enemy;
	private bool _isAlive = true;


	public void InitializeEnemyPermaDeath(EnemyBase enemy)
	{
		_isAlive = SavePersistentData.LoadPersistentFlag(_enemyPermaDeathVariant, _id);
		enemy.gameObject.SetActive(_isAlive);

		if (enemy.gameObject.activeSelf)
			enemy.OnEnemyDeath += EnablePermaDeath;
	}

	private void EnablePermaDeath()
	{
		_isAlive = false;
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
