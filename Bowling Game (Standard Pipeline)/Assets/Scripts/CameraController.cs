using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float fovDefault;
    Transform ballTransform;
    // Start is called before the first frame update
    void Start()
    {
        fovDefault = GetComponent<Camera>().fieldOfView;

    }

    // Update is called once per frame
    void Update()
    {
        ballTransform = FindObjectOfType<Pokeball>().transform;
        if (ballTransform.gameObject.GetComponent<Pokeball>().thrown == true)
        {
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView , fovDefault - Vector3.Distance(transform.position, ballTransform.position), Time.deltaTime*1);
        }
        else
        {
            GetComponent<Camera>().fieldOfView = fovDefault;
        }
    }
}
