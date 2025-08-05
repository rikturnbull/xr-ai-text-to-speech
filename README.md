# XR AI Text To Speech Sample

A Unity XR sample project demonstrating AI text-to-speech functionality using OpenAI's TTS API.

![Demo](demo.png)

## Features

- OpenAI Text-to-Speech integration
- Multiple TTS models support (tts-1, tts-1-hd, gpt-4o-mini-tts)
- Various voice options (alloy, echo, fable, onyx, nova, shimmer)

## OpenAI SDK

This project uses an [independently developed OpenAI SDK for Unity](https://github.com/RageAgainstThePixel/com.openai.unity).

## Setup

1. Clone or download this project
2. Open the project in Unity
3. Navigate to `Resources/OpenAIConfiguration`
4. Set your OpenAI API Key in the configuration file
5. Play the scene and click on a character to hear them speak!

⚠️ **Important**: Do not commit the `OpenAIConfiguration` file to version control as it contains your API key. Add it to your `.gitignore` file.

## Quick Overview

1. The `TTSBehaviour` orchestrates the text-to-speech and a little animation
2. `TTSOpenAI` is a reusable component integrated with Unity
2. Select different models and voices to try them out
3. Modify the text boxes for a different conversation

## Core Integration

```
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
```