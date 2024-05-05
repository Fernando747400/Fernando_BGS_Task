using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using Obvious.Soap;

public class SliderCurrentMax : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private Slider _slider;
    [Required][SerializeField] private IntVariable _currentValue;
    [Required][SerializeField] private IntVariable _maxValue;

    [Header("Settings")]
    [SerializeField] private bool _isHealthSlider = false;
    [ShowIf("_isHealthSlider")]
    [SerializeField] private Gradient _healthGradient;
    [ShowIf("_isHealthSlider")]
    [SerializeField] private Image _fillImage;

    private void OnEnable()
    {
        _currentValue.OnValueChanged += UpdateSlider;
        _maxValue.OnValueChanged += UpdateSlider;
    }

    private void OnDisable()
    {
        _currentValue.OnValueChanged -= UpdateSlider;
        _maxValue.OnValueChanged -= UpdateSlider;
    }

    private void Start()
    {
        UpdateSlider(0);
    }

    private void UpdateSlider(int newValue)
    {
        _slider.value = ((float)_currentValue.Value / (float)_maxValue.Value);
        if (_isHealthSlider)
        {
            Color color = _healthGradient.Evaluate(_slider.value);
            _fillImage.color = color;
        }
    }
}
