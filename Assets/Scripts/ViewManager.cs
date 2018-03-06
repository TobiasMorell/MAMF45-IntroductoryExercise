using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    [SerializeField] private bool _firstPersonMode;
    [SerializeField] private GameObject _firstPersonPrefab;
    [SerializeField] private GameObject _orbitCam;
    public KeyCode SwitchViewKeyCode;

    private GameObject _cameraObject;
    [SerializeField] private Vector3 _otherCamStartingPosition;
    [SerializeField] private Quaternion _otherCamStartingRotation;

	// Use this for initialization
	void Start ()
	{
	    _cameraObject = InstantiateCamera();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(SwitchViewKeyCode))
	    {
	        _firstPersonMode = !_firstPersonMode;

	        var loggedPosition = _otherCamStartingPosition;
	        var loggedRotation = _otherCamStartingRotation;

	        _otherCamStartingPosition = _cameraObject.transform.position;
	        _otherCamStartingRotation = _cameraObject.transform.rotation;

            Destroy(_cameraObject);

	        _cameraObject = InstantiateCamera(loggedPosition, loggedRotation);
	    }
	}

    private GameObject InstantiateCamera(Vector3? loggedPosition = null, Quaternion? loggedRotation = null)
    {
        var cam = Instantiate(_firstPersonMode ? _firstPersonPrefab : _orbitCam);
        if (loggedPosition!= null)
        {
            cam.transform.position = (Vector3) loggedPosition;
            cam.transform.rotation = (Quaternion) loggedRotation;
        }

        return cam;
    }
}
