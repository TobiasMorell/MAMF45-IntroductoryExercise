using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private float _maxDetectionDistance;
    [SerializeField] private GameObject _descriptionText;
    public KeyCode InteractionButton;
    private IInteractable _latestHit;

    void Start()
    {
        _descriptionText = GameObject.FindGameObjectWithTag("InteractionText");
        _descriptionText.SetActive(false);
    }

	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetKeyDown(InteractionButton) && _latestHit != null)
	    {
            _latestHit.Interact();
	        return;
	    }
        
        var ray = new Ray(transform.position, transform.forward );
	    RaycastHit hit;

	    if (Physics.Raycast(ray, out hit, _maxDetectionDistance))
	    {
	        var interactionScript = hit.collider.GetComponentInParent<IInteractable>();
	        if (interactionScript != null)
	        {
	            _latestHit = interactionScript;
	            _descriptionText.GetComponent<Text>().text = InteractionButton + ": " + interactionScript.Description;
	            _descriptionText.SetActive(true);
	            return;
	        }
	    }

	    _descriptionText.SetActive(false);
	    _latestHit = null;
	}
}
