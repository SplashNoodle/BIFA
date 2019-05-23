using UnityEngine;

[CreateAssetMenu(fileName = "New Global Settings", menuName = "BIFA2018/Global Settings")]
public class GlobalSettings : ScriptableObject
{
    public enum SettingsMode
	{
		MasterVolume,
		MusicVolume,
		AmbienceVolume,
		InterfaceVolume,
		EffectsVolume,
		WindowMode,
		Resolution
	}

	public SettingsMode settings;

	[Range(0f,1f)]
	public float masterVolume, musicVolume, ambientVolume, interfaceVolume, effectsVolume;
}
