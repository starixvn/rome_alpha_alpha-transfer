using System.Threading;
using Fanex.RemotingRef;

namespace Alpha.Transfer.Services
{
    public interface CacheRefService
    {
        string GetProgramName(int resourceId);
    }

    public class CacheRefServiceImpl : CacheRefService
    {
        private readonly IRemotingRefService remotingRefService;

        public CacheRefServiceImpl(IRemotingRefService remotingRefService)
            => this.remotingRefService = remotingRefService;

        public string GetProgramName(int resourceId)
            => GetResourceDataByRefIdAndResourceId("RewardProgram", resourceId);

        private string GetResourceDataByRefIdAndResourceId(string refId, int resourceId)
            => remotingRefService.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId.ToString(),
                LanguageCode,
                string.Empty);

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}
