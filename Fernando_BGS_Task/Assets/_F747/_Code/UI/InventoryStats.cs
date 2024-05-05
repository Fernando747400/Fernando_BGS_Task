using NaughtyAttributes;
using Obvious.Soap;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryStats : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private PlayerStats _playerStatsSO;
    [Required][SerializeField] private CharacterPiecesSO _equipedItemsSO;
    [Required][SerializeField] private ScriptableEventNoParam _updatedArmourChannel;
    [Required][SerializeField] private TextMeshProUGUI _healthText;
    [Required][SerializeField] private TextMeshProUGUI _damageText;
    [Required][SerializeField] private TextMeshProUGUI _speedText;

    private void OnEnable()
    {
        UpdateStats();
        _updatedArmourChannel.OnRaised += UpdateStats;
    }

    private void OnDisable()
    {
        _updatedArmourChannel.OnRaised -= UpdateStats;
    }


    private void UpdateStats()
    {
        CalculateStats();
        _healthText.text = _playerStatsSO.MaxHealth.Value.ToString();
        _damageText.text = _playerStatsSO.CurrentDamage.Value.ToString();
        _speedText.text = _playerStatsSO.CurrentSpeed.Value.ToString();
    }



    private void CalculateStats()
    {
        List<BaseItemSO> itemsList = _equipedItemsSO.GetAllCurrentItems();
        float healthTemporary = _playerStatsSO.BaseHealth.Value;
        float damageTemporary = _playerStatsSO.BaseDamage.Value;
        float speedTemporary = _playerStatsSO.BaseSpeed.Value;

        foreach (BaseItemSO item in itemsList)
        {
            if (!item.GrantsExtraStats) continue;
            healthTemporary += item.Health;
            damageTemporary += item.Damage;
            speedTemporary += item.Speed;
        }

        _playerStatsSO.MaxHealth.Value = healthTemporary;
        _playerStatsSO.CurrentDamage.Value = damageTemporary;
        _playerStatsSO.CurrentSpeed.Value = speedTemporary;
    }
}
