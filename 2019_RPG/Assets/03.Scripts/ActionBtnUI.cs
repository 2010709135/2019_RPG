using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBtnUI : MonoBehaviour {
    PlayerController _PlayerController;

    private Coroutine chainCoroutine = null;

    // Use this for initialization
    void Start () {
        _PlayerController = PlayerController.instance;

    }
    
    public void DashBtnPressed()
    {
        _PlayerController.SetDashTrue();
    }

    public void AttackBtnPressed(Image Btn)
    {
        _PlayerController.AttackChain(this, Btn);
    }

    public void Skill_1_Pressed(Image Btn)
    {
        _PlayerController.Skill_1_Attack(this, Btn);
    }

    public void Skill_2_Pressed(Image Btn)
    {
        _PlayerController.Skill_2_Attack(this, Btn);
    }

    public void Skill_3_Pressed(Image Btn)
    {
        _PlayerController.Skill_3_Attack(this, Btn);
    }

    public void LowerSkillCoolTimeImage(Image image, float duration)
    {
        StartCoroutine(LowerSkillImage(image, duration));
    }

    public void LowerChainImageAmount(Image image, float duration)
    {
        if (chainCoroutine != null)
        {
            StopCoroutine(chainCoroutine);
        }
        chainCoroutine = StartCoroutine(LowerChainImage(image, duration));
    }

    public void StopLowerChainImage()
    {
        if(chainCoroutine != null)
            StopCoroutine(chainCoroutine);
    }

    IEnumerator LowerChainImage(Image image, float duration)
    {
        image.fillAmount = 1;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            image.fillAmount = 1 - counter / duration;
            yield return null;
        }
    }

    IEnumerator LowerSkillImage(Image image, float duration)
    {
        image.fillAmount = 1;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            image.fillAmount = 1 - counter / duration;
            yield return null;
        }
    }
}
