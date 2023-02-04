using UnityEngine;
using UnityEngine.UI;

public class TransformationBar : MonoBehaviour
{
    [SerializeField] private TransformationHandler _handler;
    [SerializeField] Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _handler = FindObjectOfType<TransformationHandler>();
    }

    private void Update()
    {
        if (_slider == null || _handler == null) return;
        _slider.value = _handler.GetBarPercentage();
    }
}
