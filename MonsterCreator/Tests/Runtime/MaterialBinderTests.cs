using NUnit.Framework;
using UnityEngine;

namespace MekaruStudios.MonsterCreator.Testing
{
    public abstract class MaterialBinderTests
    {
        public class BindMethod
        {
            Material testMaterial;
            GameObject dummyGameObject;

            [SetUp]
            public void CreateTestMaterial()
            {
                testMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
                dummyGameObject = new GameObject();
                dummyGameObject.AddComponent<MeshRenderer>();
            }

            [Test]
            public void _Bind_Invoke_OnTemporaryModelUpdated()
            {
                var eventInvoked = false;

                TemporaryModel.onTemporaryModelUpdated += () => eventInvoked = true;
                MaterialBinder.Bind(testMaterial, dummyGameObject);

                Assert.That(eventInvoked);
            }

            [Test]
            public void Bind_WithValidArguments()
            {
                var rendererOfDummyObject = dummyGameObject.GetComponent<Renderer>();

                MaterialBinder.Bind(testMaterial, dummyGameObject);

                Assert.That(testMaterial.shader, Is.SameAs(rendererOfDummyObject.material.shader));
            }

            [TearDown]
            public void Clean()
            {
                Object.Destroy(dummyGameObject);
            }

        }
    }
}
