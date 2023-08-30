using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MekaruStudios.MonsterCreator
{
    [Serializable]
    public class CosmeticModule
    {

        [SerializeField]
        CosmeticSlot[] slots;
        
        Cosmetic[] cosmetics;
        
        [NonSerialized]
        bool areCosmeticsLoaded;

        public CosmeticModule(params CosmeticSlot[] slots)
        {
            this.slots = slots;
        }

        public CosmeticModule() : this(Array.Empty<CosmeticSlot>())
        {
        }

        public Cosmetic[] GetCosmetics()
        {
            if(!areCosmeticsLoaded)
                LoadCosmeticsBySlotType();
            
            return cosmetics;
        }
        public void ClearCosmetics() => cosmetics = Array.Empty<Cosmetic>();

        public void CreateCosmetic(Cosmetic cosmetic)
        {
            if (!TemporaryModel.IsExist()) throw new TemporaryModelNotFoundException();
            if (IsSameCosmeticExist(cosmetic))
            {
                DestroyCosmetic(cosmetic);
                return;
            }

            var instantiatedGameObject = (GameObject)PrefabUtility.InstantiatePrefab(cosmetic.prefab);
            var correspondedCosmeticSlot = FindCorrespondedCosmeticSlot(cosmetic);
            var parentBone = TemporaryModel.Get().transform.Find(correspondedCosmeticSlot.bonePath);

            instantiatedGameObject.transform.localPosition = correspondedCosmeticSlot.position;
            instantiatedGameObject.transform.localRotation = Quaternion.identity;
            instantiatedGameObject.transform.SetParent(parentBone, true);

            ClearCosmeticInSameSlot(cosmetic);
            TemporaryModel.GetCosmetics().Add(cosmetic, instantiatedGameObject);
            TemporaryModel.UpdateTemporaryModel();
        }
        public void LoadCosmeticsBySlotType()
        {
            var cosmeticsStorage = GetCosmeticStorage();

            var cosmeticList = new List<Cosmetic>();
            foreach (var cosmetic in cosmeticsStorage.GetCosmetics())
            {
                foreach (var cosmeticPlacementData in slots)
                {
                    if (string.Equals(cosmeticPlacementData.slotName, cosmetic.slotName))
                    {
                        cosmeticList.Add(cosmetic);
                        break;
                    }
                }
            }

#if UNITY_EDITOR
            Debug.Log($"{cosmeticList.Count} model associated with model metadata");
#endif
            cosmetics = cosmeticList.ToArray();
            areCosmeticsLoaded = true;
        }



        static CosmeticStorage GetCosmeticStorage()
        {
            var guid = AssetDatabase.FindAssets($"t:{nameof(CosmeticStorage)}")[0];
            var path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<CosmeticStorage>(path);
        }
        static void DestroyCosmetic(Cosmetic cosmetic)
        {
            TemporaryModel.GetCosmetics().TryGetValue(cosmetic, out var value);
            Object.DestroyImmediate(value);
            TemporaryModel.GetCosmetics().Remove(cosmetic);
            TemporaryModel.UpdateTemporaryModel();
        }

        CosmeticSlot FindCorrespondedCosmeticSlot(Cosmetic cosmetic)
        {
            foreach (var slot in slots)
            {
                if (string.Equals(slot.slotName, cosmetic.slotName))
                    return slot;
            }

            return new CosmeticSlot("null", "", Vector3.zero);
        }

        static bool IsSameCosmeticExist(Cosmetic cosmetic)
        {
            var cosmeticsInModel = TemporaryModel.GetCosmetics();
            return cosmeticsInModel.ContainsKey(cosmetic);

        }

        static void ClearCosmeticInSameSlot(Cosmetic cosmeticToCompare)
        {
            var cosmeticsToRemove = TemporaryModel.GetCosmetics()
                .Where(cosmetic => string.Equals(cosmetic.Key.slotName, cosmeticToCompare.slotName))
                .ToList();

            foreach (var cosmetic in cosmeticsToRemove)
            {
                Object.DestroyImmediate(cosmetic.Value);
                TemporaryModel.GetCosmetics().Remove(cosmetic.Key);
            }

        }

    }
}
