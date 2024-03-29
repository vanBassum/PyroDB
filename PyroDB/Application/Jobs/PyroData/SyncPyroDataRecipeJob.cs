﻿using Mica;
using PyroDB.Application.Jobs.PyroData.Synchronizers;
using PyroDB.Data;

namespace PyroDB.Application.Jobs.PyroData
{
    [JobConcurrency(false)]
    public class SyncPyroDataRecipeJob : IJob
    {
        private readonly PyroDataRecipeSynchronizer _crawler;

        public SyncPyroDataRecipeJob(PyroDataRecipeSynchronizer crawler)
        {
            _crawler = crawler;
        }

        public async Task Work(IProgress<double> progress, CancellationToken token = default)
        {
            await _crawler.SyncAllAsync(progress, token);
        }
    }
}
