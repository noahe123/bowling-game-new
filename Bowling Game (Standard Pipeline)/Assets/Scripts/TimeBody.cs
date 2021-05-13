using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{

	//bool isRewinding = false;

	bool isReplaying = false;

	public float recordTime = 3f;

	List<PointInTime> pointsInTime;

	Rigidbody rb;

	private GameObject replayCam;
	private GameObject mainCam;

	//audio tracking variables
	public AudioManager audioManager;
	public string audioName;
	public float audioVol;
	public bool audioPlay;
	public bool audioStop;

	// Use this for initialization
	void Start()
	{
		recordTime = 4f;
		pointsInTime = new List<PointInTime>();
		rb = GetComponent<Rigidbody>();

		replayCam = GameObject.Find("Replay Cam");
		mainCam = GameObject.Find("Main Camera");
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void FixedUpdate()
	{
		/*
		if (isRewinding)
			Rewind();
		else
			Record();*/
		
		if (isReplaying)
			Replay();
		else
			Record();
	}
	/*
		void Rewind()
		{
			if (pointsInTime.Count > 0)
			{
				PointInTime pointInTime = pointsInTime[0];
				transform.position = pointInTime.position;
				transform.rotation = pointInTime.rotation;
				pointsInTime.RemoveAt(0);
			}
			else
			{
				StopRewind();
			}

		}*/


	void Replay()
	{
		if (pointsInTime.Count > 0)
		{
			PointInTime pointInTime = pointsInTime[pointsInTime.Count - 1];
			transform.position = pointInTime.position;
			transform.rotation = pointInTime.rotation;
			if (pointInTime.audioManager != null)
			{
				if (pointInTime.audioPlay)
				{
					pointInTime.audioManager.Play(pointInTime.audioName, pointInTime.audioVol);
				}
				if (pointInTime.audioStop)
				{
					pointInTime.audioManager.StopPlaying(pointInTime.audioName);
				}
			}
			pointsInTime.RemoveAt(pointsInTime.Count - 1);
		}
		else
		{
			StopReplay();
		}
	}

	void Record()
	{
		if (pointsInTime.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
		{
			pointsInTime.RemoveAt(pointsInTime.Count - 1);
		}
		pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, null, null, 0, false, false));

		//reset audio vars to defaults
		audioManager = null;
		audioName = null;
		audioVol = 0;
		audioPlay = false;
		audioStop = false;
	}

	/*
	public void StartRewind()
	{
		isRewinding = true;
		rb.isKinematic = true;
	}

	public void StopRewind()
	{
		isRewinding = false;
		rb.isKinematic = false;
	}*/

	
	public void StartReplay()
	{
		isReplaying = true;
		GetComponent<Rigidbody>().isKinematic = true;
		replayCam.GetComponent<Camera>().enabled = true;
		replayCam.GetComponent<AudioListener>().enabled = true;
		mainCam.GetComponent<Camera>().enabled = false;
		mainCam.GetComponent<AudioListener>().enabled = false;
	}

	public void StopReplay()
	{
		isReplaying = false;
		GetComponent<Rigidbody>().isKinematic = false;
		replayCam.GetComponent<Camera>().enabled = false;
		replayCam.GetComponent<AudioListener>().enabled = false;
		mainCam.GetComponent<Camera>().enabled = true;
		mainCam.GetComponent<AudioListener>().enabled = true;
	}
}