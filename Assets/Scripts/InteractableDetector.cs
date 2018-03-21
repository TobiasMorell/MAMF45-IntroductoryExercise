using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private float _maxDetectionDistance;
    [SerializeField] private Text _descriptionText;
    public KeyCode InteractionButton;
    private IInteractable _latestHit;

    void Start()
    {
        _descriptionText = GameObject.FindGameObjectWithTag("InteractionText").GetComponent<Text>();
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
	    var hits = Physics.RaycastAll(ray, _maxDetectionDistance);

	    foreach (var raycastHit in hits)
	    {
	        var interactionScript = raycastHit.collider.GetComponentInParent<IInteractable>();
	        if (interactionScript == null) continue;

	        _latestHit = interactionScript;
	        _descriptionText.text = InteractionButton + ": " + interactionScript.Description;
	        _descriptionText.enabled = true;
	        return;
	    }

	    _descriptionText.enabled = false;
	    _latestHit = null;
	}
}
