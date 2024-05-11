using UnityEngine;

[CreateAssetMenu(fileName = "New Sprite Flash", menuName = "Configs/SpriteController")]
public class SpriteFlash : ScriptableObject
{
	[SerializeField] private float _flashSpeed;
	[SerializeField] private int _flashCount;
	[SerializeField] private Material _flashMaterial;

	public float FlashSpeed { get { return _flashSpeed; } }
	public int FlashCount { get { return _flashCount; } }
	public Material FlashMaterial { get { return _flashMaterial; } }
}
