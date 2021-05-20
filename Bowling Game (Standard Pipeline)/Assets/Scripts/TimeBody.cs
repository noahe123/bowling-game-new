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

	private GameObject replayCamParent;
	private GameObject mainCam;
	private GameObject ball;

	//audio tracking variables
	public AudioManager audioManager;
	public string audioName;
	public float audioVol;
	public bool audioPlay;
	public bool audioStop;

	// Use this for initialization
	void Start()
	{
		ball = GameObject.FindGameObjectWithTag("Ball");
		recordTime = 2.5f;
		pointsInTime = new List<PointInTime>();
		if (GetComponent<Rigidbody>() != null)
		{
			rb = GetComponent<Rigidbody>();
		}

		replayCamParent = GameObject.Find("Replay Cam Parent");
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
		pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, audioManager, audioName, audioVol, audioPlay, audioStop));

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
		if (GetComponent<Rigidbody>() != null)
		{
			GetComponent<Rigidbody>().isKinematic = true;
		}
		replayCamParent.transform.GetChild(0).gameObject.SetActive(true);
		replayCamParent.transform.GetChild(1).gameObject.SetActive(true);
		mainCam.GetComponent<Camera>().enabled = false;
		mainCam.GetComponent<AudioListener>().enabled = false;
		FindObjectOfType<CountPinsSingle>().GetComponent<CountPinsSingle>().SetScoreText();

	}

	public void StopReplay()
	{
		isReplaying = false;
		if (GetComponent<Rigidbody>() != null)
		{
			GetComponent<Rigidbody>().isKinematic = false;
		}
		replayCamParent.transform.GetChild(0).gameObject.SetActive(false);
		replayCamParent.transform.GetChild(1).gameObject.SetActive(false);
		mainCam.GetComponent<Camera>().enabled = true;
		mainCam.GetComponent<AudioListener>().enabled = true;
	}
}