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

    public enum NoteType
    {
        None,
        Notice,
        Touch,
        Swipe
    }

    public struct Note
    {
        public NoteType _type;
        public bool _isHit;
        public float _genTime;
        public string _noteName;

        public Note(float genTime, string type, string noteName)
        {
            _isHit = false;
            _genTime = genTime;
            _type = NoteType.None;
            _noteName = noteName;
            switch (type)
            {
                case "Notice":
                    _type = NoteType.Notice;
                    break;
                case "Touch":
                    _type = NoteType.Touch;
                    break;
                case "Swipe":
                    _type = NoteType.Swipe;
                    break;
                default:
                    Debug.LogErrorFormat("Undefined Note Type!! {0}", type);
                    break;
            }
        }

        public static bool operator == (Note lhs, Note rhs)
        {
            return lhs._type == rhs._type && lhs._genTime.ToString() == rhs._genTime.ToString() && lhs._noteName == rhs._noteName;
        }

        public static bool operator != (Note lhs, Note rhs)
        {
            return !(lhs == rhs);
        }
    }
}
