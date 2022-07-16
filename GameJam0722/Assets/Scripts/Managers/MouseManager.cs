using System.Collections.Generic;
using DG.Tweening;
using Entities;
using Terrain;
using UnityEngine;

public class MouseManager : Singleton<MouseManager> {
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private GameObject cubePosition = null;
    [Space] 
    [SerializeField] private LayerMask layerCheck;
    [SerializeField] private Transform player = null;
    [SerializeField] private float playerSpeed = 1;
    public float cursorHeight = .5f;
    [Space]
    [SerializeField] private Transform cubeUnderMouse = null;
    
    [SerializeField] private List<Vector3> targetPosition = new List<Vector3>();

    [SerializeField] private SpriteRenderer m_cursorRenderer;
    [SerializeField] private Sprite[] m_cursorSprites;
    
    private Character  character => Character.instance;
    
    private void Update()
    {
        UpdateCursor();
        UpdateMouseDown();
    }

    private void UpdateMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && character.canPlay)
        {
            cubeUnderMouse = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 500,layerCheck) ? hit.collider.transform : null;
         
            if (cubeUnderMouse is null)
            {
                return;
            }
            DiceTerrain dice = cubeUnderMouse.GetComponent<DiceTerrain>();
            var array = TerrainManager.instance.GetAvailableArray();
            
            if (!array[dice.pos.x, dice.pos.y])
            {
                return;
            }
            
            Character.instance.SetPath(Pathfinder.GetPath(character.pos.x, character.pos.y, dice.pos.x, dice.pos.y, array));
        }
    }
    
    private void UpdateCursor()
    {
        if (Time.frameCount % 5 == 0)
        {
            cubeUnderMouse = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 500,layerCheck) ? hit.collider.transform : null;
            if (cubeUnderMouse is null)
            {
                return;
            }
            
            DiceTerrain dice = cubeUnderMouse.GetComponent<DiceTerrain>();
            var array = TerrainManager.instance.GetAvailableArray();
            m_cursorRenderer.sprite = array[dice.pos.x, dice.pos.y] ? m_cursorSprites[0]:m_cursorSprites [1];
            cubePosition.transform.DOMove(new Vector3(cubeUnderMouse.position.x, cubeUnderMouse.position.y + cursorHeight, cubeUnderMouse.position.z), 0.25f);
        }
    }

}