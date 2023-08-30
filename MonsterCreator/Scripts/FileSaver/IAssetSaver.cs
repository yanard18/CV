using UnityEngine;

public interface IAssetSaver
{
    void Save(GameObject objectToSave, string folderPath);
}
