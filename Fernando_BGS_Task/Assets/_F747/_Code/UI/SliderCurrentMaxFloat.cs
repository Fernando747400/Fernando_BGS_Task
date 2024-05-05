using UnityEngine.UI;
using NaughtyAttributes;
using Obvious.Soap;
using UnityEngine;

public class SliderCurrentMaxFloat : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private Slider _slider;
    [Required][SerializeField] private FloatVariable _currentValue;
    [Required][SerializeField] private FloatVariable _maxValue;

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

    private void UpdateSlider(float newValue)
    {
        _slider.value = _currentValue.Value / _maxValue.Value;

        if (_isHealthSlider)
        {
            Color color = _healthGradient.Evaluate(_slider.value);
            _fillImage.color = color;
        }
    }
}
