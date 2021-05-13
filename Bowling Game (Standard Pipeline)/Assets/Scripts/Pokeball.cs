using UnityEngine;
using System.Collections;

public class Pokeball : MonoBehaviour
{
	[SerializeField]
	private float throwSpeed = 20f;
	private float speed;
	private float lastMouseX, lastMouseY;

	[SerializeField]
	public bool thrown, holding, curve;

	private Rigidbody _rigidbody;
	private Vector3 newPosition;

	[SerializeField]
	private float curveAmount = 0f, curveSpeed = 26, minCurveAmountToCurveBall = 0, maxCurveAmount = 4;
	private Rect circlingBox;

	//Additional Variables
	[SerializeField]
	private float speedVarianceFactor = .02f, verticalThrowFactor = .2f, horizontalThrowAngle = 20,
		curveThreshold = 400, bendThreshold = 10, curveDecayRate = 1.05f, minSpeedToThrow = .4f,
		movableZoneDivisor = 1.5f, ballToFingerSpeed = 30f, maxAngVelFactor = 20f, curveSpeedDefault = 1.6f,
		curveDecayRateDefault = 1.05f, respawnTime = 4, maxAngVelFactorDefault = 20, movableZoneX = 12,
		screenSidleSpeed = 1f, maxCurveAmountDefault = 4, changeDirSpeed = 1, changeDirThresh = 0, angVeloChecker = 1;

	//private Vector3 lastBallPos;

	public bool hitPin;


	//rules variables
	public int round;
	public Transform ball;
	public Pins[] pins;

	//audio
	private AudioManager myAudioManager;

	private void Awake()
	{
		pins = FindObjectsOfType<Pins>();
	}

	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();


		_rigidbody.maxAngularVelocity = curveAmount * 8f;
		/*circlingBoxX = circlingBox.x;
		circlingBoxY = circlingBox.y;*/
		myAudioManager = GetComponent<AudioManager>();

		myAudioManager.Play("Bowling Ambience");
		myAudioManager.StopPlaying("Ball Roll");
		Reset();
	}

	void Update()
	{

		if (_rigidbody.angularVelocity.magnitude < angVeloChecker)
		{
			circlingBox = new Rect(Screen.width / 2, Screen.height / 2, 0f, 0f);
		}



		if (holding)
			OnTouch();

		curve = (Mathf.Abs(curveAmount) > minCurveAmountToCurveBall);

		if (curve && thrown)
		{
			Vector3 direction = transform.right.normalized;

			_rigidbody.AddForce(direction * curveAmount * Time.deltaTime, ForceMode.Impulse);
		}

		if (thrown == false)
		{
			_rigidbody.maxAngularVelocity = curveAmount * maxAngVelFactor;
			_rigidbody.angularVelocity = transform.forward * curveAmount * 8f + _rigidbody.angularVelocity;
		}
		else
		{
			_rigidbody.maxAngularVelocity = 100000;
			return;
		}

		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100f))
			{
				if (hit.transform == transform)
				{
					holding = true;
					transform.SetParent(null);
				}
			}
		}

		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			if (lastMouseY < Input.GetTouch(0).position.y)
			{
				ThrowBall(Input.GetTouch(0).position);
			}
			else
			{
				Invoke("Reset", 0f);
			}
		}

		if (Input.touchCount == 1)
		{
			lastMouseX = Input.GetTouch(0).position.x;
			lastMouseY = Input.GetTouch(0).position.y;

			if (lastMouseX < circlingBox.x)
				circlingBox.x = lastMouseX;
			if (lastMouseX > circlingBox.xMax)
				circlingBox.xMax = lastMouseX;
			if (lastMouseY < circlingBox.y)
				circlingBox.y = lastMouseY;
			if (lastMouseY > circlingBox.yMax)
				circlingBox.yMax = lastMouseY;

			//circlingBox = new Rect(Screen.width / 2, Screen.height / 2, 0f, 0f);
		}
	}

	void Reset()
	{
		circlingBox = new Rect(Screen.width / 2, Screen.height / 2, 0f, 0f);

		curveAmount = 0f;
		CancelInvoke();
		GameObject.Find("Main Camera").GetComponent<Camera>().enabled = true;
		transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, Camera.main.nearClipPlane * 7.5f));
		newPosition = transform.position;
		thrown = holding = false;

		_rigidbody.useGravity = false;
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;
		_rigidbody.Sleep();

		transform.SetParent(Camera.main.transform);
		transform.localRotation = Quaternion.Euler(0f, 200f, 0f);
		missed = false;

		curveDecayRate = curveDecayRateDefault;

		curveSpeed = curveSpeedDefault;

		//rules reset
		increase_round();
		FindObjectOfType<CountPinsSingle>().GetComponent<CountPinsSingle>().roundCompleted();

		//sounds
		myAudioManager.StopPlaying("Ball Roll");

		//reset trail
		transform.GetChild(2).GetComponent<TrailRenderer>().enabled = false;

		//replay mode
		hitPin = false;
		foreach (TimeBody replay in FindObjectsOfType<TimeBody>())
		{
			replay.StopReplay();
			

		}


	}

	void OnTouch()
	{
		if (Input.touchCount > 0)
		{
			Vector3 mousePos = Input.GetTouch(0).position;

			if (mousePos.y <= Screen.height / movableZoneDivisor)
			{
				CalcCurveAmount();

				mousePos.z = Camera.main.nearClipPlane * 7.5f;

				Vector3 screenWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

				newPosition = screenWorldPos;

				transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, ballToFingerSpeed * Time.deltaTime);
			}
			if (holding)
			{
				if (mousePos.x <= Screen.width / movableZoneX)
				{
					transform.position += Vector3.left * screenSidleSpeed * .01f;
					GameObject.FindGameObjectWithTag("MainCamera").transform.position += Vector3.left * screenSidleSpeed * .01f;
				}
				else if (mousePos.x >= (Screen.width - (Screen.width / movableZoneX)))
				{
					transform.position += Vector3.right * screenSidleSpeed * .01f;
					GameObject.FindGameObjectWithTag("MainCamera").transform.position += Vector3.right * screenSidleSpeed * .01f;
				}
			}
		}
	}

	void CalcCurveAmount()
	{
		if (Input.touchCount > 0)
		{
			Vector2 b = new Vector2(lastMouseX, lastMouseY);
			Vector2 c = Input.GetTouch(0).position;
			Vector2 a = circlingBox.center;
			float swipeSpeed = Mathf.Abs(Input.GetTouch(0).deltaPosition.magnitude) / 100;

			if (b == c)
				return;

			float leftRightVal = ((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x));
			//bool isLeft = leftRightVal > 0; //a = mid, b = last, c = now
			/*
						var currBallPos = transform.position;
						if (currBallPos != lastBallPos)
						{
							maxCurveAmount = maxCurveAmountDefault * (currBallPos - lastBallPos).magnitude*200;
						}
						lastBallPos = currBallPos;

						*/

			curveSpeed = curveSpeedDefault * swipeSpeed;
			float bend = DistanceToLine(b, a, c);


			if (leftRightVal > curveThreshold && bend > bendThreshold)
			{
				if (curveAmount > .1f)
				{
					curveAmount -= Time.deltaTime * curveSpeed * changeDirSpeed * Mathf.Pow(Mathf.Abs(curveAmount - changeDirThresh), 1);
				}
				curveAmount -= Time.deltaTime * curveSpeed;
			}
			else if (leftRightVal < -curveThreshold && bend > bendThreshold)
			{
				if (curveAmount < -.1f)
				{
					curveAmount += Time.deltaTime * curveSpeed * changeDirSpeed * Mathf.Pow(Mathf.Abs(curveAmount + changeDirThresh), 1);
				}
				curveAmount += Time.deltaTime * curveSpeed;
			}
			else
			{
				curveAmount = curveAmount / curveDecayRate;
			}

			curveAmount = Mathf.Clamp(curveAmount, -maxCurveAmount, maxCurveAmount);

		}
	}

	void ThrowBall(Vector2 mousePos)
	{
		myAudioManager.Play("Throw");

		float differenceY = (mousePos.y - lastMouseY) / Screen.height * 100;

		if (differenceY < minSpeedToThrow)
		{
			Invoke("Reset", 0f);
			return;
		}


		_rigidbody.useGravity = true;
		speed = throwSpeed + throwSpeed * differenceY * speedVarianceFactor;

		float x = (mousePos.x - lastMouseX) / Screen.width;

		Vector3 direction = Quaternion.AngleAxis(x * horizontalThrowAngle, Vector3.up * .5f) * new Vector3(0f, 1f * verticalThrowFactor, 3f);
		direction = Camera.main.transform.TransformDirection(direction);

		_rigidbody.AddForce(direction * speed);

		holding = false;
		thrown = true;

		curveDecayRate = 0;

		curveSpeed = 0;

		//Activate Trail

		transform.GetChild(2).GetComponent<TrailRenderer>().enabled = true;

		/*
		foreach(TimeBody replay in FindObjectsOfType<TimeBody>())
        {
			replay.Re();
		}*/

		//stop replay
		foreach (TimeBody replay in FindObjectsOfType<TimeBody>())
		{
			Invoke("Reset",10f);

		}
	}


	bool missed = false;

	void OnCollisionEnter(Collision collision)
	{

		if (collision.transform.tag == "Ground" && !missed)
		{
			float yVel = Mathf.Abs(_rigidbody.velocity.y);
			myAudioManager.Play("Hit Ground", yVel > 1 ? 1 : yVel);
			myAudioManager.Play("Ball Roll", .75f);
		}

		if (collision.transform.tag == "Pin" && !missed)
		{
			if (hitPin == false)
			{
				hitPin = true;
				//replay
				foreach (TimeBody replay in FindObjectsOfType<TimeBody>())
				{
					replay.Invoke("StartReplay", 2);
				}


				Invoke("Reset", 6);


			}
		}

	}

	float DistanceToLine(Vector2 a, Vector2 b, Vector2 c)
	{
		float lengthSquared = Mathf.Pow(Vector2.Distance(b, c), 2);
		if (lengthSquared == 0.0) return Vector2.Distance(a, b);
		float t = Mathf.Max(0, Mathf.Min(1, Vector2.Dot(a - b, c - b) / lengthSquared));
		Vector2 projection = b + t * (c - b);
		return Vector2.Distance(a, projection);
	}

	//Rules functions -----------------------------------------------------------------

	public int round_number()
	{

		return round;

	}

	public void increase_round()
	{
		round++;
	}

	public void Resets()
	{
		if (round % 2 == 0)
		{

			for (int i = 0; i < 10; i++)
			{
				if (pins[i].pin_has_fallen())
				{
					pins[i].gameObject.SetActive(false);
				}
			}
		}
		else
		{
			for (int i = 0; i < 10; i++)
			{
				if (pins[i].pin_has_fallen())
				{
					pins[i].gameObject.SetActive(true);
				}
			}
			for (int i = 0; i < 10; i++)
			{

				pins[i].pins_Reset();

			}

		}
	}



}