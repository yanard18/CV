using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MekaruStudios.MonsterCreator
{
    [Serializable]
    public struct CosmeticSlot
    {
        public string slotName;
        public string bonePath;
        public Vector3 position;

        public CosmeticSlot(string slotName, string bonePath, Vector3 position)
        {
            this.slotName = slotName;
            this.bonePath = bonePath;
            this.position = position;
        }
    }
}
