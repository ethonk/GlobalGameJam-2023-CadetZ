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
        // set bar percentage
        if (_slider == null || _handler == null) return;
        _slider.value = _handler.GetBarPercentage();
        
        // set bar anim status
        if (_handler.transformationState == TransformationHandler.TransformationState.Caveman)
            GetComponent<Animator>().SetBool("enraged", true);
        else
            GetComponent<Animator>().SetBool("enraged", false);
    }
}
