using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MekaruStudios.MonsterCreator.Testing
{


    public abstract class CosmeticTests
    {
        public class GetGUIContentMethod
        {
            GameObject testPrefab;
            const string COSMETIC_SLOT_NAME = "hat";
            

            [SetUp]
            public void Setup()
            {
                testPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            }
            
            [UnityTest]
            [Explicit, Category("integration")]
            public IEnumerator Cosmetic_GetGuiContent_ReturnsNonNullGuiContent()
            {
                var cosmetic = new Cosmetic(testPrefab, COSMETIC_SLOT_NAME);
                const int maxRetries = 5;
                var retries = 0;
                GUIContent guiContent = null;

                while (retries < maxRetries && guiContent == null)
                {
                    guiContent = cosmetic.GetGUIContent();
                    retries++;
                    yield return new WaitForSeconds(.5f);
                }
                
                Assert.That(guiContent, Is.Not.Null);

            }

            [TearDown]
            public void CleanUp()
            {
                Object.DestroyImmediate(testPrefab);
            }
            
        }
    }

}
