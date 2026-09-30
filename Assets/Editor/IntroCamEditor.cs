using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;

public class IntroCamEditor : EditorWindow
{
    [MenuItem("Tools/IntroCam/Add Camera Point")]
    public static void AddCameraPoint()
    {
        SceneView view = SceneView.lastActiveSceneView;
        if (view == null) return;

        Camera sceneCam = view.camera;

        // ä˘ë∂ÇÃç≈ëÂî‘çÜÇéÊìæ
        int nextIndex = 0;

        var points = Object.FindObjectsOfType<CameraPointMarker>();

        foreach (var p in points)
        {
            Match match = Regex.Match(p.name, @"\d+");

            if (match.Success)
            {
                int number = int.Parse(match.Value);
                nextIndex = Mathf.Max(nextIndex, number + 1);
            }
        }

        GameObject point = new GameObject($"CamPoint_{nextIndex:D3}");

        point.transform.position = sceneCam.transform.position;
        point.transform.rotation = sceneCam.transform.rotation;

        var cp = point.AddComponent<CameraPointMarker>();
        cp.moveTime = 0.5f;

        Selection.activeGameObject = point;

        Undo.RegisterCreatedObjectUndo(point, "Create Camera Point");
    }
}