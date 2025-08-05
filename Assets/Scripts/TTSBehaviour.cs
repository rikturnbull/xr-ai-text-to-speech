using UnityEngine;
using System.ComponentModel;
using System.Collections;
using UnityEngine.UI;

public class TTSBehaviour : MonoBehaviour
{
    public enum Model
    {
        [Description("tts-1")]
        tts_1,
        [Description("tts-1-hd")]
        tts_1_hd,
        [Description("gpt-4o-mini-tts")]
        gpt_4o_mini_tts
    }

    public enum Voice
    {
        alloy,
        echo,
        fable,
        onyx,
        nova,
        shimmer
    }

    [SerializeField] private Model model = Model.tts_1;
    [SerializeField] private Voice voice = Voice.alloy;
    [SerializeField] private Sprite actorChatImage;
    [SerializeField] private Text text;
    [SerializeField] private string instructions;
    [SerializeField] private Image actorImage;

    public void Speak()
    {
        StartCoroutine(SpeakCoroutine(text.text, model, voice));
    }

    private IEnumerator SpeakCoroutine(string text, Model model, Voice voice)
    {
        var task = TTSOpenAI.Execute(text, model.GetDescription(), voice.ToString(), instructions);

        yield return new WaitUntil(() => task.IsCompleted);
        
        AudioClip audioClip = task.Result;
        if (audioClip != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                Sprite originalSprite = actorImage.sprite;
                
                audioSource.PlayOneShot(audioClip);
                
                StartCoroutine(AnimateActorImage(audioClip.length, originalSprite));
            }
            else
            {
                Debug.LogError("AudioSource component not found on this GameObject.");
            }
        }
    }

    private IEnumerator AnimateActorImage(float duration, Sprite originalSprite)
    {
        float elapsed = 0f;
        bool useOriginal = true;
        
        while (elapsed < duration)
        {
            actorImage.sprite = useOriginal ? originalSprite : actorChatImage;
            useOriginal = !useOriginal;
            
            yield return new WaitForSeconds(0.2f);
            elapsed += 0.2f;
        }
        
        actorImage.sprite = originalSprite;
    }
}

// Little hack to get enum to string
public static class EnumExtensions
{
    public static string GetDescription(this System.Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute == null ? value.ToString() : attribute.Description;
    }
}
