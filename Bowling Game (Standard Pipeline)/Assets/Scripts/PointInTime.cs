using UnityEngine;

public class PointInTime
{
	public Vector3 position;
	public Quaternion rotation;
	public AudioManager audioManager;
	public string audioName;
	public float audioVol;
	public bool audioPlay;
	public bool audioStop;

	public PointInTime(Vector3 _position, Quaternion _rotation, AudioManager _audioManager, string _audioName, float _audioVol, bool _audioPlay, bool _audioStop)
	{
		position = _position;
		rotation = _rotation;
		audioManager = _audioManager;
		audioName = _audioName;
		audioVol = _audioVol;
		audioPlay = _audioPlay;
		audioStop = _audioStop;
	}

}