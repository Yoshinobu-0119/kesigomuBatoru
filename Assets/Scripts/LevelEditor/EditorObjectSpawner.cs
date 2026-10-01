using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField]
    private GameObject[] spawnPrefabs;

    [Header("Grid")]
    [SerializeField]
    private bool useGrid = true;

    [SerializeField]
    private float gridSize = 1f;

    [Header("Rotation")]
    [SerializeField]
    private float rotationStep = 90f;

    [Header("Physics")]
    [SerializeField]
    private bool addRigidbody;

    [Header("Raycast")]
    [SerializeField]
    private LayerMask placementMask = ~0;

    private Camera mainCamera;

    private int currentPrefabIndex;
    private int lastPreviewIndex = -1;

    private float yRotation;

    private Vector3 spawnPos;
    private Quaternion spawnRot;

    private GameObject previewObject;

    private void Start()
    {
        mainCamera = Camera.main;

        if (spawnPrefabs != null &&
            spawnPrefabs.Length > 0)
        {
            CreatePreview();
        }
    }

    private void Update()
    {
        if (spawnPrefabs == null ||
            spawnPrefabs.Length == 0)
        {
            return;
        }

        HandleInput();

        if (currentPrefabIndex != lastPreviewIndex)
        {
            CreatePreview();
        }

        UpdatePreview();

        if (Input.GetMouseButtonDown(0))
        {
            PlaceObject();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            useGrid = !useGrid;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            yRotation -= rotationStep;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            yRotation += rotationStep;
        }

        float wheel = Input.mouseScrollDelta.y;

        if (wheel > 0)
        {
            currentPrefabIndex =
                (currentPrefabIndex + 1)
                % spawnPrefabs.Length;
        }
        else if (wheel < 0)
        {
            currentPrefabIndex =
                (currentPrefabIndex - 1 + spawnPrefabs.Length)
                % spawnPrefabs.Length;
        }
    }

    private void UpdatePreview()
    {
        Ray ray =
            mainCamera.ScreenPointToRay(
                Input.mousePosition);

        bool isHit =
            Physics.Raycast(
                ray,
                out RaycastHit hit,
                Mathf.Infinity,
                placementMask);

        if (!isHit)
        {
            if (previewObject != null)
            {
                previewObject.SetActive(false);
            }

            return;
        }

        spawnPos = hit.point;

        if (useGrid)
        {
            spawnPos = SnapToGrid(spawnPos);
        }

        Quaternion surfaceRotation =
            Quaternion.FromToRotation(
                Vector3.up,
                hit.normal);

        Quaternion localRotation =
            Quaternion.Euler(
                0f,
                yRotation,
                0f);

        spawnRot =
            surfaceRotation *
            localRotation;

        if (previewObject != null)
        {
            previewObject.transform
                .SetPositionAndRotation(
                    spawnPos,
                    spawnRot);

            float offset =
                CalculateGroundOffset(
                    previewObject);

            spawnPos +=
                hit.normal * offset;

            previewObject.transform
                .SetPositionAndRotation(
                    spawnPos,
                    spawnRot);

            previewObject.SetActive(true);
        }
    }

    private void PlaceObject()
    {
        if (previewObject == null ||
            !previewObject.activeSelf)
        {
            return;
        }

        GameObject spawnedObject =
            Instantiate(
                spawnPrefabs[currentPrefabIndex],
                spawnPos,
                spawnRot);

        if (addRigidbody)
        {
            if (!spawnedObject.TryGetComponent<Rigidbody>(out _))
            {
                spawnedObject.AddComponent<Rigidbody>();
            }
        }

        EditorSpawnedObject data =
            spawnedObject.AddComponent<EditorSpawnedObject>();

        data.prefabIndex =
            currentPrefabIndex;

        data.hasRigidbody =
            addRigidbody;
    }

    private float CalculateGroundOffset(
        GameObject obj)
    {
        Renderer[] renderers =
            obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            return 0f;
        }

        Bounds bounds =
            renderers[0].bounds;

        for (int i = 1;
             i < renderers.Length;
             i++)
        {
            bounds.Encapsulate(
                renderers[i].bounds);
        }

        return obj.transform.position.y
               - bounds.min.y;
    }

    private Vector3 SnapToGrid(
        Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize)
            * gridSize,

            position.y,

            Mathf.Round(position.z / gridSize)
            * gridSize
        );
    }

    private void CreatePreview()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        previewObject =
            Instantiate(
                spawnPrefabs[currentPrefabIndex]);

        previewObject.name =
            $"Preview_{spawnPrefabs[currentPrefabIndex].name}";

        SetLayerRecursively(
            previewObject,
            LayerMask.NameToLayer(
                "Ignore Raycast"));

        foreach (Collider collider in
                 previewObject.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        foreach (Renderer renderer in
                 previewObject.GetComponentsInChildren<Renderer>())
        {
            foreach (Material material in
                     renderer.materials)
            {
                if (!material.HasProperty("_Color"))
                    continue;

                Color color =
                    material.color;

                color.a = 0.5f;

                material.color =
                    color;
            }
        }

        lastPreviewIndex =
            currentPrefabIndex;
    }

    private void SetLayerRecursively(
        GameObject obj,
        int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(
                child.gameObject,
                layer);
        }
    }

    private void OnDestroy()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }
}