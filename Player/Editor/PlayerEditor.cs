using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace Oblation.OdinEditor
{
    public class PlayerEditor : OdinMenuEditorWindow
    {
        [MenuItem("Oblation/Player")]
        static void OpenWindow()
        {
            GetWindow<PlayerEditor>().Show();
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            var slide = AssetDatabaseExtension.FindTypeOf<SlideStateData>();
            tree.Add("Slide", slide);
            return tree;
        }
    }
}