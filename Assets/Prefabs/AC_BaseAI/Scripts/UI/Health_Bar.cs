using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    [SerializeField]
    Gradient gradient;

    [SerializeField]
    Image Fill;

    public void Set_Max_Health(int HP)
    {
        slider.maxValue = HP;
        slider.value = HP;

        Fill.color = gradient.Evaluate(1f);
    }

    public void Set_Health(int HP)
    {
        slider.value = HP;

        Fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
