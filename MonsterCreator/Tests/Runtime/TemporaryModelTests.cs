using System;
using NUnit.Framework;
using UnityEngine;

namespace MekaruStudios.MonsterCreator.Testing
{
    public abstract class TemporaryModelTests
    {

        public class CreateMethod
        {

            [Test]
            public void Create_WithNullPrefab_ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => TemporaryModel.Create(null));
            }

            [Test]
            public void Create_WithValidPrefab_CreatesModel()
            {
                DummyPrefabProvider.CreateTemporaryPrefab();
                var prefab = DummyPrefabProvider.Get();
                var prefabName = prefab.name;
                
                TemporaryModel.Create(prefab);
                DummyPrefabProvider.ClearTemporaryPrefab();

                var instantiatedPrefab = GameObject.Find(prefabName);
                Assert.IsNotNull(instantiatedPrefab, "temp model should be instantiated");
            }
        }

        public class DestroyMethod
        {
            [Test]
            public void Destroy_WithoutNullObject()
            {
                DummyPrefabProvider.CreateTemporaryPrefab();
                var prefab = DummyPrefabProvider.Get();
                var prefabName = prefab.name;
                TemporaryModel.Create(prefab);
                DummyPrefabProvider.ClearTemporaryPrefab();

                var instantiatedObject = GameObject.Find(prefabName);
                Assert.IsNotNull(instantiatedObject, "temporary model should be instantiated");
            }
        }
    }
}
