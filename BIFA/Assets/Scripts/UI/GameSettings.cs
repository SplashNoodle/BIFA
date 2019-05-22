using UnityEngine;

[CreateAssetMenu(fileName = "New Game Settings", menuName = "BIFA2018/Game Settings")]
public class GameSettings : ScriptableObject
{
	public enum SettingsMode
	{
		Type,
		Value,
		Ball,
		Events,
		Objects,
		Validation
	}

	public SettingsMode settingsMode;
	public enum GameType
	{
		Score,
		Time
	}

	public GameType gameType;

	public int value;

	public enum BallType
	{
		Foot,
		Basket,
		Bowling
	}

	public BallType ballType;

	public bool events, objects;
}
