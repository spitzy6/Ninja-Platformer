using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

public class SyncedCameraController : MonoBehaviour
{
	public AudioSource m_audio;
	private Camera m_camera;
	private float[] samples;
	private int length;
	private float currentDB;
	private float lastDB;
	public float maxDBChange;
	public float minDBChange;
	public float baseDB;
	private float baseFOV;
    private float goalFOV;
    private float cameraFOVShift;
	private float timeLeftAttack;
	private float timeLeftDecay;
    private float initTimeLeftAttack;
	private float initTimeLeftDecay;

	void Start ()
	{
		m_camera = GetComponent<Camera> ();
		samples = new float[128];
		length = 128;
        initTimeLeftAttack = 0.05f;
        initTimeLeftDecay = 0.34f;
        baseFOV = 3.9f;
		m_camera.fieldOfView = baseFOV;
		currentDB = -1 * Mathf.Infinity;
	}

	// Update is called once per frame
	void Update ()
	{
        Debug.Log("FOV: " + m_camera.fieldOfView);
		m_audio.GetOutputData (samples, 0);
		lastDB = currentDB;
		currentDB = AudioPower.ComputeDB (samples, 0, ref length);
		if (currentDB - lastDB > minDBChange && currentDB - lastDB < maxDBChange && timeLeftAttack <= Time.deltaTime && timeLeftDecay <= Time.deltaTime)
		{
			// Debug.Log(Mathf.Abs(currentDB - lastDB));
			cameraFOVShift = 0.25f * Mathf.Abs (Mathf.Pow (-1 * baseDB / currentDB, 2.0f));
            m_camera.fieldOfView = baseFOV;
            goalFOV = baseFOV * cameraFOVShift;
			Debug.Log("Goal: " + goalFOV);
			timeLeftAttack = initTimeLeftAttack;
			timeLeftDecay = initTimeLeftDecay;
		}
		if (timeLeftAttack >= 0)
		{
			m_camera.fieldOfView += cameraFOVShift / (initTimeLeftAttack / Time.deltaTime);
			timeLeftAttack -= Time.deltaTime;
		}
		if (timeLeftAttack <= Time.deltaTime && timeLeftDecay >= 0)
		{
			m_camera.fieldOfView -= cameraFOVShift / (initTimeLeftDecay / Time.deltaTime);
			timeLeftDecay -= Time.deltaTime;
		}
	}
}