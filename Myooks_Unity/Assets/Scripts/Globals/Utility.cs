using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Utility
{
	public static IEnumerator scaler(GameObject toScale, float startScale, float endScale, float endTime, UnityAction onComplete)
	{
		float timer = 0;
		while(timer/endTime < 1)
		{
			float newScale = Mathf.Lerp(startScale, endScale, timer/endTime);
			toScale.transform.localScale = new Vector3(newScale, newScale, newScale);
			yield return null;			
		}
		onComplete.Invoke();
	}
}
