using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCam : MonoBehaviour
{
    public Transform Target;
    public GameObject Baloon;

    private bool _isPerspective;
    private Camera _cam;
    private Vector3 _distance;

	// Use this for initialization
	void Start ()
	{
	    _cam = GetComponent<Camera>();
	    _isPerspective = !_cam.orthographic;
        _distance = new Vector3(0f, 0f, (transform.position - Target.position).magnitude);
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.P))
	    {
	        _isPerspective = !_isPerspective;
	        _cam.orthographic = !_isPerspective;
	    }

	    if (Input.GetKeyDown(KeyCode.B))
	    {
	        var clone = Instantiate(Baloon, transform.position, transform.rotation);
	        clone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * 10f);
	    }

        transform.Rotate(0f, -Input.GetAxis("Horizontal"), 0f, Space.World);
        transform.Rotate(Input.GetAxis("Vertical"), 0f, 0f, Space.Self);
	    transform.position = Target.position - transform.rotation * _distance;

	    _distance.z -= Input.GetAxis("Mouse ScrollWheel") * 5f;
	}
}
