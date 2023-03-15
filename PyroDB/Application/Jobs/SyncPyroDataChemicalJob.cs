using Mica;
using PyroDB.Application.Synchronizers.PyroData;

namespace PyroDB.Application.Jobs
{
    [JobConcurrency(false)]
    public class SyncPyroDataChemicalJob : IJob
    {
        private readonly PyroDataChemicalSynchronizer _crawler;

        public SyncPyroDataChemicalJob(PyroDataChemicalSynchronizer crawler)
        {
            _crawler = crawler;
        }

        public async Task Work(IProgress<double> progress, CancellationToken token = default)
        {
            await _crawler.SyncAllAsync(progress, token);
        }
    }
}
