using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pins : MonoBehaviour {

	public Transform pin;
	private Rigidbody pin_rigidBody;
	private Vector3 position_pin;
	private AudioManager myAudioManager;
	private GameObject ball;
	public bool hit;
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		myAudioManager = GetComponent<AudioManager>();
		pin = transform;
		if (GetComponent<Rigidbody>() != null)
		{
			pin_rigidBody = GetComponent<Rigidbody>();
		}
		position_pin = pin.position;
		ball = GameObject.FindGameObjectWithTag("Ball");
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Ball")
        {
			if ((pin_has_fallen()==true)){
				float collisionSpeed = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
				float playVolume = collisionSpeed / 3 > 1 ? 1 : collisionSpeed / 3;
				if (collisionSpeed > 0)
				{
					myAudioManager.Play("Pin Heavy", playVolume);
					ball.GetComponent<AudioManager>().StopPlaying("Ball Roll");
				}
			}
		}
		if ((collision.gameObject.tag == "Pin") && (pin_has_fallen() == true))
		{
			float collisionSpeed = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
			float playVolume = collisionSpeed / 3 > 1 ? 1 : collisionSpeed / 3;
			if (collisionSpeed > 0)
			{
				myAudioManager.Play("Pin Light", playVolume);
				ball.GetComponent<AudioManager>().StopPlaying("Ball Roll");
			}

		}
	}
    private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.tag == "Ball")
		{
			hit = true;
			float collisionSpeed = other.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
			float playVolume = collisionSpeed / 3 > 1 ? 1 : collisionSpeed / 3;
			if (collisionSpeed > 0)
			{
				myAudioManager.Play("Pin Heavy", playVolume);
				ball.GetComponent<AudioManager>().StopPlaying("Ball Roll");
			}
		}
	}
    public void pins_Reset(){

		if (GetComponent<Rigidbody>() != null)
		{
			pin_rigidBody.angularVelocity = Vector3.zero;
			pin_rigidBody.velocity = Vector3.zero;
		}
		
		if (GetComponent<Animator>() != null)
        {
			GetComponent<Animator>().enabled = true;
			hit = false;
		}
		else
        {
			pin.position = position_pin;
			pin.rotation = Quaternion.identity;
		}
	}

	public bool pin_has_fallen()
	{
		if (GetComponent<Animator>() == null)
		{
			Vector3 pinsUpVector = pin.up;
			if (Vector3.Angle(pinsUpVector, Vector3.up) > 1f)
			{

				return true;

			}
			else
			{

				return false;
			}
		}
		else
		{
			return hit;
		}
	}
}
