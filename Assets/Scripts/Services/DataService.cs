using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MindlessRaptorGames
{
    public class DataService : MonoBehaviour
    {
        // Singleton pattern
        public static DataService Instance { get; private set; }
        
        [Header("Game Data")]
        [SerializeField] private CardCollectionSO cardCollection;
        [SerializeField] private EncounterCollectionSO encounterCollection;
        
        private ProgressData progressData = null;
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
            if (progressData == null)
            {
                InitializeProgressData();
                SaveProgressData();
                Debug.Log("Fresh progress data created.");
            }
        }

        private void InitializeProgressData()
        {
            progressData ??= new ProgressData();
            SetGameInProgress(false);
        }
        
        public void SaveProgressData()
        {
            BinaryFormatter formatter = new BinaryFormatter();   
            
            if(File.Exists(saveFilePath))
            {
                fileStream = new FileStream(saveFilePath, FileMode.Truncate);
                formatter.Serialize(fileStream, progressData);
                fileStream.Close();
            }
            else
            {
                fileStream = new FileStream(saveFilePath, FileMode.CreateNew);
                formatter.Serialize(fileStream, progressData);
                fileStream.Close();
            }
        }
        
        public void LoadProgressData()
        {
            BinaryFormatter formatter = new BinaryFormatter();        
            if(File.Exists(saveFilePath))
            {
                fileStream = new FileStream(saveFilePath, FileMode.Open);
                progressData = formatter.Deserialize(fileStream) as ProgressData;
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

        public List<Card> GetCardCollection(GameFlowController flowController)
        {
            List<Card> cards = new List<Card>();
            foreach (var cardSO in cardCollection.Cards)
            {
                cards.Add(cardSO.ToCard(flowController));
            }
            return cards;
        }

        public List<Card> GetStartingDeck(GameFlowController flowController)
        {
            List<Card> cards = new List<Card>();
            foreach (var cardSO in cardCollection.StartingDeck)
            {
                cards.Add(cardSO.ToCard(flowController));
            }
            return cards;
        }
        
        public List<EncounterData> GetEncounterCollection()
        {
            List<EncounterData> encounters = new List<EncounterData>();
            foreach (var encounter in encounterCollection.Encounters)
            {
                encounters.Add(encounter.ToEncounter());
            }
            return encounters;
        }
        
        public bool HasGameInProgress()
        {
            return progressData?.GameInProgress ?? false;
        }
        
        public bool SetGameInProgress(bool value)
        {
            return progressData.GameInProgress = value;
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
