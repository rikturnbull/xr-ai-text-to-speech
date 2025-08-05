using System.Threading.Tasks;
using OpenAI;
using OpenAI.Audio;
using System;
using UnityEngine;

public class TTSOpenAI
{
    private static OpenAIClient _openAIClient;

    private static OpenAIClient GetOpenAIClient()
    {
        if (_openAIClient != null)
        {
            return _openAIClient;
        }
        return new OpenAIClient(Resources.Load<OpenAIConfiguration>("OpenAIConfiguration"));
    }

    public static async Task<AudioClip> Execute(string text, string model, string voice)
    {
        try
        {
            SpeechRequest request = new(text, model, voice);
            AudioClip speechClip = await GetOpenAIClient().AudioEndpoint.GetSpeechAsync(request);
            return speechClip;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in TTSOpenAI.Execute: {ex.Message}");
            return null;
        }
    }
}
