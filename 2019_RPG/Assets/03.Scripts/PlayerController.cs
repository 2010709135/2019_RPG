using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum FightType
{
    Knight,
    Archer,
    Mage
}

public class PlayerController : MonoBehaviour {
    #region Singleton
    public static PlayerController instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }
    #endregion

    private CharacterController _controller;
    Animator _playerAnimator;

    private bool _isMoving = false;
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;

    private float _width;
    private float _height;

    private int _firstTouchID;
    private Vector3 _touchStartPos = Vector3.zero;
    private Vector3 _touchMovingPos = Vector3.zero;

    bool _Dash;
    
    // values related to attack and skills
    bool _isAttacking;
    private int _attack;
    private bool canChain;
    public FightType fightType;

    Coroutine chainLockMovementRoutine;
    Coroutine chainBrokeChain;

    private bool[] _skill_Able = new bool[3];
    private bool _all_skill_able;
    private bool _isSkillActivate = false;
    
    // Use this for initialization
    void Start () {
        _playerAnimator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();

        _width = (float)Screen.width / 2.0f;
        _height = (float)Screen.height / 2.0f;
        //_position = new Vector3(0.0f, 0.0f, 0.0f);
        
        _Dash = false;
//_velocity = Vector3.zero;

        _isAttacking = false;
        _attack = 0;
        canChain = false;
        fightType = FightType.Knight;

        _skill_Able[0] = true;
        _skill_Able[1] = true;
        _skill_Able[2] = true;
        _all_skill_able = true;

        _firstTouchID = -1;
        
    }

// Update is called once per frame
    void Update () {
        foreach (Touch touch in Input.touches)
        {
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {                                
                if(_firstTouchID >= 0)
                {
                    if (_firstTouchID != touch.fingerId)
                        continue;
                }

                if (touch.phase == TouchPhase.Began)
                {
                    if(_firstTouchID <= 0)
                    {
                        _firstTouchID = touch.fingerId;
                    }
                    Vector2 pos = touch.position;
                    pos.x = (pos.x - _width) / _width;
                    pos.y = (pos.y - _height) / _height;
                    _touchStartPos = new Vector3(pos.x, pos.y, 0.0f);
                    _isMoving = true;
                }
                if (touch.phase == TouchPhase.Moved ||
                    touch.phase == TouchPhase.Stationary &&
                    _isMoving)
                {
                    Vector2 pos = touch.position;
                    pos.x = (pos.x - _width) / _width;
                    pos.y = (pos.y - _height) / _height;
                    _touchMovingPos = new Vector3(pos.x, pos.y, 0.0f);

                    Vector2 dragDir = _touchMovingPos - _touchStartPos;
                    float dragAmount = dragDir.magnitude * 7;
                    dragAmount = Mathf.Clamp(dragAmount, 0f, 1f);

                    Vector3 horizontal = new Vector3(1, 0, -1);
                    Vector3 vertical = new Vector3(1, 0, 1);

                    Vector3 moveDir = (horizontal * dragDir.x + vertical * dragDir.y).normalized;


                    if (!_isAttacking)
                    {
                        if (dragAmount > 0.1f)
                        {
                            _playerAnimator.SetBool("isMoving", true);
                            _playerAnimator.SetFloat("characterSpeed", dragAmount);
                            transform.rotation = Quaternion.LookRotation(moveDir);
                        }
                        else
                        {
                            dragDir = Vector2.zero;
                        }

                        if (!_Dash)
                        {
                            _controller.Move(moveDir * Time.deltaTime * speed * dragAmount);
                            _playerAnimator.SetBool("Rolling", false);
                        }
                    }
                }else if(touch.phase == TouchPhase.Ended)
                {
                    _firstTouchID = -1;
                }                
            } // if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId)) end
        } // foreach end


        if (Input.touchCount <= 0)
        {
            _isMoving = false;
            _playerAnimator.SetBool("isMoving", false);
            _playerAnimator.SetFloat("characterSpeed", 0);
            if (!_Dash)
            {
                _playerAnimator.SetBool("Rolling", false);
            }

            _touchStartPos = Vector3.zero;
            _touchMovingPos = Vector3.zero;
        }
    }

    public void SetDashTrue()
    {
        if (!_Dash && _isMoving && !_isAttacking)
        {
            _playerAnimator.SetBool("Rolling", true);
            _Dash = true;
            _all_skill_able = false;

            StartCoroutine(LockWhileRolling(0.5f));            
        }
    }

    IEnumerator LockWhileRolling(float pauseTime)
    {
        //_playerAnimator.SetFloat("characterSpeed", 0f);
        //_playerAnimator.SetBool("isMoving", false);
        //_isAttacking = false;
        yield return new WaitForSeconds(pauseTime);
        _playerAnimator.SetBool("Rolling", false);
        yield return new WaitForSeconds(0.15f);
        _all_skill_able = true;
        _Dash = false;
    }

    
    public void AttackChain(ActionBtnUI actionBtnUI, Image ChainImage)
    {
        if (_Dash || _isSkillActivate) return;

        if (_attack == 0)
        {
            Attack1(actionBtnUI, ChainImage);
        }
        else if (canChain)
        {
            if (_attack == 1)
            {
                Attack2(actionBtnUI, ChainImage);
            }
            else if (_attack == 2)
            {
                Attack3(actionBtnUI, ChainImage);
            }
            else
            {
                // do nothing
            }
        }
        else
        {
            // do nothing
        }
    }

    void Attack1(ActionBtnUI actionBtnUI, Image ChainImage)
    {
        if (chainBrokeChain != null && chainLockMovementRoutine != null)
        {
            StopCoroutine(chainBrokeChain);
            StopCoroutine(chainLockMovementRoutine);
        }
        canChain = false;
        //if (_playerAnimator.GetInteger("Attack") != 0) return;
        _playerAnimator.SetInteger("Attack", 1);
        _attack = 1;

        if (fightType == FightType.Knight)
        {
            chainLockMovementRoutine = StartCoroutine(LockForAttack(0.6f));
            chainBrokeChain = StartCoroutine(BrokeChainAttack(0.3f, 0.7f));
            //actionBtnUI.LowerChainImageAmount(ChainImage, 3f);
        }
    }

    void Attack2(ActionBtnUI actionBtnUI, Image ChainImage)
    {
        if (chainBrokeChain != null && chainLockMovementRoutine != null)
        {
            StopCoroutine(chainBrokeChain);
            StopCoroutine(chainLockMovementRoutine);
        }
        canChain = false;
        _playerAnimator.SetInteger("Attack", 2);
        _attack = 2;

        if (fightType == FightType.Knight)
        {
            chainLockMovementRoutine = StartCoroutine(LockForAttack(0.7f));
            chainBrokeChain = StartCoroutine(BrokeChainAttack(0.4f, 0.6f));
            //actionBtnUI.LowerChainImageAmount(ChainImage, 3f);

        }

    }

    void Attack3(ActionBtnUI actionBtnUI, Image ChainImage)
    {
        if (chainBrokeChain != null && chainLockMovementRoutine != null)
        {
            StopCoroutine(chainBrokeChain);
            StopCoroutine(chainLockMovementRoutine);
        }
        _playerAnimator.SetInteger("Attack", 3);
        _attack = 3;
        canChain = false;

        if (fightType == FightType.Knight)
        {
            chainLockMovementRoutine = StartCoroutine(LockForAttack(0.8f));
            chainBrokeChain = StartCoroutine(BrokeChainAttack(0f, 1.35f));
            //actionBtnUI.LowerChainImageAmount(ChainImage, 0.01f);
        }

    }

    IEnumerator LockForAttack(float pauseTime)
    {
        _all_skill_able = false;
        _isAttacking = true;
        yield return new WaitForSeconds(pauseTime);
        _playerAnimator.SetInteger("Attack", 0);
        yield return new WaitForSeconds(0.2f);
        _isAttacking = false;
        _all_skill_able = true;
    }

    IEnumerator BrokeChainAttack(float WaitForClip, float BrokeTime)
    {
        yield return new WaitForSeconds(WaitForClip);
        canChain = true;
        yield return new WaitForSeconds(BrokeTime);
        canChain = false;
        _attack = 0;
    }

    IEnumerator LockForSkills(float pauseTime)
    {
        _all_skill_able = false; // when one skill is activate, others can't be used
        _isSkillActivate = true; // when skill is activate, attack can be used
        _isAttacking = true;  
        // when use skill, broke attack chain
        _attack = 0;
        canChain = false;
        _playerAnimator.SetInteger("Attack", 0);
        yield return new WaitForSeconds(pauseTime);
        _isSkillActivate = false;
        _all_skill_able = true;
        _isAttacking = false;
        
    }
    
    public void Skill_1_Attack(ActionBtnUI actionBtnUI, Image CoolTimeImage)
    {
        if (_skill_Able[0] && _all_skill_able)
        {
            _playerAnimator.SetTrigger("Skill_1");
            _isAttacking = true;
            _skill_Able[0] = false;

            if (fightType == FightType.Knight)
            {
                //LockMomentForSkill();
                StartCoroutine(LockForSkills(1f));
                actionBtnUI.LowerSkillCoolTimeImage(CoolTimeImage, 3f);
                StartCoroutine(SkillCoolTimeChecker(3, 0));
            }
        }
    }

    public void Skill_2_Attack(ActionBtnUI actionBtnUI, Image CoolTimeImage)
    {
        if (_skill_Able[1] && _all_skill_able)
        {
            _playerAnimator.SetTrigger("Skill_2");
            _isAttacking = true;
            _skill_Able[1] = false;

            if (fightType == FightType.Knight)
            {
                StartCoroutine(LockForSkills(1f));
                actionBtnUI.LowerSkillCoolTimeImage(CoolTimeImage, 5f);
                StartCoroutine(SkillCoolTimeChecker(5, 1));
            }
        }
    }

    public void Skill_3_Attack(ActionBtnUI actionBtnUI, Image CoolTimeImage)
    {
        if (_skill_Able[2] && _all_skill_able)
        {
            _playerAnimator.SetTrigger("Skill_3");
            _isAttacking = true;
            _skill_Able[2] = false;

            if (fightType == FightType.Knight)
            {
                StartCoroutine(LockForSkills(1.1f));
                actionBtnUI.LowerSkillCoolTimeImage(CoolTimeImage, 4f);
                StartCoroutine(SkillCoolTimeChecker(4, 2));
            }
        }
    }

    IEnumerator SkillCoolTimeChecker(float duration, int skill_Idx)
    {
        float count = 0;

        while(count < duration)
        {
            count += Time.deltaTime;
            
            yield return null;
        }
        _skill_Able[skill_Idx] = true;
        
    }
    
}
