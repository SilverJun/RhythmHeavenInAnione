using System.Collections;
using System.Linq;
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

    public class Note
    {
        public NoteType _type;
        public bool _isHit;
        public bool _isSucceed;
        public float _genTime;
        public string _noteName;
        public float _beat;

        public Note(float genTime, string type, string noteName, float beat)
        {
            _isHit = false;
            _isSucceed = false;
            _genTime = genTime;
            _beat = beat;
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
                case "None":
                    _type = NoteType.None;
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

    /// <summary>
    /// Sheet에서 특정 beat에 맞춰 수행할 동작의 추상 부모 클래스.
    /// </summary>
    public class Command
    {
        public float _beat;
        public string _commandName;
        public string _patternName;
        public System.Action<BaseStage> _action;

        public Command(
            float beat,
            string commandName,
            System.Action<BaseStage> action)
        {
            _beat = beat;
            _commandName = commandName;
            _action = action;
        }

        public Command(
            float beat,
            string commandName,
            string patternName)
        {
            _beat = beat;
            _commandName = commandName;
            _patternName = patternName;
        }
    }
}
