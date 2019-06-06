using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bonus))]
public class BonusEditor : Editor
{
    public override void OnInspectorGUI() {
        serializedObject.Update();
        Bonus b = (Bonus)target;

		b.takeBonus = EditorGUILayout.ObjectField("Take Bonus Clip",b.takeBonus,typeof(AudioClip),true) as AudioClip;
		b.bonusSoundMaster = EditorGUILayout.ObjectField("Bonus Sound Master", b.bonusSoundMaster, typeof(AudioSource), true) as AudioSource; 

		b.defaultVolume = EditorGUILayout.FloatField("Default Volume", b.defaultVolume);


		GUILayout.Space(15f);

        b.BonusT = (Bonus.BonusType)EditorGUILayout.EnumPopup("Bonus type", b.BonusT);

        switch (b.BonusT) {
            case Bonus.BonusType.Aimant:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("magnets"), new GUIContent("Aimants"), true);
                EditorGUILayout.HelpBox("L'aimant s'active lorsque le joueur appuie sur la touche de bonus. La balle est attirée vers le but adverse.", MessageType.Info, true);
                break;
            case Bonus.BonusType.Eclair:
                EditorGUILayout.HelpBox("L'éclair s'active immédiatement. Le premier joueur touché par celui ayant le bonus est temporairement incapable de bouger.", MessageType.Info, true);
                break;
            case Bonus.BonusType.Flocon:
                EditorGUILayout.HelpBox("Le flocon s'active lorsque le joueur appuie sur la touche de bonus. Un mur de glace apparaît et protège le but pendant quelques secondes.", MessageType.Info, true);
                break;
            case Bonus.BonusType.Grappin:
                EditorGUILayout.HelpBox("Le grappin s'active lorsque le joueur appuie sur la touche de bonus. La balle est rapprochée du joueur.", MessageType.Info, true);                
                break;
            case Bonus.BonusType.Undefined:
                EditorGUILayout.HelpBox("Bonus non défini.", MessageType.Warning, true);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
