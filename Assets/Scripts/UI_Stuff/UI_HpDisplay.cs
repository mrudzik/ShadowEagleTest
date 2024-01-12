using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_HpDisplay : MonoBehaviour
{
	public Slider hpSlider;
	public TextMeshProUGUI hpTextNum;


	public void RefreshHP(float currentHP, float maxHP)
	{
		hpSlider.value = currentHP / maxHP;
		hpTextNum.text = currentHP.ToString();

		if (currentHP <= 0)
		{
			hpSlider.gameObject.SetActive(false);
			hpTextNum.gameObject.SetActive(false);
		}
	}

}
