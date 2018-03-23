using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using System;
using System.Linq;
using Assets.Scripts;

public enum SelectionMode
{
    Move, Delete, Inspect, None
}

public class Builder : MonoBehaviour
{
    private class MeshRendererHelper
    {
        public MeshRenderer Renderer { get; private set; }
        public Material OriginalMaterial { get; private set; }

        public MeshRendererHelper(MeshRenderer renderer, Material originalMaterial)
        {
            Renderer = renderer;
            OriginalMaterial = originalMaterial;
        }
    }

    public Material CorrectPlacementMaterial;
    public Material WrongPlacementMaterial;

    private bool _isBuilding = false;
    [SerializeField]
    private float BuildingDistance = 2f;

    private bool _instanceOverlappingOtherCollider = false;

    private Furniture _furniture;
    private GameObject _furnitureInstance;
    private List<MeshRendererHelper> _meshRenderers;
    [SerializeField] private LineRenderer SelectionLine;

    public static Builder Instance {
        get
        {
            return _instance;
        }
    }
    private static Builder _instance;
    private SelectionMode _selection = SelectionMode.None;
    public GameObject FurnitureDisplayPrefab;

    private void SetMeshTo(Material material)
    {
        foreach (var mrh in _meshRenderers)
        {
            mrh.Renderer.material = material;
        }
    }
    private void SetMeshToDefault()
    {
        foreach (var mrh in _meshRenderers)
        {
            mrh.Renderer.material = mrh.OriginalMaterial;
        }
    }

    void Start()
    {
        if (_instance != null)
            throw new InvalidOperationException(
                "The builder script is singleton, please make sure only one is assigned!");
        _instance = this;
        SelectionLine = GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    BuildingDistance += Input.GetAxis("Mouse ScrollWheel") * 2f;

        if (_selection != SelectionMode.None)
	    {
	        HandleSelection();
	        return;
	    }
	    if (!_isBuilding)
	        return;
	    if (Input.GetMouseButtonUp(1))
	    {
	        CancelBuilding();
	        return;
	    }

        _furnitureInstance.transform.position = transform.position + transform.forward * BuildingDistance;

	    if (_instanceOverlappingOtherCollider)
	    {
	        SetMeshTo(WrongPlacementMaterial);
	        return;
	    }

	    var canPlace = false;
	    if (!_furniture.SnapsToFloor && !_furniture.SnapsToWall)
	    {
	        canPlace = true;
	        SetMeshTo(CorrectPlacementMaterial);
	    }
        else if (_furniture.SnapsToWall)
            canPlace = CheckWallPlacement();
        else if (_furniture.SnapsToFloor)
            canPlace = CheckFloorPlacement();

	    if (canPlace && Input.GetMouseButtonUp(0))
            ConfirmBuilding();
	}

    private void HandleSelection()
    {
        if (Input.GetMouseButtonUp(1))
        {
            SelectionLine.enabled = false;
            _selection = SelectionMode.None;
            return;
        }

        RaycastHit hit;
        Vector3 lineEnd;
        SelectionLine.endColor = _selection.GetColor();
        if (!Physics.Raycast(transform.position, transform.forward, out hit, BuildingDistance))
            lineEnd = transform.position + (transform.forward * BuildingDistance);
        else
        {
            lineEnd = hit.point;
            if (hit.collider.CompareTag("Furniture") && Input.GetMouseButtonUp(0))
            {
                SelectionLine.enabled = false;
                ExecuteSelectionAction(hit.collider);
            }
        }
        SelectionLine.SetPositions(new []
        {
            transform.position + transform.up * -.2f,
            lineEnd
        });
    }

    private void ExecuteSelectionAction(Collider target)
    {
        switch (_selection)
        {
            case SelectionMode.Move:
                PickUp(target.gameObject);
                break;
            case SelectionMode.Delete:
                Destroy(target.gameObject);
                break;
            case SelectionMode.Inspect:
                var canvas = GameObject.Find("Canvas");
                var fd = Instantiate(FurnitureDisplayPrefab, canvas.transform);
                fd.GetComponent<RectTransform>().anchoredPosition = new Vector2(800, -384);
                fd.AddComponent<Inspector>();
                var fb = fd.GetComponent<FurnitureButton>();
                fb.DisplayProduct(target.GetComponent<InteriorComponent>().Furniture);
                fb.FurnitureImage.enabled = false;
                fb.FurnitureDetails.SetActive(true);
                break;
            case SelectionMode.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _selection = SelectionMode.None;
    }

    private bool CheckWallPlacement()
    {
        RaycastHit rayHit;
        if (!Physics.Raycast(transform.position, transform.forward, out rayHit, BuildingDistance, 1 << 8))
        {
            SetMeshTo(WrongPlacementMaterial);
            return false;
        }
        if (rayHit.point.y > 0)
        {
            _furnitureInstance.transform.position = rayHit.point;
            _furnitureInstance.transform.rotation = rayHit.collider.transform.rotation;
            _furnitureInstance.transform.Rotate(Vector3.right, 90);
            //_furnitureInstance.transform.Rotate(Vector3.up, 180);
            SetMeshTo(CorrectPlacementMaterial);
            return true;
        }
        else
        {
            SetMeshTo(WrongPlacementMaterial);
            return false;
        }
    }

    private bool CheckFloorPlacement()
    {
        RaycastHit rayHit;
        if (!Physics.Raycast(transform.position, transform.forward, out rayHit, BuildingDistance, 1 << 8))
        {
            SetMeshTo(WrongPlacementMaterial);
            return false;
        }
        if (rayHit.point.y <= 0.5)
        {
            _furnitureInstance.transform.position = rayHit.point;
            SetMeshTo(CorrectPlacementMaterial);
            return true;
        }
        else
        {
            SetMeshTo(WrongPlacementMaterial);
            return false;
        }
    }

    private void StoreFurnitureDeatils(Furniture furniture)
    {
        _furniture = furniture;
        _meshRenderers = _furnitureInstance.GetComponentsInChildren<MeshRenderer>().Select(mr => new MeshRendererHelper(mr, mr.material)).ToList();
        SetCollidersToTrigger(true);
        var bct = _furnitureInstance.AddComponent<BuildingColliderTrigger>();
        bct.OnColliderOverlapChanged += (source, args) => _instanceOverlappingOtherCollider = args.IsOverlappingColliders;
        GetComponent<InteractableDetector>().enabled = false;
        _isBuilding = true;
    }

    public void BeginBuilding(Furniture furniture)
    {
        _furnitureInstance = Instantiate(furniture.Model, null);
        _furnitureInstance.tag = "Furniture";
        
        StoreFurnitureDeatils(furniture);
    }

    public void PickUp(GameObject furniture)
    {
        _furnitureInstance = furniture;
        var fc = furniture.GetComponent<InteriorComponent>();
        if (!fc) return;
        StoreFurnitureDeatils(fc.Furniture);
        //TODO: Movement cancellation should probably snap the furniture back to where it came from
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
        _furnitureInstance.AddComponent<InteriorComponent>().Furniture = _furniture;
        Debug.Log("Confirming");
        SetCollidersToTrigger(false);
        _isBuilding = false;
        SetMeshToDefault();
        Destroy(_furnitureInstance.GetComponent<BuildingColliderTrigger>());
        _furnitureInstance = null;
        _meshRenderers = null;
        BuildingDistance = 2f;
    }

    public void BeginSelection(SelectionMode mode)
    {
        _selection = mode;
        SelectionLine.enabled = true;
    }
}
