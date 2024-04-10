using System.Collections.Generic;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Extensions;

namespace Alpha.Transfer.Models.AutoTransfer
{
    public class UpdateMultipleAutoTransferResultModel
    {
        public UpdateMultipleAutoTransferResultModel() { }

        public UpdateMultipleAutoTransferResultModel(int code, string stopMessage)
        {
            Code = code;
            StopMessage = stopMessage;
        }

        public int? Code { get; set; }

        public List<string> WarningCusts { get; set; } = new List<string>();

        public string StopMessage { get; set; }

        public string WarningMessage
        {
            get
            {
                var names = WarningCusts.ToSeparatedString();
                if (WarningCusts.Count > 1)
                {
                    return $"{Resources.AutoTransfer_MultipleAffectNextConditionWarning}({names})";
                }

                if (WarningCusts.Count == 1)
                {
                    return $"{Resources.AutoTransfer_MultipleAffectNextConditionWarning_Single}({names})";
                }

                return string.Empty;
            }
        }

        public string ErrorMessage
        {
            get
            {
                var result = new List<string>();

                foreach (var err in ErrMessages)
                {
                    var custs = ErrCusts[err.Key];
                    var suffix = custs.Count > 1 ? "Multiple" : "Single";
                    var message = Resources.ResourceManager.GetString($"AutoTransfer_Error_{err.Key}_{suffix}");
                    message = string.IsNullOrWhiteSpace(message) ? err.Value : message;

                    var names = custs.ToSeparatedString();
                    message += $"({names})";
                    result.Add(message);
                }

                return string.Join("<br />", result);
            }
        }

        public string SuccessMessage => Resources.AutoTransfer_UpdateSuccess;

        public Dictionary<string, string> ErrMessages { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, List<string>> ErrCusts { get; set; } = new Dictionary<string, List<string>>();
    }
}
