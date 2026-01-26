using LikeLion.LH1.Client.Core.OmokScene;
using LikeLion.LH1.Client.UnityWorld.OmokScene;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LikeLion.LH1.Client.UnityWorld
{
    public class Starter : MonoBehaviour
    {
        [SerializeField]
        private View.OmokScene.Checkerboard _checkerboard;
        [SerializeField]
        private Loop _loop;

        private void Start()
        {
            var time = new Time();
            var board = new Core.OmokScene.Checkerboard(_checkerboard);
            var logger = new DebugLogger();

            var aiConsole = new AIConsole(logger);
            var aiPlayer = new AIPlayer(board, aiConsole);
            var mainPlayer = new MainPlayer(board);
            var players = new List<IPlayer>
            {
                mainPlayer,
                aiPlayer
            };

            var host = new OmokHost(board, players, new Core.Timer(time, _loop), 60);

            host.Start();
            host.GameFinishedEvent += (sender, args) =>
            {
                Debug.Log($"GameFinished. Win {args.WinnerStone}");
            };

            //var testboard = new List<List<int>>();
            //for (var i = 0; i < 19; i++)
            //{
            //    testboard.Add(new List<int>());
            //    for (var j = 0; j < 19; j++)
            //        testboard[i].Add(0);
            //}
            //var sample = Newtonsoft.Json.JsonConvert.SerializeObject(testboard.ToArray());
            //Debug.Log(sample);

            //Test();
        }

        //private static readonly string apiKey = "AIzaSyC92K2xdQh4r3tfJQPJJ4dOWCymDuPX8-o"; // 발급받은 API 키 입력
        //private static readonly string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

        //private async void Test()
        //{
        //    using var client = new HttpClient();

        //    // 1. 요청 데이터 구성 (JSON 구조 정의)
        //    var requestBody = new
        //    {
        //        contents = new[]
        //        {
        //            new { parts = new[] { new { text = "## Role\r\n당신은 오목(Gomoku) 전문가입니다. \r\n제공된 19x19 격자판 상태를 분석하여, 승리하기 위한 최적의 다음 수를 제안하세요.\r\n\r\n## Difficulty Level\r\n[난이도 입력: 초보자 / 중급자 / 고수]\r\n\r\n## Difficulty Guidelines\r\n1. 초보자: 상대방이 3목이나 4목을 만들면 방어하는 데 집중하며, 눈에 보이는 자신의 돌을 연결합니다.\r\n2. 중급자: '공격이 최선의 방어'임을 이해합니다. 33이나 43을 만들기 위한 빌드업을 시도하며, 상대의 노림수를 미리 차단합니다.\r\n3. 고수: 렌주룰(Renju Rule) 등의 금수를 고려하거나 유도하며, 수십 수 앞을 내다보는 수읽기를 통해 필승법(VCF, VCT)을 찾아냅니다.\r\n\r\n## Input Data Format\r\n- 19x19 2차원 배열 (0: 빈 공간, 1: 흑돌, 2: 백돌)\r\n- 현재 차례: [흑/백]\r\n\r\n## Board State\r\n[여기에 2차원 배열 데이터 붙여넣기]\r\n\r\n## Rule & Constraint\r\n- 반드시 5목을 먼저 완성하는 것이 목표입니다.\r\n- 상대방의 4목은 즉시 막아야 하며, 3목은 적절히 견제해야 합니다.\r\n- 응답은 반드시 지정된 JSON 형식으로만 하세요.\r\n\r\n## Response Format (JSON)\r\n{\r\n  \"thought\": \"현재 수의 전략적 이유 (방어, 공격 빌드업, 4-3 유도 등)\",\r\n  \"x\": \"추천하는 위치의 x 좌표 (0-18)\",\r\n  \"y\": \"추천하는 위치의 y 좌표 (0-18)\"\r\n}" } } }
        //        }
        //    };

        //    string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
        //    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        //    try
        //    {
        //        // 2. POST 요청 전송
        //        var response = await client.PostAsync(url, content);
        //        response.EnsureSuccessStatusCode();

        //        // 3. 응답 결과 출력
        //        string responseBody = await response.Content.ReadAsStringAsync();
        //        Debug.Log("--- Gemini 응답 ---");
        //        Debug.Log(responseBody);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.Log($"오류 발생: {ex.Message}");
        //    }
        //}
    }
}
