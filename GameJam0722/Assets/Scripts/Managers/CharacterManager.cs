using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager> {
    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private GameObject cubePosition = null;
    [Space] 
    [SerializeField] private LayerMask layerCheck;
    [SerializeField] private Transform player = null;
    [SerializeField] private float playerSpeed = 1;
    [SerializeField] private float cursorHeight = .5f;
    [Space]
    [SerializeField] private Transform cubeUnderMouse = null;

    
    
    [SerializeField] private List<Vector3> targetPosition = new List<Vector3>();
    
    private void Update() {
        cubeUnderMouse = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 500,layerCheck) ? hit.collider.transform : null;
        
        if (cubeUnderMouse != null) {
            cubePosition.transform.position = Vector3.Lerp(cubePosition.transform.position, new Vector3(cubeUnderMouse.position.x, cubeUnderMouse.position.y + cursorHeight, cubeUnderMouse.position.z), .05f);
        }

        if (Input.GetMouseButtonDown(0) && cubeUnderMouse != null) {
            player.DOKill();
            targetPosition.Add(new Vector3(cubeUnderMouse.position.x, cubeUnderMouse.position.y + cursorHeight * 2, cubeUnderMouse.position.z));
            ChangeTarget();
        }
    }

    /// <summary>
    /// Change the target of the player
    /// </summary>
    private void ChangeTarget() {
        if (targetPosition.Count == 0) return;
        player.DOLocalMove(targetPosition[0], GetPlayerSpeed(targetPosition[0])).OnComplete(ChangeTarget);
        targetPosition.RemoveAt(0);
    }
    
    #region Helper
    private float GetPlayerSpeed(Vector3 endPos) => Vector3.Distance(new Vector3(cubeUnderMouse.position.x, cubeUnderMouse.position.y + cursorHeight, cubeUnderMouse.position.z), player.transform.position) / playerSpeed;
    #endregion Helper
}