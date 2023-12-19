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

        public List<string> ErrorCusts { get; set; } = new List<string>();

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
                var names = ErrorCusts.ToSeparatedString();
                if (ErrorCusts.Count > 1)
                {
                    return $"{Resources.AutoTransfer_MultipleErrorProccessing}({names})";
                }

                if (ErrorCusts.Count == 1)
                {
                    return $"{Resources.AutoTransfer_MultipleErrorProccessing_Single}({names})";
                }

                return string.Empty;
            }
        }

        public string SuccessMessage => Resources.AutoTransfer_UpdateSuccess;
    }
}
