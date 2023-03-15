using Mica;
using PyroDB.Application.Jobs.PyroData.Synchronizers;

namespace PyroDB.Application.Jobs.PyroData
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
