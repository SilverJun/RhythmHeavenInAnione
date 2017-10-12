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
    /// <summary>
    /// NoteSetting의 메소드의 정보값들을 저장할 구조체
    /// </summary>
    public struct NoteSetting
    {
        public string _name;
        public float _beat;
        public string _type;

        public NoteSetting(string name, float beat, string type)
        {
            _name = name;
            _beat = beat;
            _type = type;
        }
    }

    /// <summary>
    /// 스테이지의 한 패턴을 나타내는 구조체
    /// </summary>
    public struct Pattern
    {
        public string _name;
        public NoteSetting[] _noteSetting;
        public float _totalBeat;

        public Pattern(string name, NoteSetting[] notes)
        {
            _name = name;
            _noteSetting = notes;
            _totalBeat = notes.Select(x => x._beat).ToArray().Sum();
        }
    }
}
