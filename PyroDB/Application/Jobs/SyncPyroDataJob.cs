using Mica;
using PyroDB.Application.Synchronizers.PyroData;
using PyroDB.Data;

namespace PyroDB.Application.Jobs
{
    [JobConcurrency(false)]
    public class SyncPyroDataJob : IJob
    {
        private readonly PyroDataSynchronizer _crawler;

        public SyncPyroDataJob(PyroDataSynchronizer crawler)
        {
            _crawler = crawler;
        }

        public async Task Work(IProgress<double> progress, CancellationToken token = default)
        {
            await _crawler.SyncAll(progress, token);
        }
    }
}
