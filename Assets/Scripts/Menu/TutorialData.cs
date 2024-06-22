using UnityEngine;

[CreateAssetMenu(fileName = "New Tutorial Data", menuName = "UI/TutorialData")]
public class TutorialData : ScriptableObject
{
    public bool IsTutorialEnabled { get; private set; }

    private ObjectWithPersistentData _tutorial = ObjectWithPersistentData.tutorial;

	public void EnableTutorial(bool isEnable)
    {
		IsTutorialEnabled = isEnable;
		SavePersistentData.SavePersistentFlag(_tutorial, "0", IsTutorialEnabled);
	}

	public void Reset()
    {
		IsTutorialEnabled = false;
    }

	private void OnEnable()
	{
		if(SaveSystem.IsSaveFileFound())
			IsTutorialEnabled = SavePersistentData.LoadPersistentFlag(_tutorial,"0");
		else
			IsTutorialEnabled = false;
	}
}
