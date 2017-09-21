using MarkLight;
using MarkLight.Views.UI;
using UnityEngine;

public class RessourceBar : UIView {

    public Region Background;
    public Region Front;

    public SpriteAsset backgroundImage;

    public Color backgroundColor;
    public Color frontColor;
    public Color gainColor;
    public Color lossColor;

    public float fillAmount = 0.6f;

    public void setFillAmount(float fillAmount) {
        this.fillAmount = fillAmount;
    }
}
