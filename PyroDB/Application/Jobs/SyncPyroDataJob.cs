﻿using Mica;
using PyroDB.Application.Crawlers.PyroData;
using PyroDB.Data;

namespace PyroDB.Application.Jobs
{
    [JobConcurrency(false)]
    public class SyncPyroDataJob : IJob
    {
        private readonly PyroDataCrawler _crawler;

        public SyncPyroDataJob(PyroDataCrawler crawler)
        {
            _crawler = crawler;
        }

        public async Task Work(IProgress<double> progress, CancellationToken token = default)
        {
            await _crawler.SyncAll(progress, token);
        }
    }
}
