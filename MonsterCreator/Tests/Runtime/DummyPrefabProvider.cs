using UnityEditor;
using UnityEngine;

namespace MekaruStudios.MonsterCreator
{
    public static class DummyPrefabProvider
    {

        const string ASSET_NAME = "monster_creator_test_temp";
        static GameObject tempPrefab;

        public static GameObject Get() => tempPrefab;

        public static void CreateTemporaryPrefab()
        {
            var newObject = new GameObject();
            var prefab = PrefabUtility.SaveAsPrefabAsset(newObject, $"Assets/{ASSET_NAME}.prefab");
            tempPrefab = prefab;
            Object.DestroyImmediate(newObject);
        }

        public static void ClearTemporaryPrefab()
        {
            if (tempPrefab == null)
                return;

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(tempPrefab));
        }

    }
}
