using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LifeSlider : MonoBehaviour
{
    public Slider slider;
    public PlayerLife player;

    void Update()
    {
        slider.maxValue = player.lifeMax;
        slider.value = player.life;
    }
}
