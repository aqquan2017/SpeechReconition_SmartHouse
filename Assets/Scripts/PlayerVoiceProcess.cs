using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using UnityEngine.Windows.Speech;


public class PlayerVoiceProcess : MonoBehaviour
{
    public static PlayerVoiceProcess instance;

    public Font playerFont;

    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public string[] keywords = new string[] { };
    private PhraseRecognizer recognizer;

    public string playerSpeech;
    public bool micState = false;


    private AudioSource audioSource;
    public AudioClip openObj;
    public AudioClip closeObj;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void SoundInteract(bool is_open)
    {
        if (is_open)
        {
            audioSource.PlayOneShot(openObj, 0.7f);
        }
        else
        {
            audioSource.PlayOneShot(closeObj, 0.5f);
        }
    }


    //API
    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (micState)
        {
            playerSpeech = args.text;
            Debug.Log("You said: " + playerSpeech);
        }
        else Debug.Log("MIC OFF");
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
