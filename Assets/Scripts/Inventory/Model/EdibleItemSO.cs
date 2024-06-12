using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField] private List<ModifierData> _modifierDatas = new List<ModifierData>();
        
        public string ActionName => "Исп.";
        
        [field: SerializeField]
        public AudioClip actionSfx { get; private set; }
        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            foreach (var data in _modifierDatas)
            {
                data.statModifier.AffectCharacter(character, data.value);
            }
            
            AudioSource.PlayClipAtPoint(actionSfx, character.transform.position);
            
            return true;
        }
    }

    public interface IDestroyableItem
    {
        
    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSfx { get; }
        bool PerformAction(GameObject gameObject, List<ItemParameter> itemState);
    }

    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifierSO statModifier;
        public float value;
    }
}