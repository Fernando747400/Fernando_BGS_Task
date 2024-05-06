using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;
using Obvious.Soap;

public class HealthSlider : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private Slider _slider;
    [Required][SerializeField] private Image _fillImage;

    [Header("Settings")]
    [SerializeField] private bool _usesSOVariables = false;
    [SerializeField] private Gradient _healthGradient;

    [HideIf("_usesSOVariables")]
    [SerializeField] private float _currentValue;
    [HideIf("_usesSOVariables")]
    [SerializeField] private float _maxValue;

    [ShowIf("_usesSOVariables")]
    [SerializeField] private FloatVariable _currentValueSO;
    [ShowIf("_usesSOVariables")]
    [SerializeField] private FloatVariable _maxValueSO;

   
    private void OnEnable()
    {
        if (_usesSOVariables)
        {
            _currentValueSO.OnValueChanged += UpdateSlider;
            _maxValueSO.OnValueChanged += UpdateSlider;
        }
        else
        {
            _slider.value = _currentValue / _maxValue;
        }
    }

    private void Start()
    {
        UpdateSlider(0);
    }

    public void SetValues(float currentValue, float maxValue)
    {
        _currentValue = currentValue;
        _maxValue = maxValue;
        UpdateSlider(0);
    }

    private void UpdateSlider(float unusedParam)
    {
        if(_usesSOVariables)
        {
            _currentValue = _currentValueSO.Value;
            _maxValue = _maxValueSO.Value;
        }

        _slider.value = _currentValue / _maxValue;
        Color color = _healthGradient.Evaluate(_slider.value);
        _fillImage.color = color;
    }
}
