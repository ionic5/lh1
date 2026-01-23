using LikeLion.LH1.Client.Core.OmokScene;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking;

namespace LikeLion.LH1.Client.UnityWorld.OmokScene
{
    public class AIPlayer : IPlayer
    {
        private string apiKey = "AIzaSyC92K2xdQh4r3tfJQPJJ4dOWCymDuPX8-o";
        private string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key=";

        [Serializable]
        public class GeminiResponse
        {
            public List<Candidate> candidates;
        }

        [Serializable]
        public class Candidate
        {
            public Content content;
            public string finishReason;
        }

        [Serializable]
        public class Content
        {
            public List<Part> parts;
            public string role;
        }

        [Serializable]
        public class Part
        {
            public string text;
        }

        [Serializable]
        public class AiResponse
        {
            public string thought;
            public int x;
            public int y;
        }

        private readonly Checkerboard _board;

        public AIPlayer(Checkerboard board)
        {
            _board = board;
        }

        public void HaltTurn()
        {

        }

        public bool IsStoneOwner(int stoneType)
        {
            return stoneType == StoneType.White;
        }

        public async void StartTurn()
        {
            try
            {
                var whiteStones = new List<int[]>();
                var blackStones = new List<int[]>();

                var array = _board.ToArray();
                for (var i = 0; i < array.Length; i++)
                {
                    for (var j = 0; j < array[i].Length; j++)
                    {
                        if (array[i][j] == StoneType.White)
                            whiteStones.Add(new int[2] { j, i });
                        if (array[i][j] == StoneType.Black)
                            blackStones.Add(new int[2] { j, i });
                    }
                }

                AiResponse move = await GetAiMoveAsync(19, StoneType.White, blackStones, whiteStones, 1);

                if (move != null)
                {
                    Debug.Log($"AI의 생각: {move.thought}");
                    Debug.Log($"추천 위치: ({move.x}, {move.y})");

                    _board.PutStone(move.x, move.y, StoneType.White);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"AI 요청 실패: {e.Message}");
            }
        }

        private async Task<AiResponse> GetAiMoveAsync(int size, int turn, List<int[]> blackStones, List<int[]> whiteStones, int difficulty)
        {
            // 1. 데이터 준비
            var omokData = new
            {
                placed_stones = new { black = blackStones, white = whiteStones }
            };

            string omokJson = JsonConvert.SerializeObject(omokData);
            string prompt = $@"
## Role
You are a Gomoku (Five in a Row) Player. 
Analyze the board size and stone positions to suggest the optimal next move for victory.

## Input Data Format
- 0: Empty, 1: Black, 2: White
- Current Turn: {turn}

## Game Setting
- Win Condition: 5 consecutive stones of the same color (horizontal, vertical, or diagonal).
- Coordinate System: Bottom-left is (0,0). X increases to the right, Y increases upwards.

## Difficulty Guidelines
0 : Defensive focus. Blocks opponent's 3 or 4 in a row and connects own stones visible on the surface.
1 : Aggressive play. Builds up for 3-3 or 4-3 threats and preemptively blocks opponent's strategic paths.
2 : Advanced tactics. Considers Renju rules (forbidden moves), performs deep look-ahead (VCF, VCT), and induces winning sequences.

## Task
1. Map the entire board state mentally based on the provided stone positions.
2. Determine whether to block the opponent's threats or create a winning formation (e.g., 4-3) for the current stone ({{StoneType}}).
3. Select the single best move according to the Difficulty Level: {difficulty}.
4. You MUST NOT place a stone where one already exists.

## Current State
{omokJson}

## Output Format
Respond ONLY in JSON format:
{{
  ""thought"": ""Reasoning for the move"",
  ""x"": x_coordinate,
  ""y"": y_coordinate
}}";

            var requestBody = new
            {
                contents = new[] {
                new { parts = new[] { new { text = prompt } } }
            }
            };

            string finalJson = JsonConvert.SerializeObject(requestBody);
            byte[] jsonToSend = Encoding.UTF8.GetBytes(finalJson);

            // 2. UnityWebRequest 설정
            using (UnityWebRequest www = new UnityWebRequest(url + apiKey, "POST"))
            {
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");

                // [중요] UnityWebRequest를 await 할 수 있게 만드는 핵심
                var operation = www.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield(); // 유니티 메인 스레드에서 완료될 때까지 대기

                if (www.result != UnityWebRequest.Result.Success)
                {
                    throw new Exception(www.error);
                }

                // 3. 응답 파싱
                var root = JsonConvert.DeserializeObject<GeminiResponse>(www.downloadHandler.text);
                string aiText = root.candidates[0].content.parts[0].text;

                // 마크다운 태그 제거 후 파싱
                string cleanedJson = aiText.Replace("```json", "").Replace("```", "").Trim();

                return JsonConvert.DeserializeObject<AiResponse>(cleanedJson);
            }
        }
    }
}
