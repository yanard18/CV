using System.Collections.Generic;
using MekaruStudios.MonsterCreator.Infrastructure;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace MekaruStudios.MonsterCreator.Tests
{
    public class CosmeticModuleTests
    {
        readonly CosmeticSlot hatCosmeticSlot = new CosmeticSlot("hat", "armature/body/head", Vector3.zero);

        public class LoadCosmeticsBySlotMethod : CosmeticModuleTests
        {
            [Test]
            public void _Hat_Slot_Load_Any_Cosmetic()
            {
                var cosmeticModule = new CosmeticModule(hatCosmeticSlot);

                cosmeticModule.LoadCosmeticsBySlotType();
                Assert.IsNotEmpty(cosmeticModule.GetCosmetics(), "available cosmetics are should be loaded");
            }
            [Test]
            public void _Hat_Slot_Load_Hat_Cosmetics()
            {
                var cosmeticModule = new CosmeticModule(hatCosmeticSlot);

                cosmeticModule.LoadCosmeticsBySlotType();
                var cosmetics = cosmeticModule.GetCosmetics();
                var loadedCosmeticSlotTypeNames = new List<string>();
                foreach (var cosmetic in cosmetics)
                {
                    if (!loadedCosmeticSlotTypeNames.Contains(cosmetic.slotName))
                        loadedCosmeticSlotTypeNames.Add(cosmetic.slotName);
                }

                Assert.That(loadedCosmeticSlotTypeNames, Does.Contain("hat"));
            }
        }

        public class CreateCosmeticMethod : CosmeticModuleTests
        {
            CosmeticModule defaultCosmeticModule;
            Cosmetic testCosmetic;
            GameObject cosmeticTestPrefab;
            GameObject tempModelTestPrefab;

            const string HEAD_BONE_PATH = "armature/body/head/";
            const string BODY_BONE_PATH = "armature/body/";

            [SetUp]
            public void SetUp()
            {

                var rootGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var armatureGameObject = new GameObject("armature");
                var headGameObject = new GameObject("head");
                var bodyGameObject = new GameObject("body");

                headGameObject.transform.SetParent(bodyGameObject.transform);
                bodyGameObject.transform.SetParent(armatureGameObject.transform);
                armatureGameObject.transform.SetParent(rootGameObject.transform);
                tempModelTestPrefab = A.Prefab()
                    .WithName("test_temp_model_prefab")
                    .WithGameObject(rootGameObject);

                cosmeticTestPrefab = A.Prefab().WithName("test_cosmetic_prefab");
                testCosmetic = new Cosmetic(cosmeticTestPrefab, "hat");

                defaultCosmeticModule = new CosmeticModule(hatCosmeticSlot);
                defaultCosmeticModule.LoadCosmeticsBySlotType();
            }


            [Test]
            public void _Create_Without_Temporary_Model_Should_Throw_TemporaryModelNotFoundException()
            {
                Assert.That(() => defaultCosmeticModule.CreateCosmetic(testCosmetic),
                    Throws.TypeOf<TemporaryModelNotFoundException>());
            }

            [Test]
            public void _Create_With_Valid_Temporary_Model_Should_Instantiate_Cosmetic()
            {
                TemporaryModel.Create(tempModelTestPrefab);
                defaultCosmeticModule.CreateCosmetic(testCosmetic);
                var instantiatedCosmetic = GameObject.Find(testCosmetic.prefab.name);

                Assert.That(instantiatedCosmetic, Is.Not.Null);
            }

            [Test]
            public void _Hat_Cosmetic_Slot_Should_Place_On_Correct_Bone()
            {
                TemporaryModel.Create(tempModelTestPrefab);
                defaultCosmeticModule.CreateCosmetic(testCosmetic);
                var instantiatedCosmetic =
                    TemporaryModel.Get().transform.Find(HEAD_BONE_PATH + testCosmetic.prefab.name);


                Assert.That(instantiatedCosmetic, Is.Not.Null);
            }

            [Test]
            public void _Hat_Cosmetic_Slot_Should_Place_On_Correct_Position()
            {
                TemporaryModel.Create(tempModelTestPrefab);
                var expectedPos = Vector3.one;
                var cosmeticSlot = new CosmeticSlot("hat", "armature/body/head", expectedPos);
                var cosmeticModule = new CosmeticModule(cosmeticSlot);
                
                cosmeticModule.CreateCosmetic(testCosmetic);
                var instantiatedCosmetic =
                    TemporaryModel.Get().transform.Find($"armature/body/head/{testCosmetic.prefab.name}");
                var localPosOfCosmetic = instantiatedCosmetic.localPosition;

                Assert.That(localPosOfCosmetic, Is.EqualTo(Vector3.one));
            }

            [Test]
            public void _Create_Should_Remove_Cosmetic_With_Same_Type()
            {
                TemporaryModel.Create(tempModelTestPrefab);
                GameObject newPrefab = A.Prefab().WithName("test_cosmetic_same_type");
                var newCosmetic = new Cosmetic(newPrefab, "hat");
                defaultCosmeticModule.CreateCosmetic(testCosmetic);

                defaultCosmeticModule.CreateCosmetic(newCosmetic);

                var instantiatedCosmetic =
                    TemporaryModel.Get().transform.Find($"armature/body/head/{newCosmetic.prefab.name}");
                var droppedCosmetic =
                    TemporaryModel.Get().transform.Find($"armature/body/head/{testCosmetic.prefab.name}");
                var isCosmeticsSwapped = instantiatedCosmetic != null && droppedCosmetic == null;

                Assert.That(isCosmeticsSwapped, Is.True);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(newPrefab));
            }

            [Test]
            public void _Create_Same_Cosmetic_Should_Remove_Existent_Cosmetic()
            {
                TemporaryModel.Create(tempModelTestPrefab);
                GameObject newPrefab = A.Prefab().WithName("test_cosmetic");
                var cosmetic = new Cosmetic(newPrefab, "hat");
                defaultCosmeticModule.CreateCosmetic(cosmetic);

                defaultCosmeticModule.CreateCosmetic(cosmetic);

                var instantiatedCosmetic =
                    TemporaryModel.Get().transform.Find($"armature/body/head/{cosmetic.prefab.name}");


                Assert.That(instantiatedCosmetic, Is.Null);
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(newPrefab));
            }

            [TearDown]
            public void CleanCosmeticPrefab()
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(cosmeticTestPrefab));
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(tempModelTestPrefab));
            }
        }
    }
}
