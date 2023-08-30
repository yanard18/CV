using System;
using System.Collections.Generic;
using MekaruStudios.MonsterCreator;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class TemporaryModel
{
    public static event Action onTemporaryModelUpdated;
    static GameObject gameObject;
    static Dictionary<Cosmetic, GameObject> activeCosmetics = new Dictionary<Cosmetic, GameObject>();

    public static GameObject Get() => gameObject;
    public static bool IsExist() => gameObject != null;

    public static void Create(Object prefab)
    {
        if (gameObject != null)
            Destroy();

        TryCreateNewGameObject(prefab);
    }
    public static void Destroy()
    {
        if (gameObject != null)
            Object.DestroyImmediate(gameObject, false);
    }

    public static Dictionary<Cosmetic, GameObject> GetCosmetics()
    {
        return activeCosmetics;
    }
    public static void UpdateTemporaryModel()
    {
        onTemporaryModelUpdated?.Invoke();
    }
    static void TryCreateNewGameObject(Object prefab)
    {
        try
        {
            CreateNewGameObject(prefab);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"Argument is null: {e}");
            throw;
        }
    }



    static void CreateNewGameObject(Object prefab)
    {
        if (prefab == null) throw new ArgumentNullException(nameof(prefab));
        gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

        // ReSharper disable once PossibleNullReferenceException
        gameObject.hideFlags = HideFlags.HideInHierarchy;
    }



}
