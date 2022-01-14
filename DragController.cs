using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public Draggable LastDragged => _lastDragged;  
   private bool _isDragActive = false;

    private Vector2 _screenPosition;
    private Vector3 _worldPosition;

    private Draggable _lastDragged;


    void Awake()
    {
        // Makes sure that there is only 1 DragController in the scene
        DragController[] controllers = FindObjectsOfType<DragController>();
        if(controllers.Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(_isDragActive && (Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        {
            Drop();
            return; 
        }

        if(Input.touchCount > 0)
        {
            _screenPosition = Input.GetTouch(0).position; 
        }
        else
        {
            return; 
        }

        // Convert position on screen to world position
        _worldPosition = Camera.main.ScreenToWorldPoint(_screenPosition);

        if (_isDragActive)
        {
            Drag(); 
        }
        else
        {
            // What is on the screen at the current touched position
            RaycastHit2D hit = Physics2D.Raycast(_worldPosition, Vector2.zero);
            if(hit.collider != null)
            {
                // Only works if your item has a collider!
                Draggable draggable = hit.transform.gameObject.GetComponent<Draggable>(); 

                if(draggable != null)
                {
                    _lastDragged = draggable;
                    InitDrag(); 
                }
            }
        }
    }

    void InitDrag()
    {
        _lastDragged.LastPosition = _lastDragged.transform.position;
        UpdateDragStatus(true);    
    }

    void Drag()
    {
        // Update dragged objects position
        _lastDragged.transform.position = new Vector2(_worldPosition.x, _worldPosition.y);
    }

    void Drop()
    {
        UpdateDragStatus(false); 
    }
    
    void UpdateDragStatus(bool isDragging)
    {
        _isDragActive = _lastDragged.IsDragging = isDragging;
        _lastDragged.gameObject.layer = isDragging ? Layer.Dragging : Layer.Default;
    } 
}
