using System.Collections.Generic;
using DG.Tweening;
using Entities;
using Managers;
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

    [SerializeField] private Leurre m_leurre;
    
    private Character  character => Character.instance;
    
    private void Update()
    {
        UpdateCursor();
        UpdateMouseDown();
    }

    private void UpdateMouseDown()
    {
        if (character.canPlay && !TerrainManager.instance.IsUpdateingLevel)
        {
            if (Input.GetMouseButtonDown(0))
            {
                cubeUnderMouse =
                    Physics.Raycast(CameraManager.instance.Camera.ScreenPointToRay(Input.mousePosition),
                        out RaycastHit hit, 500, layerCheck)
                        ? hit.collider.transform
                        : null;

                if (cubeUnderMouse is null || cubeUnderMouse.CompareTag("Water"))
                {
                    return;
                }

                DiceTerrain dice = cubeUnderMouse.GetComponent<DiceTerrain>();
                var array = TerrainManager.instance.GetAvailableArray(TerrainManager.instance
                    .diceTerrainlsit[character.pos.x, character.pos.y].diceData.isWall);

                if (dice is null) return;

                if (!array[dice.pos.x, dice.pos.y])
                {
                    return;
                }

                Character.instance.SetPath(Pathfinder.GetPath(character.pos.x, character.pos.y, dice.pos.x, dice.pos.y, array));
            }
            
            if (Input.GetMouseButtonDown(1))
            {      
                cubeUnderMouse = Physics.Raycast(CameraManager.instance.Camera.ScreenPointToRay(Input.mousePosition),
                        out RaycastHit hit, 500, layerCheck) ? hit.collider.transform : null;

                if (cubeUnderMouse is null || cubeUnderMouse.CompareTag("Water"))
                {
                    return;
                }
                
                DiceTerrain dice = cubeUnderMouse.GetComponent<DiceTerrain>();
                if (dice is null) return;
                
                character.canPlay = false;
                
                Leurre leurre = Instantiate(m_leurre, dice.pivot.transform);
                leurre.transform.localPosition = new Vector3(0, 1.05f, 0);
                leurre.Init(dice.pos.x, dice.pos.y);
                character.EndTurn();
            }
        }
    }
    
    private void UpdateCursor()
    {
        if (Time.frameCount % 5 == 0)
        {
            cubeUnderMouse = Physics.Raycast(CameraManager.instance.Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 500,layerCheck) && !hit.collider.gameObject.CompareTag("Water")? hit.collider.transform : null;
            if (cubeUnderMouse is null) {
                cubePosition.SetActive(false);
                return;
            }

            cubePosition.SetActive(true);
            
            DiceTerrain dice = cubeUnderMouse.GetComponent<DiceTerrain>();
            var array = TerrainManager.instance.GetAvailableArray(TerrainManager.instance.diceTerrainlsit[character.pos.x,character.pos.y].diceData.isWall);
            
            if (TerrainManager.instance.IsUpdateingLevel) m_cursorRenderer.sprite = m_cursorSprites[2];
            else if (Vector2Int.Distance(dice.pos, character.pos) > 1) m_cursorRenderer.sprite = m_cursorSprites[1];
            else m_cursorRenderer.sprite = array[dice.pos.x, dice.pos.y] ? m_cursorSprites[0] : m_cursorSprites[1];
            
            cubePosition.transform.DOMove(new Vector3(cubeUnderMouse.position.x, cubeUnderMouse.position.y + cursorHeight, cubeUnderMouse.position.z), 0.25f);
        }
    }

}