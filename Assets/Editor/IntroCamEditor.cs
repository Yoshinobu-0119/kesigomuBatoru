using UnityEngine;
using UnityEditor;

public class IntroCamEditor : EditorWindow
{
    [MenuItem("Tools/IntroCam/Add Camera Point")]
    public static void AddCameraPoint()
    {
        SceneView view = SceneView.lastActiveSceneView;
        if (view == null) return;

        Camera sceneCam = view.camera;

        GameObject point = new GameObject("CamPoint");
        point.transform.position = sceneCam.transform.position;
        point.transform.rotation = sceneCam.transform.rotation;

        // moveTimer を保持するためのコンポーネント
        var cp = point.AddComponent<CameraPointMarker>();
        cp.moveTime = 0.5f;

        Selection.activeGameObject = point;
    }
}
