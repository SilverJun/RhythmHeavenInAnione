using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace RAS
{
    public class Parser
    {
        public Dictionary<string, System.Action<string[]>> _methods = new Dictionary<string, System.Action<string[]>>();

        public Dictionary<string, NoteSetting> _noteSettings = new Dictionary<string, NoteSetting>();
        public List<Pattern> _patterns = new List<Pattern>();
        
        public List<Command> _sheet;
        public List<Command> _initCommands = new List<Command>();
        private List<Command> _currentSheet;

        private string _script;

        // stage prefab.
        public BaseStage _baseStage;
        private float _currentBeat;

        public Parser(BaseStage baseStage, string script)
        {
            _script = script;
            _baseStage = baseStage;
            
            _methods.Add("NoteSetting", ParseNoteSetting);
            _methods.Add("SetPattern", ParseSetPattern);
            _methods.Add("Action", ParseAction);
            _methods.Add("SetStartDelay", ParseSetStartDelay);
            _methods.Add("SetBpm", ParseSetBpm);
            _methods.Add("SetStage", ParseSetStage);
            _methods.Add("Init", ParseInit);
            _methods.Add("Sheet", ParseSheet);
            _methods.Add("Beat", ParseBeat);
        }

        public void ParseSource()
        {
            var reader = new StringReader(_script);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                char[] split = { ' ', '\t', '(', ',', ')', '{', '}', '[', ']' };

                var token = line.Split(split);

                token = token.Where(x => x != "").ToArray();

                if (token.Length == 0)
                    continue;

                switch (token[0])
                {
                    case "":
                    case " ":
                    case "\n":
                    case "\t":
                    case "//":
                        continue;
                    default:
                        ParseLine(token);
                        break;
                }
            }
            reader.Close();
        }

        private void ParseLine(string[] token)
        {
            if (_methods.ContainsKey(token[0])) { _methods[token[0]](token); }
            else { Debug.Assert(false, "ParseLine Error!! unrecognized string " + token[0]); }
        }

        private void ParseNoteSetting(string[] token)
        {
            _noteSettings.Add(token[1], new NoteSetting(token[1], GetBeat(token[2]), token[3]));
        }

        private void ParseSetPattern(string[] token)
        {
            List<string> tokenList = new List<string>(token);
            List<NoteSetting> noteList = new List<NoteSetting>();
            tokenList.ForEach(x =>
            {
                if (_noteSettings.ContainsKey(x))
                {
                    noteList.Add(_noteSettings[x]);
                }
            });
            //NoteSetting[] noteList = _noteSettings.Where(x => tokenList.Contains(x.Key)).Select(x => x.Value).ToArray();
            _patterns.Add(new Pattern(token[1], noteList.ToArray()));
        }

        private void ParseAction(string[] token)
        {
            _currentSheet.Add(new Command(_currentBeat, token[0], token[1]));
        }

        private void ParseInit(string[] token)
        {
            _currentSheet = _initCommands;
        }

        private void ParseSheet(string[] token)
        {
            _sheet = new List<Command>();
            _currentSheet = _sheet;
        }

        private void ParseBeat(string[] token)
        {
            _currentBeat = float.Parse(token[1]);
        }

        private void ParseSetBpm(string[] token)
        {
            _currentSheet.Add(new Command(_currentBeat, token[0], stage => { stage.SetBpm(float.Parse(token[1])); }));
        }

        private void ParseSetStartDelay(string[] token)
        {
            _currentSheet.Add(new Command(_currentBeat, token[0], stage => { stage._startDelay = float.Parse(token[1]); }));
        }

        private void ParseSetStage(string[] token)
        {
            _currentSheet.Add(new Command(_currentBeat, token[0], stage =>
            {
                Debug.Log(token[1]);
                if (stage._stageObject != null)
                    GameObject.Destroy(stage._stageObject);

                stage._stageObject = GameObject.Instantiate(Resources.Load("Prefab/Stage/" + token[1]), Vector3.zero, Quaternion.identity) as GameObject;
                stage._stage = stage._stageObject.GetComponent("AbstractStage") as AbstractStage;
                stage._stage._baseStage = _baseStage;
            }));
        }

        public static float GetBeat(string str)
        {
            string beatString = str;

            // 기본 박자 => 4 / 비트 => 4분음표는 1, 8분음표는 0.5
            float beat = 4 / float.Parse(beatString[0].ToString());

            if (beatString.Length == 3)
            {
                // 셋잇단음표 구분
                // 4.3 => 4분음표의 셋잇단음표 한개. 3개가 나와야 함
                // 4분음표가 1박이면 0.6666박.
                if (beatString[0] == '4' && beatString[2] != '3')
                {
                    beat += 0.666666f;
                }
                // 점 구분
                // 4.5 => 점 4분음표.
                else if (beatString[2] != '5')
                {
                    beat += beat / 2.0f;
                }
            }

            return beat;
        }
    }
}
