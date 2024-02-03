using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private SpecialAbility multishotAbility;
    [SerializeField] private SpecialAbility shieldAbility;
    private AbilitiesUI _abilitiesUI;

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

    [Header("Sound")]

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ShopSounds _sounds;

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
                    _audioSource.PlayOneShot(_sounds.purchaseApproved);
                    multishotLevel1.sprite = goldStarSprite;
                    multishotPriceText.text = multishotAbility.GetPrice(multishotAbility.currentLevel + 1).ToString();
                    _abilitiesUI._multiShotImage.fillAmount = 0;
                }
                else if (multishotAbility.currentLevel == 2)
                {
                    multishotBtn.interactable = false;
                    _audioSource.PlayOneShot(_sounds.purchaseApproved);
                    multishotLevel2.sprite = goldStarSprite;
                    multishotPriceText.text = "";
                }
            }
            else
            {
                _audioSource.PlayOneShot(_sounds.purchaseRejected);
                Debug.Log("<color=yellow>You don't have enough $.</color>");
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
                    _audioSource.PlayOneShot(_sounds.purchaseApproved);
                    shieldLevel1.sprite = goldStarSprite;
                    shieldPriceText.text = shieldAbility.GetPrice(shieldAbility.currentLevel + 1).ToString();
                    _abilitiesUI._shieldImage.fillAmount = 0;
                }
                else if (shieldAbility.currentLevel == 2)
                {
                    shieldBtn.interactable = false;
                    _audioSource.PlayOneShot(_sounds.purchaseApproved);
                    shieldLevel2.sprite = goldStarSprite;
                    shieldPriceText.text = "";
                }
            }
            else
            {
                _audioSource.PlayOneShot(_sounds.purchaseRejected);
                Debug.Log("<color=yellow>You don't have enough $.</color>");
            }
            UpdateMoneyText();
        }
    }
}

[System.Serializable]
public class ShopSounds
{
    public AudioClip purchaseRejected, purchaseApproved;
}

