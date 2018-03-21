using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Builder : MonoBehaviour
{
    public Material CorrectPlacementMaterial;
    public Material WrongPlacementMaterial;

    private bool _isBuilding = false;
    [SerializeField]
    private float BuildingDistance = 2f;

    private bool _instanceOverlappingOtherCollider = false;

    private Furniture _furniture;
    private GameObject _furnitureInstance;
    private Material _originalMaterial;
    private MeshRenderer _instanceMeshRenderer;
	
	// Update is called once per frame
	void Update ()
	{
	    if (!_isBuilding)
	        return;
	    if (Input.GetMouseButtonUp(1))
	        CancelBuilding();

	    BuildingDistance += Input.GetAxis("Mouse ScrollWheel") * 2f;
        _furnitureInstance.transform.position = transform.position + Vector3.forward * BuildingDistance;

	    if (_instanceOverlappingOtherCollider)
	    {
	        _instanceMeshRenderer.material = WrongPlacementMaterial;
	        return;
	    }
        if (!_furniture.SnapsToFloor && !_furniture.SnapsToWall)
            _instanceMeshRenderer.material = CorrectPlacementMaterial;
        else if (_furniture.SnapsToWall)
            CheckWallPlacement();
        else if (_furniture.SnapsToFloor)
            CheckFloorPlacement();

	    if (Input.GetMouseButtonUp(0))
            ConfirmBuilding();
	}

    private void CheckWallPlacement()
    {

    }

    private void CheckFloorPlacement()
    {

    }

    public void BeginBuilding(Furniture furniture)
    {
        _isBuilding = true;
        _furnitureInstance = Instantiate(furniture.Model);
        _instanceMeshRenderer = _furnitureInstance.GetComponent<MeshRenderer>();
        _originalMaterial = _instanceMeshRenderer.material;
        SetCollidersToTrigger(true);
        var bct = _furnitureInstance.AddComponent<BuildingColliderTrigger>();
        bct.OnColliderOverlapChanged += (source, args) => _instanceOverlappingOtherCollider = args.IsOverlappingColliders;
    }

    private void SetCollidersToTrigger(bool isTrigger)
    {
        foreach (var col in _furnitureInstance.GetComponentsInChildren<Collider>())
        {
            col.isTrigger = isTrigger;
        }
    }

    public void CancelBuilding()
    {
        _isBuilding = false;
        Destroy(_furnitureInstance);
        _furniture = null;
    }

    public void ConfirmBuilding()
    {
        _isBuilding = false;
        _instanceMeshRenderer.material = _originalMaterial;
        SetCollidersToTrigger(false);
        Destroy(_furnitureInstance.GetComponent<BuildingColliderTrigger>());
        _furnitureInstance = null;
        _instanceMeshRenderer = null;
        _originalMaterial = null;
        BuildingDistance = 2f;
    }
}
