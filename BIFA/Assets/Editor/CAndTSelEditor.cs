using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharAndTeeSelection))]
[CanEditMultipleObjects]
public class CAndTSelEditor : Editor
{
	public override void OnInspectorGUI() {
		serializedObject.Update();
		CharAndTeeSelection s = (CharAndTeeSelection)target;

		GUILayout.Label("Character Infos", EditorStyles.boldLabel);
		s.pInfos = EditorGUILayout.ObjectField("Player Infos", s.pInfos, typeof(PInfos), true) as PInfos;
		s.mateInfos = EditorGUILayout.ObjectField("Mate Player Infos", s.mateInfos, typeof(PInfos), true) as PInfos; ;
		s.charImage = EditorGUILayout.ObjectField("Character Image", s.charImage, typeof(UnityEngine.UI.Image), true) as UnityEngine.UI.Image;
		s.tenueImage = EditorGUILayout.ObjectField("T-shirt Image", s.tenueImage, typeof(UnityEngine.UI.Image), true) as UnityEngine.UI.Image;

		GUILayout.Space(15);

		GUILayout.Label("Character UI", EditorStyles.boldLabel);
		s.pseudoDisplay = EditorGUILayout.ObjectField("Pseudo Displayer", s.pseudoDisplay, typeof(TMPro.TextMeshProUGUI), true) as TMPro.TextMeshProUGUI;
		s.flag = EditorGUILayout.ObjectField("Flag", s.flag, typeof(GameObject), true) as GameObject;
		s.ready = EditorGUILayout.ObjectField("Ready Icon", s.ready, typeof(GameObject), true) as GameObject;

		GUILayout.Space(15);
		EditorGUILayout.EnumPopup("Selection mode", s.SelMode);
		/*s.canvasAnim = EditorGUILayout.ObjectField("Canvas Animator", s.canvasAnim, typeof(Animator), true) as Animator;*/

		GUILayout.Space(15);

		GUILayout.Label("Options", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("chars"), new GUIContent("Characters"), true);
		s.SetMaxReadyCount = EditorGUILayout.IntField("Max Ready Count", s.SetMaxReadyCount);

		GUILayout.Space(15);
		GUILayout.Label("Selected options", EditorStyles.boldLabel);
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("colors"), new GUIContent("Character"), true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("charSprites"), new GUIContent("Character"), true);
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("colors"), new GUIContent("T-shirt"), true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("clothesSprites"), new GUIContent("T-shirt"), true);
		serializedObject.ApplyModifiedProperties();
	}
}
