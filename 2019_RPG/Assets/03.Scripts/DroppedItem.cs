using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : Interactable {
    public Item item;
    public GameObject _particle;

    private void Start()
    {
        base.Start();
        _collider.enabled = false;

        if (item.isStoreAble)
        {
            _particle = Resources.Load("Particle/DestroyEffect", typeof(GameObject)) as GameObject;
            _particle = Instantiate(_particle);
            //_particle.transform.SetParent(this.transform);
            _particle.transform.position = transform.position;
        }
        else
        {
            switch (item.category)
            {
                case ItemCategory.Heal:
                    _particle = Instantiate(Resources.Load("/Particle/DroppedHealEffect")) as GameObject;
                    break;
                case ItemCategory.Gold:
                    _particle = Instantiate(Resources.Load("/Particle/DroppedGoldEffect")) as GameObject;
                    break;
                case ItemCategory.Exp:
                    _particle = Instantiate(Resources.Load("/Particle/DroppedExpEffect")) as GameObject;
                    break;
            }
        }

        StartCoroutine(DelayDetectTrigger());
    }

    protected override void InteractToPlayer(Collider other)
    {
        CharacterInfo characterInfo = other.GetComponent<CharacterInfo>();

        if (characterInfo == null) return;

        if (item.isStoreAble)
        {
            characterInfo._inventory.Add(item);
        }
        else
        {
            switch (item.category)
            {
                case ItemCategory.Heal:
                    break;
                case ItemCategory.Gold:
                    break;
                case ItemCategory.Exp:
                    break;
            }
        }
        _particle.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);

        //StartCoroutine(DestoryCoroutine());
    }

    IEnumerator DelayDetectTrigger()
    {
        yield return new WaitForSeconds(1.0f);
        _collider.enabled = true;
    }

}
