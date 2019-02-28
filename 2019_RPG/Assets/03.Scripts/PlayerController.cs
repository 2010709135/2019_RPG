using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance = null;

    private PlayerController()
    {

    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }
 
    private CharacterController _controller;
    Animator _playerAnimator;

    Vector3 _moveDirection = Vector3.zero;
    private bool _isMoving = false;
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;

    private float _width;
    private float _height;
    private Vector3 _position;

    private Vector3 _touchStartPos = Vector3.zero;
    private Vector3 _touchMovingPos = Vector3.zero;

    bool _Dash;
    Vector3 _velocity;
    public float DashDistance = 5f;
    public Vector3 Drag;
    // Use this for initialization
    void Start () {
        _playerAnimator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();

        _width = (float)Screen.width / 2.0f;
        _height = (float)Screen.height / 2.0f;
        _position = new Vector3(0.0f, 0.0f, 0.0f);
        
        _Dash = false;
        _velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update () {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                Vector2 pos = touch.position;
                pos.x = (pos.x - _width) / _width;
                pos.y = (pos.y - _height) / _height;
                _touchStartPos = new Vector3(pos.x, pos.y, 0.0f);                
            }
            if (touch.phase == TouchPhase.Moved || 
                touch.phase == TouchPhase.Stationary)
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

                if (dragAmount > 0.1f)
                {
                    _isMoving = true;
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

        }
        else
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


        // moment in pc mode
        //float translation = Input.GetAxis("Vertical") * speed;
        //float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
                
        //translation *= Time.deltaTime;
        //rotation *= Time.deltaTime;

        //float v = Input.GetAxis("Vertical");
        //controller.Move(transform.forward * v * Time.deltaTime * speed);

        //if (translation > 0f)
        //{
        //    playerAnimator.SetBool("isMoving", true);
        //    playerAnimator.SetFloat("characterSpeed", translation * 15);
        //}
        //else
        //{
        //    playerAnimator.SetBool("isMoving", false);
        //    playerAnimator.SetFloat("characterSpeed", 0);

        //}
        //transform.Rotate(0, rotation, 0);
	}

    public void SetDashTrue()
    {
        if (!_Dash && _isMoving && !_playerAnimator.GetBool("Rolling"))
        {
            _Dash = true;
            _playerAnimator.SetBool("Rolling", true);
        }
    }

    // Called from animation event 
    // with rolling anim and AnimationEventHelper    
    public void SetDashFalse()
    {
        _Dash = false;
    }
}
