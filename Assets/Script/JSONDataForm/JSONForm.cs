using UnityEngine;

namespace JSONForm
{
    [SerializeField]
    public struct StageInfo
    {
        public string StageName;
        public string StageTitle;
        public string StageStatus;
        public string StageIntro;

        public string GetStageStatusString()
        {
            string result = "";

            switch (StageStatus)
            {
                case "Lock":
                    result = "���";
                    break;
                case "UnLock":
                    result = "�������";
                    break;
                case "Clear":
                    result = "Ŭ����";
                    break;
            }

            return result;
        }
    }

}
