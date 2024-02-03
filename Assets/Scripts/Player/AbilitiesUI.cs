using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesUI : MonoBehaviour
{
    public Image _multiShotImage;
    public Image _shieldImage;

    private void Start()
    {
        _multiShotImage.fillAmount = 1;
        _shieldImage.fillAmount = 1;
    }

    public void UpdateUI(SpecialAbility ability)
    {
        if (ability is MultishotAbility && _multiShotImage != null)
        {
            _multiShotImage.fillAmount = ability.cooldownTimer / ability.cooldownTime;
        }
        else if (ability is ShieldAbility && _shieldImage != null)
        {
            _shieldImage.fillAmount = ability.cooldownTimer / ability.cooldownTime;
        }
    }
}
