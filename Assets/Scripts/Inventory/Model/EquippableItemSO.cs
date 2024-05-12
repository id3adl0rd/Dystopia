using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        public string ActionName => "Equip";
        [field: SerializeField]
        public AudioClip actionSfx { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            WeaponContoller weaponContollerSystem = character.GetComponent<WeaponContoller>();
            if (weaponContollerSystem != null)
            {
                weaponContollerSystem.SetWeapon(this, itemState == null ? DefaultParametersList : itemState);
                return true;
            }
            
            return false;
        }
    }
}