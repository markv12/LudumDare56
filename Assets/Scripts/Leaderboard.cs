using UnityEngine;
using Dan.Main;
using System;
using TMPro;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour {
    public TMP_Text mainText;
    public RectTransform contentT;
    public TMP_InputField usernameInput;
    public Button updateUsernameButton;

    //https://danqzq.itch.io/leaderboard-creator
    //Name Fear of the Ungnome
    //Secret Key df8974ad2d42ca62483bab9b7fc65d6776ecba5e57a29174c8fe8404193d9f7984f8874bc5a5ac900df9898a4d8b193c88b8a77651c564c5d26f0684547055d7528af73c53481090145f567d45ab32ed03e13f7a9058c1424594cf9202affa4d6a89b287f41ea1bb1debde7435344ec703c650ee9e14e86817f31bccb85c71bf
    private const string PUBLIC_KEY = "ee89facec1e06cff06d01d69a9395beed08f62d0918ff294b2274d244d6af841";

    private void Start() {
        LoadLeaderboard();
        updateUsernameButton.onClick.AddListener(UpdateUsername);
        FieldCleaner.Attach(usernameInput);
        usernameInput.text = Username;
    }

    private void LoadLeaderboard() {
        LeaderboardCreator.GetLeaderboard(PUBLIC_KEY, (Dan.Models.Entry[] entries) => {
            string text = "";
            for (int i = 0; i < entries.Length; i++) {
                Dan.Models.Entry entry = entries[i];
                text += entry.Username + ": " + entry.Score + Environment.NewLine;
            }
            mainText.text = text;
            mainText.ForceMeshUpdate();
            contentT.sizeDelta = contentT.sizeDelta.SetY(mainText.renderedHeight + 80);
        });
    }

    public static void UploadNewScore(int score) {
        LeaderboardCreator.UploadNewEntry(PUBLIC_KEY, Username, score);
    }

    private const string USERNAME_KEY = "USERNAME";
    private static string Username {
        get {
            if (PlayerPrefs.HasKey(USERNAME_KEY)) {
                return PlayerPrefs.GetString(USERNAME_KEY);
            } else {
                string username = "Anonymous";
                PlayerPrefs.SetString(USERNAME_KEY, username);
                return username;
            }
        }
        set {
            PlayerPrefs.SetString(USERNAME_KEY, value);
        }
    }

    private void UpdateUsername() {
        if (!string.IsNullOrWhiteSpace(usernameInput.text)) {
            Username = usernameInput.text;
            LeaderboardCreator.UpdateEntryUsername(PUBLIC_KEY, Username, (bool success) => {
                if (success) {
                    LoadLeaderboard();
                }
            });
        }
    }
}
