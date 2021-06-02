using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UIManager : MonoBehaviour
{
    [Header("Cursor")]
    [SerializeField] Image cursor;

    [Header("Main Menu UI")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] Text speech;
    [SerializeField] string[] speechTexts;
    int speechId = 0;

    [Header("Characters UI")]
    [SerializeField] GameObject charactersMenu;
    [SerializeField] GameObject slotsPanel;
    [SerializeField] CharacterSlot charSlotPrototype;
    [SerializeField] CharacterDescription[] descriptions;
    CharacterSlot[] charSlots;

    [Header("Pause Menu")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Text statusText;

    [Header("Game UI")]
    [SerializeField] GameObject gameUI;

    [Header("Top Bar")]
    [SerializeField] HealthBar bar;
    public Text counter;

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
        charSlots = new CharacterSlot[descriptions.Length];
        for (int i = 0; i < descriptions.Length; i++)
        {
            CharacterSlot slot = Instantiate(charSlotPrototype);
            slot.charDescription = descriptions[i];
            slot.Init(i);
            slot.transform.SetParent(slotsPanel.transform);
            slot.gameObject.GetComponent<Button>().onClick.AddListener(StartGame);
            charSlots[i] = slot;
        }
    }

    private void Update()
    {
        cursor.transform.position = Input.mousePosition;

        switch (GameManager.Instance.GetState)
        {
            case GameStates.MAIN_MENU:
                if (!mainMenu.activeSelf)
                {
                    mainMenu.SetActive(true);
                    charactersMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    gameUI.SetActive(false);
                }
                break;
            case GameStates.STORY:
                if (Input.GetButtonDown("Fire1"))
                {
                    if(speechId >= speechTexts.Length)
                    {
                        GameManager.Instance.SetState(GameStates.CHARACTERS);
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
                        if (!GameManager.Instance.isHeroesLive[i])
                        {
                            charSlots[i].gameObject.GetComponent<Button>().interactable = false;
                            charSlots[i].icon.sprite = charSlots[i].charDescription.deadIcon;
                        }
                    }
                    mainMenu.SetActive(false);
                    charactersMenu.SetActive(true);
                    pauseMenu.SetActive(false);
                    gameUI.SetActive(false);
                }
                break;
            case GameStates.GAME:
                if (!gameUI.activeSelf)
                {
                    mainMenu.SetActive(false);
                    charactersMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    gameUI.SetActive(true);
                }
                else if (pauseMenu.activeSelf)
                {
                    mainMenu.SetActive(false);
                    charactersMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    gameUI.SetActive(true);
                }
                break;
            case GameStates.PAUSE:
                if (!pauseMenu.activeSelf)
                {
                    mainMenu.SetActive(false);
                    charactersMenu.SetActive(false);
                    pauseMenu.SetActive(true);
                    gameUI.SetActive(true);
                }
                break;
        }
    }

    public void StartStory()
    {
        GameManager.Instance.SetState(GameStates.STORY);
        speech.text = speechTexts[speechId];
        speechId++;
    }

    public void StartGame()
    {
        GameManager.Instance.SetState(GameStates.GAME);
        GameManager.Instance.selectedHero = EventSystem.current.currentSelectedGameObject.GetComponent<CharacterSlot>().id;
    }

    public void SetHP(int hp)
    {
        bar.ShowHP(hp);
    }

    public void OnRestart()
    {
        GameManager.Instance.SetState(GameStates.CHARACTERS);
    }

    public void OnExit()
    {
        GameManager.Instance.SetState(GameStates.MAIN_MENU);
    }
}
