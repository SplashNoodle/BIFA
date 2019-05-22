#region System & Unity
using System.Collections;
using UnityEngine;
#endregion

public static class BoolExtensions
{
	/// <summary>
	/// Checks if a value is within a range, min and max excluded.
	/// </summary>
	/// <returns><c>true</c>, if exclusive range was checked, <c>false</c> otherwise.</returns>
	/// <param name="val">Value.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Maximum.</param>
	public static bool CheckExclusiveRange(this int val, int min, int max){
		return(val > min && val < max);
	}

	/// <summary>
	/// Checks if a value is within a range, min and max excluded.
	/// </summary>
	/// <returns><c>true</c>, if exclusive range was checked, <c>false</c> otherwise.</returns>
	/// <param name="val">Value.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Maximum.</param>
	public static bool CheckExclusiveRange(this float val, float min, float max){
		return(val > min && val < max);
	}

	/// <summary>
	/// Checks if a value is within a range, min and max included.
	/// </summary>
	/// <returns><c>true</c>, if inclusive range was checked, <c>false</c> otherwise.</returns>
	/// <param name="val">Value.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Maximum.</param>
	public static bool CheckInclusiveRange(this int val, int min, int max){
		return(val >= min && val <= max);
	}

	/// <summary>
	/// Checks if a value is within a range, min and max included.
	/// </summary>
	/// <returns><c>true</c>, if inclusive range was checked, <c>false</c> otherwise.</returns>
	/// <param name="val">Value.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Maximum.</param>
	public static bool CheckInclusiveRange(this float val, float min, float max){
		return(val >= min && val <= max);
	}
}

