using UnityEngine;
using System.Collections;

public class EmptySoundObject : MonoBehaviour,
    IHandle<LoadLevelMessage>,
//    IHandle<DestroyLevelMessage>,
    IHandle<LevelIsDestroyedMessage>
{

	// Use this for initialization
    private static bool created = false;
    SoundManager m_SoundManager;
    public GameObject m_MainMenu;
    private bool HasDestroyedBeenCalled = true;
    #region Awake()
    void Awake()
    {
        if (!created)
        {
            EventAggregatorManager.AddEventAggregator(GameEventAggregator.GameMessenger);
        }
        else
        {
            m_MainMenu.SetActive(true);
            Destroy(this.gameObject);
        }
    }
    #endregion

	void Start () {
        if (!created)
        {
            GameEventAggregator.GameMessenger.Subscribe(this);
            m_SoundManager = gameObject.GetComponent<SoundManager>() as SoundManager;
            LoadSounds();
            GameEventAggregator.GameMessenger.Update();
            EventAggregatorManager.Publish(new PlaySoundMessage("lvlMusic", true));
        }
	}
	
	// Update is called once per frame
	void Update () {
        GameEventAggregator.GameMessenger.Update();
	}
    void LoadSounds()
    {
        Debug.Log("Loading Sounds");
        EventAggregatorManager.Publish(new LoadSoundMessage("hotdogStep", "SFX/hotdogStepSound", 2));					// Load player movement sound
        EventAggregatorManager.Publish(new LoadSoundMessage("splat", "SFX/condimentSound", 5));						// Load condiment acquired sound
        EventAggregatorManager.Publish(new LoadSoundMessage("switch", "SFX/switchSound", 1));							// Load the switch sound
        EventAggregatorManager.Publish(new LoadSoundMessage("teleport", "SFX/teleportSound", 3));						// Load the teleport sound
        EventAggregatorManager.Publish(new LoadSoundMessage("goal", "SFX/goalSound", 1));								// Load the goal sound
        EventAggregatorManager.Publish(new LoadSoundMessage("death", "SFX/deathSound", 1));							// Load the death sound
        EventAggregatorManager.Publish(new LoadSoundMessage("lvlMusic", "Music/music1", 1));							// Load some background music
    }
    public void Handle(LoadLevelMessage message)
    {
        LevelGeneratorScript.sLevel = message.LevelToLoad;
        if (HasDestroyedBeenCalled)
        {
            Debug.Log("Loading Level " + message.LevelToLoad);
            m_MainMenu.SetActive(false);

            Application.LoadLevelAdditive(message.LevelToLoad);
            HasDestroyedBeenCalled = false;
            LevelGeneratorScript.CallLoadLevelMessage = false;
        }
        else
        {
            LevelGeneratorScript.CallLoadLevelMessage = true;
        }
    }

    public void Handle(LevelIsDestroyedMessage message)
    {
        HasDestroyedBeenCalled = true;
    }
}
