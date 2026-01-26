using LikeLion.LH1.Client.Core;
using LikeLion.LH1.Client.Core.OmokScene;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace LikeLion.LH1.Client.UnityWorld.OmokScene
{
    public class AIConsole : IAIConsole
    {
        private string _apiKey = "AIzaSyAPYXovjVADpIHMEvkSemGyUpKSVWgwH8s";
        private string _url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key=";
        private readonly Core.ILogger _logger;

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

        public AIConsole(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<Tuple<int, int>> RequestStonePoint(int stoneType, int[][] array, CancellationToken token)
        {
            try
            {
                var whiteStones = new List<int[]>();
                var blackStones = new List<int[]>();

                for (var i = 0; i < array.Length; i++)
                {
                    for (var j = 0; j < array[i].Length; j++)
                    {
                        if (array[i][j] == StoneType.White)
                            whiteStones.Add(new int[2] { i, j });
                        if (array[i][j] == StoneType.Black)
                            blackStones.Add(new int[2] { i, j });
                    }
                }

                var move = await GetAiMoveAsync(19, stoneType, blackStones, whiteStones, 0, token);

                return new Tuple<int, int>(move.x, move.y);
            }
            catch (OperationCanceledException)
            {
                _logger.Info("AI 요청이 취소되었습니다.");
                return default;
            }
            catch (Exception e)
            {
                _logger.Fatal(e.Message);
                return default;
            }
        }

        private async Task<AiResponse> GetAiMoveAsync(int size, int turn, List<int[]> blackStones, List<int[]> whiteStones,
            int difficulty, CancellationToken token)
        {
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
- Board size : {size}x{size}
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
Respond ONLY with raw JSON.
Do not include explanations, comments, code blocks, or any other text.
If you output anything other than raw JSON, the answer will be INVALID.
Output MUST look exactly like this format:
{{
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

            using (UnityWebRequest www = new UnityWebRequest(_url + _apiKey, "POST"))
            {
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");

                var operation = www.SendWebRequest();

                while (!operation.isDone)
                {
                    if (token.IsCancellationRequested)
                    {
                        www.Abort();
                        token.ThrowIfCancellationRequested();
                    }
                    await Task.Yield();
                }

                if (www.result != UnityWebRequest.Result.Success)
                {
                    throw new Exception(www.error);
                }

                var root = JsonConvert.DeserializeObject<GeminiResponse>(www.downloadHandler.text);
                string aiText = root.candidates[0].content.parts[0].text;

                string cleanedJson = aiText.Replace("```json", "").Replace("```", "").Trim();
                cleanedJson = ExtractJson(cleanedJson);

                _logger.Info(cleanedJson);

                return JsonConvert.DeserializeObject<AiResponse>(cleanedJson);
            }
        }

        public static string ExtractJson(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var match = Regex.Match(input, @"\{[\s\S]*\}", RegexOptions.Singleline);

            return match.Success ? match.Value : string.Empty;
        }
    }
}
