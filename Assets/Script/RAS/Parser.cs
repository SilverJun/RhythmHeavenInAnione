using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace RAS
{
    public class Parser
    {
        public Dictionary<string, System.Action<string[]>> _methods = new Dictionary<string, System.Action<string[]>>();
        public List<Pattern> _patterns = new List<Pattern>();
        public Dictionary<int, Pattern> _actions = new Dictionary<int, Pattern>();
        public Dictionary<string, NoteSetting> _noteSettings = new Dictionary<string, NoteSetting>();

        private TextAsset _script;

        public Parser(TextAsset script)
        {
            _script = script;

            _methods.Add("NoteSetting", ParseNoteSetting);
            _methods.Add("Pattern", ParsePattern);
            _methods.Add("Action", ParseAction);
        }

        public void ParseSource()
        {
            var reader = new StringReader(_script.text);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                char[] split = { ' ', '(', ',', ')' };

                var token = line.Split(split);

                switch (token[0])
                {
                    case "":
                    case " ":
                    case "\n":
                    case "\t":
                    case "//":
                        continue;
                    default:
                        token = token.Where(x => x != "").ToArray();
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
            _noteSettings.Add(token[1], new NoteSetting(token[1], AbstractStage.GetBeat(token[2]), token[3]));
        }

        private void ParsePattern(string[] token)
        {
            List<string> tokenList = new List<string>(token);
            NoteSetting[] noteList = _noteSettings.Where(x => tokenList.Contains(x.Key)).Select(x => x.Value).ToArray();
            _patterns.Add(new Pattern(token[1], noteList));
        }

        private void ParseAction(string[] token)
        {
            _actions.Add(int.Parse(token[1]), _patterns.Find(x => x._name == token[2]));
        }
    }
}
