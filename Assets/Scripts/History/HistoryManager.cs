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
        public static int NUMBER_OF_FILES = 20;
        private static string HEADER = "history";

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

            Save(historySheet, $"{HEADER}_{now.ToString("yyyy_MM_dd_HH_mm_ss")}.json");            
        }

        public static void Save<T>(T data, string fileName)
        {
            string dir = Path.Combine(Application.persistentDataPath, HEADER);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string path = Path.Combine(dir, fileName);

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log($"저장 위치: {path}");
            RemainCountFiles(NUMBER_OF_FILES);  // -- 저장할때 기존 파일들 갯수 넘치면 정리
        }

        public static T Load<T>(string fileName)
        {
            string dir = Path.Combine(Application.persistentDataPath, HEADER);
            string path = Path.Combine(dir, fileName);
            if (!File.Exists(path)) return default;

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }
        
        public static List<string> GetHistoryFiles()
        {
            string dir = Path.Combine(Application.persistentDataPath, HEADER);
            if (!Directory.Exists(dir)) return new List<string>();

            string[] files = Directory.GetFiles(dir, HEADER+"_*.json");
            List<string> fileNames = new List<string>();
            foreach (string file in files)
            {
                fileNames.Add(Path.GetFileName(file));
            }
            return fileNames;
        }

        // 히스토리 파일 삭제 - [leomanic]
        public static void DeleteHistoryFile()
        {            
            List<string> files = GetHistoryFiles();
            // 파일 갯수가 최대 저장 갯수보다 작으면 
            if(files.Count < NUMBER_OF_FILES) return;
            string fileName = files[0];     // -- 마지막으로 저장된 파일
            string dir = Path.Combine(Application.persistentDataPath, HEADER);
            string path = Path.Combine(dir, fileName);
            
            Debug.Log($"삭제 파일 : {path}");
            try
            {
                File.Delete(path);
            }
            catch (IOException e)
            {
                // 파일이 사용 중이거나 입출력 에러가 발생했을 때
                Debug.Log($"파일 삭제 중 오류 발생: {e.Message}");
            }
        }

        // 최대 저장 갯수만큼 파일 남기기 - [leomanic]
        public static void RemainCountFiles(int remainCount)
        {            
            int count = GetHistoryFiles().Count;
            
            while (count > remainCount)
            {                
                DeleteHistoryFile();
                count--;
            }
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