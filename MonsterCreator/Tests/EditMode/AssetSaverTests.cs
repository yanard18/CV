using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using MekaruStudios.MonsterCreator.FileSaving;

namespace MekaruStudios.MonsterCreator.Tests
{

    public abstract class AssetSaverTests
    {
        public class SaveMethod
        {

            [Test]
            public void _Save_Should_Save_Asset()
            {
                var newObject = new GameObject("temp_model");
                var fileSaver = new FileSaver();

                var rootPath = Application.dataPath;
                var folderPath = $"{rootPath}/Prefabs";
                var savedAssetPath = $"Assets/Prefabs/{newObject.name}.prefab";
                
                fileSaver.Save(newObject, folderPath);
                var savedAsset = AssetDatabase.LoadAssetAtPath<GameObject>(savedAssetPath);
                
                Assert.That(savedAsset, Is.Not.Null);
                
                
                // Clean up
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(savedAsset));
            }

            [Test]
            public void _Save_Outside_Assets_Folder_Should_Throw()
            {
                var fileSaver = new FileSaver();
                var newObject = new GameObject("temp_model");
                var savePath = Application.dataPath;
                savePath = savePath.Replace("/Assets", "");
                
                Assert.That(() => fileSaver.Save(newObject, savePath),
                    Throws.TypeOf<FilePathNotValidException>());
            }
        }
    }

}
