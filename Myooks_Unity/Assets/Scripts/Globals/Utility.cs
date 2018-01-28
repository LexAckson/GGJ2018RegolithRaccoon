using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public static class Utility
{
	public static IEnumerator scaler(GameObject toScale, float startScale, float endScale, float endTime, UnityAction onComplete)
	{
		float timer = 0;
		while(timer/endTime < 1)
		{
			timer += Time.deltaTime;
			float newScale = Mathf.Lerp(startScale, endScale, timer/endTime);
			toScale.transform.localScale = new Vector3(newScale, newScale, newScale);
			yield return null;			
		}
		if(onComplete != null)
			onComplete.Invoke();
	}

	public static IEnumerable<T> GetValues<T>()
	{
		return Enum.GetValues(typeof(T)).Cast<T>();
	}

	public static T getRandomEnum<T>() where T: struct, IConvertible
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException("T must be an enumerated type");
		} 
		
		IEnumerable<T> enums = GetValues<T>(); 
		return enums.ElementAt(UnityEngine.Random.Range(0, enums.Count()));
	}

	public static TValue RandomValue<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        List<TValue> values = Enumerable.ToList(dict.Values);
        return values[UnityEngine.Random.Range(0, values.Count)];
    }

	public static TValue RandomValue<TValue>(IEnumerable<TValue> list)
    {
        List<TValue> values = Enumerable.ToList(list);
        return values[UnityEngine.Random.Range(0, values.Count)];
    }

	public static Vector3 offset(float xmin, float xmax, float ymin, float ymax)
	{
		return new Vector3(UnityEngine.Random.Range(xmin, xmax), 0, UnityEngine.Random.Range(ymin, ymax));
	}

}
