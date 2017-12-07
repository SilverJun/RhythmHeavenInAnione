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
                    result = "잠김";
                    break;
                case "UnLock":
                    result = "잠금해제";
                    break;
                case "Clear":
                    result = "클리어";
                    break;
            }

            return result;
        }
    }

}
