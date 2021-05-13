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

	// Use this for initialization
	void Start()
	{
		recordTime = 4f;
		pointsInTime = new List<PointInTime>();
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		/*
		if (Input.GetKeyDown(KeyCode.Return))
			StartRewind();
		if (Input.GetKeyUp(KeyCode.Return))
			StopRewind();*/
		
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
		pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
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
		GameObject.Find("Replay Cam").GetComponent<Camera>().enabled = true;
		GameObject.Find("Main Camera").GetComponent<Camera>().enabled = false;
	}

	public void StopReplay()
	{
		isReplaying = false;
		GetComponent<Rigidbody>().isKinematic = false;
		GameObject.Find("Replay Cam").GetComponent<Camera>().enabled = false;
		
	}
}