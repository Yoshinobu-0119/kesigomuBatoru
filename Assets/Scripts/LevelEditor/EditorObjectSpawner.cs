using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorObjectSpawner : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera editorCamera;

    [Header("Placeables")]
    [SerializeField] private PlaceableData[] placeables;

    [Header("Grid")]
    [SerializeField] private bool useGrid = true;
    [SerializeField] private float gridSize = 0.5f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;

    [Header("Rotation")]
    [SerializeField] private float rotateStep = 5f;

    [Header("Placement Bounds")]
    [SerializeField] private Vector2 minBounds = new(-10f, -10f);
    [SerializeField] private Vector2 maxBounds = new(10f, 10f);

    [Header("Physics")]
    [SerializeField] private bool useRigidBody = true;

    [SerializeField]
    private TMP_InputField scoreInput;

    [SerializeField]
    private Toggle respawnToggle;
    private enum EditorMode
    {
        Place,
        Edit
    }

    private EditorMode currentMode = EditorMode.Place;

    private GameObject selectedPlacedObject;
    private bool isDraggingPlacedObject;
    private GameObject previewObject;
    private int selectedIndex;
    private float currentRotationY;
    private float previewBottomOffset;

    [SerializeField]
    private TMP_InputField stageNameInput;

    private readonly Stack<GameObject> placedObjects = new();

    private void Start()
    {
        if (placeables.Length > 0)
        {
            SelectObject(0);
        }
    }

    private void Update()
    {
        MoveCamera();

        HandleObjectSelection();

        HandleGridToggle();

        HandleRigidBodyToggle();

        HandleUndo();

        if (currentMode == EditorMode.Place)
        {
            HandleRotation();
            UpdatePreview();
            HandlePlacement();
        }
        else
        {
            if (previewObject != null)
            {
                previewObject.SetActive(false);
            }

            HandlePlacedObjectEdit();
        }

        HandlePlacedObjectSelection();

        HandleModeCancel();
    }

    #region Camera

    private void MoveCamera()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            move += transform.forward;

        if (Input.GetKey(KeyCode.S))
            move -= transform.forward;

        if (Input.GetKey(KeyCode.D))
            move += transform.right;

        if (Input.GetKey(KeyCode.A))
            move -= transform.right;

        if (Input.GetKey(KeyCode.E))
            move += Vector3.up;

        if (Input.GetKey(KeyCode.Q))
            move -= Vector3.up;

        if (move.sqrMagnitude > 0f)
        {
            transform.position +=
                move.normalized *
                moveSpeed *
                Time.deltaTime;
        }
    }

    #endregion

    #region Selection

    private void HandleObjectSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectObject(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectObject(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectObject(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectObject(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectObject(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectObject(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectObject(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) SelectObject(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) SelectObject(8);
        if (Input.GetKeyDown(KeyCode.Alpha0)) SelectObject(9);
    }

    public void SelectObject(int index)
    {
        if (index < 0 ||
            index >= placeables.Length)
            return;

        currentMode = EditorMode.Place;

        selectedPlacedObject = null;

        selectedIndex = index;

        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        previewObject =
            Instantiate(placeables[index].prefab);

        foreach (Collider col in
                 previewObject.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        previewBottomOffset =
            GetBottomOffset(previewObject);
    }

    #endregion

    #region Grid

    private void HandleGridToggle()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            useGrid = !useGrid;
        }
    }

    public void ToggleGrid()
    {
        useGrid = !useGrid;
    }

    #endregion

    #region Rotation

    private void HandleRotation()
    {
        float wheel = Input.mouseScrollDelta.y;

        if (wheel > 0)
            currentRotationY += rotateStep;

        if (wheel < 0)
            currentRotationY -= rotateStep;
    }

    #endregion

    #region Preview

    private void UpdatePreview()
    {
        if (previewObject == null)
            return;

        Ray ray =
            editorCamera.ScreenPointToRay(
                Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            previewObject.SetActive(false);
            return;
        }

        PlaceSurface surface =
            hit.collider.GetComponentInParent<PlaceSurface>();

        if (surface == null)
        {
            previewObject.SetActive(false);
            return;
        }

        PlaceType currentType =
            placeables[selectedIndex].placeType;

        if (!surface.CanPlace(currentType))
        {
            previewObject.SetActive(false);
            return;
        }

        // 側面禁止
        if (Vector3.Dot(hit.normal, Vector3.up) < 0.8f)
        {
            previewObject.SetActive(false);
            return;
        }

        Vector3 position = hit.point;

        if (useGrid)
        {
            position.x =
                Mathf.Round(position.x / gridSize) * gridSize;

            position.y =
                Mathf.Round(position.y / gridSize) * gridSize;

            position.z =
                Mathf.Round(position.z / gridSize) * gridSize;
        }

        if (!IsInsideBounds(position))
        {
            previewObject.SetActive(false);
            return;
        }

        previewObject.SetActive(true);

        previewObject.transform.position =
            position + Vector3.up * previewBottomOffset;

        previewObject.transform.rotation =
            Quaternion.Euler(
                0f,
                currentRotationY,
                0f);
    }

    #endregion

    #region Placement

    private void HandlePlacement()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (previewObject == null)
            return;

        if (!previewObject.activeSelf)
            return;

        GameObject placedObject =
            Instantiate(
                placeables[selectedIndex].prefab,
                previewObject.transform.position,
                previewObject.transform.rotation);

        EditorPlacedObject editorObject =
            placedObject.GetComponent<EditorPlacedObject>();

        if (editorObject == null)
        {
            editorObject =
                placedObject.AddComponent<EditorPlacedObject>();
        }

        editorObject.placeType =
            placeables[selectedIndex].placeType;

        editorObject.placeType =
            placeables[selectedIndex].placeType; // 既存のコード

        // 以下の1行を追加してprefabIdを記録する
        editorObject.prefabId = placeables[selectedIndex].prefabId;

        editorObject.SetRigidBodyEnabled(useRigidBody); // 既存のコード
    }

    #endregion

    #region Undo

    private void HandleUndo()
    {
        if (!Input.GetKey(KeyCode.LeftControl))
            return;

        if (!Input.GetKeyDown(KeyCode.Z))
            return;

        if (placedObjects.Count == 0)
            return;

        Destroy(
            placedObjects.Pop());
    }

    #endregion

    #region Utility

    private float GetBottomOffset(GameObject obj)
    {
        Renderer[] renderers =
            obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return 0f;

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(
                renderers[i].bounds);
        }

        return bounds.extents.y;
    }

    #endregion

    private void HandleRigidBodyToggle()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRigidBody();
        }
    }

    public void ToggleRigidBody()
    {
        useRigidBody = !useRigidBody;
    }

    private bool IsInsideBounds(Vector3 position)
    {
        return
            position.x >= minBounds.x &&
            position.x <= maxBounds.x &&
            position.z >= minBounds.y &&
            position.z <= maxBounds.y;
    }

    private void HandlePlacedObjectSelection()
    {
        if (!Input.GetMouseButtonDown(1))
            return;

        Ray ray =
            editorCamera.ScreenPointToRay(
                Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        EditorPlacedObject placed =
            hit.collider.GetComponentInParent<EditorPlacedObject>();

        if (placed == null)
            return;

        selectedPlacedObject = placed.gameObject;

        currentMode = EditorMode.Edit;

        Debug.Log("編集モード");
    }

    private void HandleModeCancel()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        selectedPlacedObject = null;
        isDraggingPlacedObject = false;

        currentMode = EditorMode.Place;

        if (previewObject != null)
        {
            previewObject.SetActive(true);
        }
    }

    private void HandlePlacedObjectEdit()
    {
        if (selectedPlacedObject == null)
            return;

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(selectedPlacedObject);

            selectedPlacedObject = null;

            currentMode = EditorMode.Place;

            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            isDraggingPlacedObject =
                !isDraggingPlacedObject;
        }

        float wheel =
            Input.mouseScrollDelta.y;

        if (wheel > 0)
        {
            selectedPlacedObject.transform.Rotate(
                0f,
                rotateStep,
                0f,
                Space.World);
        }

        if (wheel < 0)
        {
            selectedPlacedObject.transform.Rotate(
                0f,
                -rotateStep,
                0f,
                Space.World);
        }

        if (!isDraggingPlacedObject)
            return;

        Ray ray =
            editorCamera.ScreenPointToRay(
                Input.mousePosition);

        if (!Physics.Raycast(ray,
                out RaycastHit hit,
                1000f))
            return;

        PlaceSurface surface =
            hit.collider.GetComponentInParent<PlaceSurface>();

        if (surface == null)
            return;

        EditorPlacedObject info =
            selectedPlacedObject.GetComponent<EditorPlacedObject>();

        if (info == null)
            return;

        if (!surface.CanPlace(info.placeType))
            return;

        Vector3 pos = hit.point;

        if (useGrid)
        {
            pos.x =
                Mathf.Round(pos.x / gridSize) *
                gridSize;

            pos.y =
                Mathf.Round(pos.y / gridSize) *
                gridSize;

            pos.z =
                Mathf.Round(pos.z / gridSize) *
                gridSize;
        }

        if (!IsInsideBounds(pos))
            return;

        Renderer[] renderers =
            selectedPlacedObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            Bounds bounds =
                renderers[0].bounds;

            for (int i = 1;
                 i < renderers.Length;
                 i++)
            {
                bounds.Encapsulate(
                    renderers[i].bounds);
            }

            pos.y += bounds.extents.y;
        }

        selectedPlacedObject.transform.position =
            pos;
    }

    public void SaveStage()
    {
        if (string.IsNullOrWhiteSpace(
            stageNameInput.text))
        {
            Debug.LogWarning(
                "ステージ名を入力してください");
            return;
        }

        StageData stageData =
            new StageData();

        stageData.stageName =
            stageNameInput.text;

        EditorPlacedObject[] objects =
            FindObjectsByType<EditorPlacedObject>(
                FindObjectsSortMode.None);

        foreach (EditorPlacedObject obj in objects)
        {
            ObjectData objectData =
                new ObjectData();

            objectData.prefabId =
                obj.prefabId;

            Vector3 pos =
                obj.transform.position;

            objectData.posX = pos.x;
            objectData.posY = pos.y;
            objectData.posZ = pos.z;

            Vector3 rot =
                obj.transform.eulerAngles;

            objectData.rotX = rot.x;
            objectData.rotY = rot.y;
            objectData.rotZ = rot.z;

            objectData.useRigidBody =
                obj.useRigidBody;

            objectData.itemScore =
                obj.itemScore;

            objectData.canRespawn =
                obj.canRespawn;

            stageData.objects.Add(
                objectData);
        }

        StageSaveSystem.SaveStage(
            stageData);
    }

    private void RefreshEditUI()
    {
        if (selectedPlacedObject == null)
            return;

        EditorPlacedObject obj =
            selectedPlacedObject
            .GetComponent<EditorPlacedObject>();

        scoreInput.text =
            obj.itemScore.ToString();

        respawnToggle.isOn =
            obj.canRespawn;
    }
    public void ApplyGameplaySettings()
    {
        if (selectedPlacedObject == null)
            return;

        EditorPlacedObject obj =
            selectedPlacedObject
            .GetComponent<EditorPlacedObject>();

        if (int.TryParse(
            scoreInput.text,
            out int score))
        {
            obj.itemScore = score;
        }

        obj.canRespawn =
            respawnToggle.isOn;
    }
}