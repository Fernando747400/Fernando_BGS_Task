using NaughtyAttributes;
using Obvious.Soap;
using TMPro;
using UnityEngine;

public class StoreInventoryPanelManager : InventoryPanelManager
{
    [Header("Dependencies")]
    [Required][SerializeField] private ScriptableEventNoParam _purchasedItemChannel;
    [Required][SerializeField] private TextMeshProUGUI _purchasedText;

    private void OnEnable()
    {
        _purchasedItemChannel.OnRaised += SetupItem;
        _button.onClick.AddListener(() => EquipItem());
        SetupItem();
    }

    private void OnDisable()
    {
        _purchasedItemChannel.OnRaised -= SetupItem;
        _button.onClick.RemoveListener(() => EquipItem());
    }

    protected override void SetupItem()
    {
        _button.interactable = false;
        _iconImage.color = Color.black;
        _purchasedText.gameObject.SetActive(false);
        if (_baseItem == null) return;
        _button.interactable = true;
        _iconImage.color = Color.white;

        if (_baseItem.Purchased)
        {
            _purchasedText.gameObject.SetActive(true);
            _button.interactable = false;
            _iconImage.color = Color.black;
        } else
        {
            _purchasedText.gameObject.SetActive(false);
        }
        _iconImage.sprite = _baseItem.StoreIcon;
    }
}
