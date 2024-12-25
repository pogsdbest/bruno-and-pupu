using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private string _savePath;

    private void Awake()
    {
        _savePath = Application.persistentDataPath + "/save.json";
        CardOrganizer.OnSaveGame += SaveGame;
    }

    private void OnDestroy()
    {
        CardOrganizer.OnSaveGame -= SaveGame;
    }

    private void SaveGame(SaveWrapper save)
    {
        StartCoroutine(SaveGameAsync(save));
    }

    public bool CheckSaveFile()
    {
        return File.Exists(_savePath);
    }

    public void LoadSaveFile(System.Action<SaveWrapper> callback)
    {
        StartCoroutine(LoadGameAsync( (save) =>
        {
            callback?.Invoke(save);
        }));
    }

    public IEnumerator SaveGameAsync(SaveWrapper save)
    {
        string json = JsonUtility.ToJson(save, true);

        yield return null;

        Task writeTask = File.WriteAllTextAsync(_savePath, json);
        while (!writeTask.IsCompleted)
        {
            yield return null;
        }
        if (writeTask.IsFaulted)
        {
            Debug.LogError("Failed to save game: " + writeTask.Exception);
        }
        else
        {
            Debug.Log($"Game saved asynchronously to: {_savePath}");
            Debug.Log("Game saved successfully!");
        }
    }

    public IEnumerator LoadGameAsync(System.Action<SaveWrapper> callback)
    {
        if (File.Exists(_savePath))
        {
            string json = null;

            yield return new WaitForEndOfFrame();

            json = File.ReadAllText(_savePath);

            SaveWrapper wrapper = JsonUtility.FromJson<SaveWrapper>(json);
            Debug.Log("Game loaded asynchronously.");

            callback?.Invoke(wrapper);
        }
        else
        {
            Debug.LogWarning("No save file found.");
        }
    }

    
}
