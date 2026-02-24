using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Omok.States;
using UnityEngine;

namespace Omok
{

    public static class HistoryManager
    {

        public static void HistorySave(BaseState player1, BaseState player2, string result, List<Move> moves)
        {
            var now = DateTime.Now;
            HistorySheet historySheet = new HistorySheet(
                player1: player1 is PlayerState ? "Human" : "AI",
                player2: player2 is PlayerState ? "Human" : "AI",
                result: result,
                dateTime: now,
                moves: moves
            );

            Save(historySheet, $"history_{now.ToString("yyyy_MM_dd_HH_mm_ss")}.json");
        }

        public static void Save<T>(T data, string fileName)
        {
            string dir = Path.Combine(Application.persistentDataPath, "history");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string path = Path.Combine(dir, fileName);

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("저장 위치: " + path);
        }

        public static T Load<T>(string fileName)
        {
            string dir = Path.Combine(Application.persistentDataPath, "history");
            string path = Path.Combine(dir, fileName);
            if (!File.Exists(path)) return default;

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }
        
        public static List<string> GetHistoryFiles()
        {
            string dir = Path.Combine(Application.persistentDataPath, "history");
            if (!Directory.Exists(dir)) return new List<string>();

            string[] files = Directory.GetFiles(dir, "history_*.json");
            List<string> fileNames = new List<string>();
            foreach (string file in files)
            {
                fileNames.Add(Path.GetFileName(file));
            }
            return fileNames;
        }

    }

    public struct HistorySheet
    {

        public string player1;
        public string player2;
        public string result;
        public DateTime dateTime;
        public List<Move> moves;

        public HistorySheet(string player1, string player2, string result, DateTime dateTime, List<Move> moves)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.result = result;
            this.dateTime = dateTime;
            this.moves = moves;
        }

    }

}