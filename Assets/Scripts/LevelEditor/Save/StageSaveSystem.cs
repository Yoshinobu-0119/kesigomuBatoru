using System.IO;
using UnityEngine;

public static class StageSaveSystem
{
    public static void SaveStage(StageData data)
    {
        string json =
            JsonUtility.ToJson(data, true);

        string path =
            Path.Combine(
                Application.persistentDataPath,
                data.stageName + ".json");

        File.WriteAllText(path, json);

        Debug.Log($"Saved : {path}");
    }

    public static StageData LoadStage(
        string stageName)
    {
        string path =
            Path.Combine(
                Application.persistentDataPath,
                stageName + ".json");

        if (!File.Exists(path))
        {
            Debug.LogWarning($"Not Found : {path}");
            return null;
        }

        string json =
            File.ReadAllText(path);

        return JsonUtility.FromJson<StageData>(
            json);
    }
}