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
당신은 오목(Gomoku) 전문가입니다. 
제공된 오목판의 크기와 돌들의 위치를 분석하여, 승리하기 위한 최적의 다음 수를 제안하세요.

## Input Data Format
- 0: 빈 공간, 1: 흑돌, 2: 백돌
- 현재 차례: {turn}

## Game Setting
- Win Condition: 같은 색의 돌 5개가 가로, 세로, 혹은 대각선으로 연속되면 승리합니다.
- Coordinate System: 좌하단이 (0,0)이며, 오른쪽으로 갈수록 x가 증가, 위로 갈수록 y가 증가합니다.

## Difficulty Guidelines
0 : 상대방이 3목이나 4목을 만들면 방어하는 데 집중하며, 눈에 보이는 자신의 돌을 연결합니다.
1 : '공격이 최선의 방어'임을 이해합니다. 33이나 43을 만들기 위한 빌드업을 시도하며, 상대의 노림수를 미리 차단합니다.
2 : 렌주룰(Renju Rule) 등의 금수를 고려하거나 유도하며, 수십 수 앞을 내다보는 수읽기를 통해 필승법(VCF, VCT)을 찾아냅니다.

## Task
1. 현재 놓인 돌들의 위치를 파악하여 머릿속으로 전체 판을 그리세요.
2. 상대방의 공격 흐름(3목, 4목)을 차단해야 하는지, 혹은 본인({StoneType.White})의 승리 수(4-3 등)를 만들지 결정하세요.
3. {difficulty} 수준에 맞는 최선의 수 하나를 결정하세요.
4. 이미 돌이 놓여 있다면, 그 자리에는 돌을 놓을 수 없습니다.

현재 상태: {omokJson}
반드시 JSON 형식으로만 답변하세요: {{ ""thought"": ""이유"", ""x"": x좌표, ""y"": y좌표 }}";

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
