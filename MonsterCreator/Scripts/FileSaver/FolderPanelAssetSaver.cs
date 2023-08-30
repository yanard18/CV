using UnityEditor;
using UnityEngine;

namespace MekaruStudios.MonsterCreator.FileSaving
{

    public static class FolderPanelAssetSaver
    {
        public static void Save(GameObject objectToSave)
        {
            TrySave(objectToSave);
        }

        static void TrySave(GameObject objectToSave)
        {
            try
            {
                var fileSaver = new FileSaver();
                var path = EditorUtility.SaveFolderPanel("Select Folder", "", "");
                fileSaver.Save(objectToSave, path);
            }
            catch (FilePathNotValidException e)
            {
                Debug.LogWarning($"File path should be relative to the project: {e}");
            }

        }
    }
}
