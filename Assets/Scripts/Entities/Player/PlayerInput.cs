using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Player player;
    new AudioPrefab audio;
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
        audio = GetComponent<AudioPrefab>();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameStates.GAME &&
            GameManager.Instance.GameState != GameStates.WIN)
            return;

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
        if (Input.GetButtonDown("Fire2"))
            Attack(1);
    }
    public void Attack(int attackId)
    {
        if (player.attacks[attackId].IsCooldownEnd())
        {
            if (attackId == 0)
                audio.Play("hero_pri_attack");
            else
                audio.Play("hero_sec_attack");
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            player.attacks[attackId].Action(player.gameObject.transform.position, direction.normalized, player);

            StartCoroutine(UIManager.Instance.CooldownStart(player.attacks[attackId].CoolDown, attackId));
        }
    }
}
