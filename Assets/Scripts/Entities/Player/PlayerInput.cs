using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Player player;
    VariableJoystick moveJoystick;
    VariableJoystick attackJoystick;
    private static PlayerInput _instance;
    public static PlayerInput Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType<PlayerInput>();
            return _instance;
        }
    }

    private Vector2 playerInput;

    public Vector2 GetPlayerInput => playerInput; 

    private void Start()
    {
        player = GetComponent<Player>();
        moveJoystick = UIManager.Instance.moveJoystick.GetComponent<VariableJoystick>();
        attackJoystick = UIManager.Instance.attackJoystick.GetComponent<VariableJoystick>();
    }

    private void Update()
    {
#if UNITY_ANDROID
        playerInput = moveJoystick.Direction;
        if (attackJoystick.Direction.sqrMagnitude > 0)
            Attack(0);
#else
        playerInput.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            playerInput.y = 1;
        }
        else playerInput.y = 0;
        if (Input.GetButtonUp("Jump"))
        {
            playerInput.y = 2;
        }

        if (Input.GetButtonDown("Down"))
        {
            playerInput.y = -1;
        }
        else if (Input.GetButtonUp("Down"))
        {
            playerInput.y = -2;
        }

        if (Input.GetButtonDown("Fire1"))
            Attack(0);
#endif
    }
    public void Attack(int attackId)
    {
#if UNITY_ANDROID
        player.attacks[attackId].Action(player.gameObject.transform.position, attackJoystick.Direction, BeingType.PLAYER);
        print(attackJoystick.Direction);
#else
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        player.attacks[attackId].Action(player.gameObject.transform.position, direction.normalized, BeingType.PLAYER);
#endif
    }
}
