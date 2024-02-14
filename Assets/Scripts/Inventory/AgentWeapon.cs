using System;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

namespace Inventory
{
    public class AgentWeapon : MonoBehaviour
    {
        [SerializeField] private EquippableItemSO _weapon;
        [SerializeField] private InventorySO _inventoryData;
        [SerializeField] private List<ItemParameter> _parametersToModify, _itemCurrentState;

        public void SetWeapon(EquippableItemSO weaponItemSO, List<ItemParameter> itemState)
        {
            if (_weapon != null)
                _inventoryData.AddItem(_weapon, 1, _itemCurrentState);

            this._weapon = weaponItemSO;
            this._itemCurrentState = new List<ItemParameter>();
            ModifyParameters();
        }

        private void ModifyParameters()
        {
            foreach (var parameter in _parametersToModify)
            {
                if (_itemCurrentState.Contains(parameter))
                {
                    int index = _itemCurrentState.IndexOf(parameter);
                    float newValue = _itemCurrentState[index].value + parameter.value;
                    _itemCurrentState[index] = new ItemParameter
                    {
                        itemParameter = parameter.itemParameter,
                        value = newValue
                    };
                }
            }
        }
    }
}