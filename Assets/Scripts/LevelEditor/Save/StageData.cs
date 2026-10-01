using System.Collections.Generic;

[System.Serializable]
public class StageData
{
    public string stageName;

    public List<ObjectData> objects =
        new List<ObjectData>();
}