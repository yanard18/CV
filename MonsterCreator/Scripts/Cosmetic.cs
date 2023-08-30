using System;
using UnityEditor;
using UnityEngine;

namespace MekaruStudios.MonsterCreator
{
    public class Cosmetic
    {
        public readonly GameObject prefab;
        public readonly string slotName;
        
        GUIContent gui;
        Texture2D previewIcon;

        public Cosmetic(GameObject prefab, string slotName)
        {
            this.prefab = prefab;
            this.slotName = slotName;
            this.previewIcon = null;
        }

        public GUIContent GetGUIContent()
        {
            if (gui == null && previewIcon == null)
            {
                previewIcon = AssetPreview.GetAssetPreview(prefab);

                if (previewIcon != null)
                    gui = new GUIContent(previewIcon);

            }

            return gui;
        }

    }
}
