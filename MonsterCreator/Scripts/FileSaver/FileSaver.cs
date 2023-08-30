using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MekaruStudios.MonsterCreator.FileSaving
{
    public class FileSaver : IAssetSaver
    {
        public void Save(GameObject objectToSave, string folderPath)
        {
            if (!IsPathRelative(folderPath))
                throw new FilePathNotValidException();

            var prefabPath = $"{folderPath}/{objectToSave.name}.prefab";
            Object prefab = PrefabUtility.SaveAsPrefabAsset(objectToSave, prefabPath);
        }

        static bool IsPathRelative(string folderPath)
        {
            return folderPath.StartsWith(Application.dataPath);
        }

    }
}
