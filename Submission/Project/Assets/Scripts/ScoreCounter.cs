using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[CreateAssetMenu(menuName = "Scriptable Objects/Score Counter")]
public sealed class ScoreCounter : ScriptableObject
{
    const string PlayerDataStorePath = "/Save Data/Player";
    const string PlayerDataFileName = "/PlayerData.dbg";

    private PlayerData m_PlayerData;

    public int Score { get; private set; }
    public float LastScoreTime { get; private set; }
    public int HighScore => m_PlayerData.HighScore;

    private void OnEnable()
    {
        m_PlayerData = ReadPlayerData();
        Score = 0;

        PauseController.OnGameReload += ResetScore;
        PauseController.OnGameQuit += ResetScore;
    }

    public void AddScore(int points)
    {
        Score += points;
        LastScoreTime = Time.time;
    }

    public void ResetScore()
    {
        if (Score > HighScore)
        {
            m_PlayerData.HighScore = Score;
            SavePlayerData();
        }
        Score = 0;
    }

    public void SavePlayerData ()
    {
        if (m_PlayerData != null)
        {
            if (!Directory.Exists(Application.persistentDataPath + PlayerDataStorePath))
            {
                Directory.CreateDirectory(Application.persistentDataPath + PlayerDataStorePath);
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + PlayerDataStorePath + PlayerDataFileName, FileMode.Create);

            formatter.Serialize(stream, m_PlayerData);
            stream.Close();
        }
    }

    public PlayerData ReadPlayerData ()
    {
        if (File.Exists(Application.persistentDataPath + PlayerDataStorePath + PlayerDataFileName))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + PlayerDataStorePath + PlayerDataFileName, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }

        else return new PlayerData();
    }

    [System.Serializable]
    public class PlayerData
    {
        public int HighScore;
    }
}
