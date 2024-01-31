using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private SpecialAbility multishotAbility;
    [SerializeField] private SpecialAbility shieldAbility;

    [Header("Buttons")]

    [SerializeField] private Button multishotBtn;
    [SerializeField] private Button shieldBtn;

    [Header("Text")]

    [SerializeField] private TMP_Text multishotPriceText;
    [SerializeField] private TMP_Text shieldPriceText;
    [SerializeField] private TMP_Text currentMoneyText;

    [Header("Level Stars")]

    [SerializeField] private Image multishotLevel1;
    [SerializeField] private Image multishotLevel2;
    [SerializeField] private Image shieldLevel1;
    [SerializeField] private Image shieldLevel2;
    [SerializeField] private Sprite goldStarSprite;

    private AbilitiesUI _abilitiesUI;

    private void Awake()
    {
        _abilitiesUI = FindObjectOfType<AbilitiesUI>();
    }

    private void Start()
    {
        multishotPriceText.text = multishotAbility.priceLevel1.ToString() + " $";
        shieldPriceText.text = shieldAbility.priceLevel1.ToString() + " $";
        currentMoneyText.text = $"You have <color=#FFE15C>{GameManager.Instance.currentMoney} $";

        // Inicializar nivel de las habilidades
        multishotAbility.currentLevel = 0;
        shieldAbility.currentLevel = 0;
    }

    public void UpdateMoneyText()
    {
        currentMoneyText.text = $"You have <color=#FFE15C>{GameManager.Instance.currentMoney} $";
        MenuManager.Instance.UpdateMoneyText();
    }

    public void PurchaseMultiShot()
    {
        int currentLevel = multishotAbility.currentLevel;

        if (currentLevel < multishotAbility.GetMaxLevel())
        {
            int price = multishotAbility.GetPrice(currentLevel + 1);

            if (GameManager.Instance.currentMoney >= price)
            {
                GameManager.Instance.currentMoney -= price;
                multishotAbility.currentLevel++;

                if (multishotAbility.currentLevel == 1)
                {
                    FindObjectOfType<AbilityHolder>().abilities.Add(multishotAbility);
                    multishotLevel1.sprite = goldStarSprite;
                    multishotPriceText.text = multishotAbility.GetPrice(multishotAbility.currentLevel + 1).ToString();
                    _abilitiesUI._multiShotImage.fillAmount = 0;
                }
                else if (multishotAbility.currentLevel == 2)
                {
                    multishotBtn.interactable = false;
                    multishotLevel2.sprite = goldStarSprite;
                    multishotPriceText.text = "";
                }
            }
            else
            {
                Debug.LogWarning("You don't have enough $.");
            }
            UpdateMoneyText();
        }
    }

    public void PurchaseShield()
    {
        int currentLevel = shieldAbility.currentLevel;

        if (currentLevel < shieldAbility.GetMaxLevel())
        {
            int price = shieldAbility.GetPrice(currentLevel + 1);

            if (GameManager.Instance.currentMoney >= price)
            {
                GameManager.Instance.currentMoney -= price;
                shieldAbility.currentLevel++;

                if (shieldAbility.currentLevel == 1)
                {
                    FindObjectOfType<AbilityHolder>().abilities.Add(shieldAbility);
                    shieldLevel1.sprite = goldStarSprite;
                    shieldPriceText.text = shieldAbility.GetPrice(shieldAbility.currentLevel + 1).ToString();
                    _abilitiesUI._shieldImage.fillAmount = 0;
                }
                else if (shieldAbility.currentLevel == 2)
                {
                    shieldBtn.interactable = false;
                    shieldLevel2.sprite = goldStarSprite;
                    shieldPriceText.text = "";
                }
            }
            else
            {
                Debug.LogWarning("You don't have enough $.");
            }
            UpdateMoneyText();
        }
    }
}
