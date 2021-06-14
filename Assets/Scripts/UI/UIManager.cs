using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UIManager : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] float blinkTime = 0.2f;
    [SerializeField] int blinkCount = 8;
    [HideInInspector] public bool IsSlotAnim;

    [Header("Cursor")]
    [SerializeField] Image cursor = default;

    [Header("Tutorial UI")]
    [SerializeField] GameObject tutorial = default;
    public Text clickToContinue = default;

    [Header("Main Menu UI")]
    [SerializeField] GameObject mainMenu = default;
    [SerializeField] Text speech = default;
    [SerializeField] string[] speechTexts = default;
    [SerializeField] GameObject chest = default;
    [SerializeField] Sprite openChest;
    [SerializeField] Sprite closedChest;
    int speechId = 0;

    [Header("Characters UI")]
    [SerializeField] GameObject charactersMenu = default;
    [SerializeField] GameObject slotsPanel = default;
    [SerializeField] CharacterSlot charSlotPrototype = default;
    [SerializeField] CharacterDescription[] descriptions = default;
    CharacterSlot[] charSlots;

    [Header("Pause Menu")]
    [SerializeField] GameObject pauseMenu = default;
    [SerializeField] public Text statusText = default;

    [Header("Game UI")]
    [SerializeField] GameObject gameUI = default;
    [SerializeField] Image cooldownMaskFirst = default;
    [SerializeField] Image cooldownMaskSecond = default;

    [Header("Game UI Top Bar")]
    [SerializeField] GameObject topBar = default;
    [SerializeField] HealthBar healthBar = default;
    [SerializeField] Image characterIcon = default;
    [SerializeField] Sprite[] icons = default;
    public Text Counter;

    [Header("Win Screen")]
    [SerializeField] GameObject win = default;

    new AudioPrefab audio;

    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<UIManager>();
            return _instance;
        }
    }

    private void Start()
    {
        audio = GetComponent<AudioPrefab>();
        charSlots = new CharacterSlot[descriptions.Length];
        for (int i = 0; i < descriptions.Length; i++)
        {
            CharacterSlot slot = Instantiate(charSlotPrototype, slotsPanel.transform);
            slot.charDescription = descriptions[i];
            slot.Init(i);
            slot.gameObject.GetComponent<Button>().onClick.AddListener(StartGame);
            charSlots[i] = slot;
        }
    }

    private void Update()
    {
        Vector3 cursorPos = Input.mousePosition;
        cursor.transform.position = new Vector2(cursorPos.x, cursorPos.y);

        if (GameManager.Instance.ReadyToChangeUI)
            SwitchUI();
    }

    private void SwitchUI()
    {
        switch (GameManager.Instance.GameState)
        {
            case GameStates.TUTORIAL:
                if (Input.GetButtonDown("Fire1"))
                {
                    GameManager.Instance.GameState = GameStates.MAIN_MENU;
                }
                if (!tutorial.activeSelf)
                {
                    tutorial.SetActive(true);
                    mainMenu.SetActive(false);
                    charactersMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    gameUI.SetActive(false);
                    win.SetActive(false);
                }
                break;
            case GameStates.MAIN_MENU:
                if (!mainMenu.activeSelf)
                {
                    tutorial.SetActive(false);
                    mainMenu.SetActive(true);
                    charactersMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    gameUI.SetActive(false);
                    win.SetActive(false);
                }
                break;
            case GameStates.STORY:
                if (Input.GetButtonDown("Fire1"))
                {
                    audio.Play("enemy_hit", 0.5f);
                    if (speechId >= speechTexts.Length)
                    {
                        GameManager.Instance.GameState = GameStates.CHARACTERS;
                        return;
                    }
                    speech.text = speechTexts[speechId];
                    speechId++;
                }
                break;
            case GameStates.CHARACTERS:
                if (!charactersMenu.activeSelf)
                {
                    for (int i = 0; i < charSlots.Length; i++)
                    {
                        if (!GameManager.Instance.IsHeroLive[i])
                        {
                            charSlots[i].gameObject.GetComponent<Button>().interactable = false;
                            charSlots[i].icon.sprite = charSlots[i].charDescription.deadIcon;
                            charSlots[i].play.text = "DEAD";
                        }
                    }
                    tutorial.SetActive(false);
                    mainMenu.SetActive(false);
                    charactersMenu.SetActive(true);
                    pauseMenu.SetActive(false);
                    gameUI.SetActive(false);
                    win.SetActive(false);
                }
                break;
            case GameStates.GAME:
                if (!gameUI.activeSelf)
                {
                    tutorial.SetActive(false);
                    mainMenu.SetActive(false);
                    charactersMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    gameUI.SetActive(true);
                    topBar.SetActive(false);
                    win.SetActive(false);

                    characterIcon.sprite = icons[GameManager.Instance.SelectedHero];
                }
                break;
            case GameStates.PAUSE:
                if (!pauseMenu.activeSelf)
                {
                    tutorial.SetActive(false);
                    mainMenu.SetActive(false);
                    charactersMenu.SetActive(false);
                    pauseMenu.SetActive(true);
                    win.SetActive(false);
                    gameUI.SetActive(true);
                }
                break;
        }
    }

    public void StartStory()
    {
        GameManager.Instance.GameState = GameStates.STORY;

        chest.GetComponent<Image>().sprite = openChest;
        chest.GetComponent<Button>().interactable = false;
        audio.Play("enemy_hit", 0.5f);
        speech.text = speechTexts[speechId++];
    }

    public void StartGame()
    {
        if(IsSlotAnim == false)
            StartCoroutine(SlotAnim());
    }

    public void Reset()
    {
        chest.GetComponent<Image>().sprite = closedChest;
        chest.GetComponent<Button>().interactable = true;
        speechId = 0;
        speech.text = "Open chest to play the game";
        for (int i = 0; i < charSlots.Length; i++)
        {
            charSlots[i].gameObject.GetComponent<Button>().interactable = true;
            charSlots[i].icon.sprite = charSlots[i].charDescription.icon;
            charSlots[i].play.text = "PLAY";
        }
    }

    public IEnumerator SlotAnim()
    {
        IsSlotAnim = true;
        audio.Stop();
        audio.Play("character_select");
        CharacterSlot selected = EventSystem.current.currentSelectedGameObject.GetComponent<CharacterSlot>();
        selected.gameObject.GetComponent<Button>().interactable = false;
        Image img = selected.gameObject.GetComponent<Image>();
        GameManager.Instance.SelectedHero = selected.id;

        for (int i = 0; i < blinkCount; i++)
        {
            if (img.color == Color.black)
                img.color = Color.white;
            else img.color = Color.black;
            yield return new WaitForSeconds(blinkTime);
        }
        img.color = Color.white;
        yield return new WaitForSeconds(1);
        IsSlotAnim = false;

        GameManager.Instance.GameState = GameStates.GAME;
    }

    public IEnumerator CooldownStart(float time, int id)
    {
        float maxTime = time;
        Image mask = id == 0 ? cooldownMaskFirst : cooldownMaskSecond;
        while (time > 0)
        {
            mask.fillAmount = Remap(time, 0, maxTime, 1f, 0f);
            time -= Time.deltaTime;
            yield return null;
        }
        mask.fillAmount = 1;
    }

    public IEnumerator BlinkAnim(Text obj, GameStates inState)
    {
        while (GameManager.Instance.GameState == inState)
        {
            if (obj.color == Color.white)
                obj.color = Color.black;
            else
                obj.color = Color.white;
            yield return new WaitForSeconds(blinkTime);
        }
    }

    public void ShowWinScreen()
    {
        tutorial.SetActive(false);
        mainMenu.SetActive(false);
        charactersMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameUI.SetActive(false);
        win.SetActive(true);
    }

    public void SetHP(int hp)
    {
        healthBar.ShowHP(hp);
    }

    public void EnableTopBar()
    {
        topBar.SetActive(true);
    }

    float Remap(float value, float a1, float a2, float b1, float b2)
    {
        return b1 + (value - a1) * (b2 - b1) / (a2 - a1);
    }
}
