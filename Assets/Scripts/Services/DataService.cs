using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RoundBallGame.Systems.Data;
using UnityEngine;

namespace RoundBallGame.Systems.Services
{
    public class DataService : MonoBehaviour
    {
        // Singleton pattern
        public static DataService Instance { get; private set; }
        
        // TODO - Card data goes here
        //[Header("Level Data")]
        //[SerializeField] private LevelCollectionSO levelCollection;
        
        private ProgressData ProgressData = new ProgressData();
        private string saveFilePath;
        private FileStream fileStream;
        private const string SaveFileName = "save.maru"; // Little nod to another project
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Initialize()
        {
            saveFilePath = Application.persistentDataPath + "/" + SaveFileName;
            LoadProgressData();
            if (ProgressData.LevelsProgressData == null || ProgressData.LevelsProgressData.Length == 0)
            {
                InitializeProgressData();
                SaveProgressData();
                Debug.Log("Fresh progress data created.");
            }
        }

        private void InitializeProgressData()
        {
            /*
            ProgressData.LevelsProgressData = new LevelProgressData[levelCollection.Levels.Length];
            for (int i = 0; i < levelCollection.Levels.Length; i++)
            {
                ProgressData.LevelsProgressData[i] = new LevelProgressData
                {
                    LevelIndex = i,
                    IsCompleted = false
                };
            }
            */
        }
        
        public void SaveProgressData()
        {
            BinaryFormatter formatter = new BinaryFormatter();   
            
            if(File.Exists(saveFilePath))
            {
                fileStream = new FileStream(saveFilePath, FileMode.Truncate);
                formatter.Serialize(fileStream, ProgressData);
                fileStream.Close();
            }
            else
            {
                fileStream = new FileStream(saveFilePath, FileMode.CreateNew);
                formatter.Serialize(fileStream, ProgressData);
                fileStream.Close();
            }
        }
        
        public void LoadProgressData()
        {
            BinaryFormatter formatter = new BinaryFormatter();        
            if(File.Exists(saveFilePath))
            {
                fileStream = new FileStream(saveFilePath, FileMode.Open);
                ProgressData = formatter.Deserialize(fileStream) as ProgressData;
                fileStream.Close();
            }
            else
            {
                Debug.Log("Progress data not found.");
            }
        }
        
        public void DeleteProgressData()
        {
            if(File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                Debug.Log("Progress data deleted.");
            }
        }
        
#if UNITY_EDITOR
        [ContextMenu("Delete Progress Data")]
        void ContextMenuDeleteProgressData()
        {
            saveFilePath = Application.persistentDataPath + "/" + SaveFileName;
            DeleteProgressData();
        }
#endif
    }
}
