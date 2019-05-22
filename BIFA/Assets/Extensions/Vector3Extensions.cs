#region System & Unity
using System.Collections;
using UnityEngine;
#endregion

public static class Vector3Extensions{

	/// <summary>
	/// Calculates a direction from two positions.
	/// </summary>
	/// <returns>The direction from two positions.</returns>
	/// <param name="startPos">Start position of the wanted direction.</param>
	/// <param name="endPos">End position of the wanted direction.</param>
    /// <param name="normalized">If true, direction will be normalized.</param>
	public static Vector3 CalculateDir(this Vector3 v3, Vector3 startPos, Vector3 endPos, bool normalized){
		Vector3 dir = endPos - startPos;
        if (normalized)
            dir.Normalize();
		return dir;
	}
}
