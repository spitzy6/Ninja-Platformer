using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

public class SyncedColorChanger : MonoBehaviour
{
	public AudioSource m_audio;
	private Light m_light;
	private float[] samples;
	private int length;
	private float currentDB;
	private float lastDB;
	public float maxDBChange;
	public float minDBChange;
	public float baseDB;
	private float r;
	private float g;
	private float b;
	private float colorShift;
	private Color origColor;
	private Color goalColor;
	private float timeLeftAttack;
	private float timeLeftDecay;

	void Start ()
	{
		m_light = GetComponent<Light> ();
		samples = new float[128];
		length = 128;
		r = 0.02f;
		g = 0.1f;
		b = 0.25f;
		origColor = new Color(r, g, b, 0f);
		m_light.color = origColor;
		currentDB = -1 * Mathf.Infinity;
	}

	// Update is called once per frame
	void Update ()
	{
		m_audio.GetOutputData (samples, 0);
		lastDB = currentDB;
		currentDB = AudioPower.ComputeDB (samples, 0, ref length);
		if (currentDB - lastDB > minDBChange && currentDB - lastDB < maxDBChange && timeLeftAttack <= Time.deltaTime && timeLeftDecay <= Time.deltaTime)
		{
			// Debug.Log(Mathf.Abs(currentDB - lastDB));
			colorShift = Mathf.Abs (Mathf.Pow (-1 * baseDB / currentDB, 2.0f));
			goalColor = new Color (3 * colorShift * r, colorShift * g, colorShift * b, 0f);
			// Debug.Log(goalColor.ToString());
			timeLeftAttack = 0.05f;
			timeLeftDecay = 0.34f;
		}
		if (timeLeftAttack >= 0)
		{
			m_light.color = Color.Lerp (m_light.color, goalColor, Time.deltaTime / timeLeftAttack);
			timeLeftAttack -= Time.deltaTime;
		}
		if (timeLeftAttack <= Time.deltaTime && timeLeftDecay >= 0)
		{
			m_light.color = Color.Lerp (m_light.color, origColor, Time.deltaTime / timeLeftDecay);
			timeLeftDecay -= Time.deltaTime;
		}
	}
}