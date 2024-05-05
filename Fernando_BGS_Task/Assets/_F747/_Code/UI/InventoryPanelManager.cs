using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanelManager : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] protected ScriptableEventBaseItemSO _equipItemChannel;
    [Required][SerializeField] protected Button _button;
    [Required][SerializeField] protected Image _iconImage;
    
    [SerializeField] protected BaseItemSO _baseItem;

    public BaseItemSO BaseItem { get => _baseItem; set { _baseItem = value; SetupItem(); } }

    private void OnEnable()
    {
        _button.onClick.AddListener(() => EquipItem());
        SetupItem();
    }
    private void OnDisable()
    {
        _button.onClick.RemoveListener(() => EquipItem());
    }

    protected virtual void SetupItem()
    {
        _button.interactable = false;
        _iconImage.color = Color.black;
        if (_baseItem == null) return;

        if (_baseItem.Purchased)
        {
            _button.interactable = true;
            _iconImage.color = Color.white;
        } 
        _iconImage.sprite = _baseItem.StoreIcon;
    }

    protected void EquipItem()
    {
        _equipItemChannel.Raise(_baseItem);
    }
}
