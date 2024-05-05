using NaughtyAttributes;
using Obvious.Soap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private ScriptableEventBaseItemSO _storeItemChannel;
    [Required][SerializeField] private ScriptableEventNoParam _purchasedItemChannel;
    [Required][SerializeField] private IntVariable _currentCoins;
    [Required][SerializeField] private Button _purchaseButton;
    [Required][SerializeField] private Image _iconImage;
    [Required][SerializeField] private TextMeshProUGUI _priceText;
    [Required][SerializeField] private TextMeshProUGUI _healthText;
    [Required][SerializeField] private TextMeshProUGUI _damageText;
    [Required][SerializeField] private TextMeshProUGUI _speedText;

    private BaseItemSO _currentItem;
    private Sprite _initialIcon;

    private void OnEnable()
    {
        _storeItemChannel.OnRaised += UpdateCurrentItem;
        _currentCoins.OnValueChanged += UpdateButton;
        _initialIcon = _iconImage.sprite;
        UpdateAll();
    }

    private void OnDisable()
    {
        _storeItemChannel.OnRaised -= UpdateCurrentItem;
        _currentCoins.OnValueChanged -= UpdateButton;
    }

    public void PurchaseItem()
    {
        if(_currentItem == null) return;
        if(_currentCoins.Value < _currentItem.Price) return;
        _currentCoins.Value -= _currentItem.Price;
        _currentItem.SaveInfo(true);
        _purchasedItemChannel.Raise();
        _currentItem = null;
        UpdateAll();
    }

    private void UpdateAll()
    {
        UpdateButton(_currentCoins.Value);
        UpdateStats();
        UpdateIconImage();
    }

    private void UpdateCurrentItem(BaseItemSO baseItem)
    {
        _currentItem = baseItem;
        UpdateAll();
    }

    private void UpdateButton(int value)
    {
        if(_currentItem != null)
        {
            _priceText.text = _currentItem.Price.ToString();
            _purchaseButton.interactable = _currentCoins.Value >= _currentItem.Price;
        } else
        {
            _priceText.text = "0";
            _purchaseButton.interactable = false;
        }
    }

    private void UpdateStats()
    {
        _healthText.text = "0";
        _damageText.text = "0";
        _speedText.text = "0";
        if (_currentItem != null)
        {
            if (!_currentItem.GrantsExtraStats) return;
            _healthText.text = _currentItem.Health.ToString();
            _damageText.text = _currentItem.Damage.ToString();
            _speedText.text = _currentItem.Speed.ToString();
        }    
    }

    private void UpdateIconImage()
    {
        if(_currentItem != null)
        {
            _iconImage.sprite = _currentItem.StoreIcon;
        }
        else
        {
            _iconImage.sprite = _initialIcon;
        }
    }
}
