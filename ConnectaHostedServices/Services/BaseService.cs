using ConnectaHostedServices.Scheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NCrontab;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectaHostedServices.Services
{
    public abstract class BaseService : ScopedProcessor
    {
        private readonly bool _isActive;
        private readonly string _uri;

        private DateTime _nextRun;
        private readonly CrontabSchedule _schedule;

        private readonly ILogger<BaseService> _logger;
        private readonly IConfiguration _configuration;

        public BaseService(IServiceScopeFactory serviceScopeFactory, ILogger<BaseService> logger, IConfiguration configuration) :
            base(serviceScopeFactory)
        {
            this._logger = logger;
            this._configuration = configuration;
            string name = this.GetType().Name;

            dynamic conf = GetServiceConfiguration(name);

            _isActive = bool.Parse(conf.IsActive);
            _uri = conf.Uri;
            try
            {
                _schedule = CrontabSchedule.Parse(conf.Schedule);
                _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            }
            catch (Exception)
            {
                _schedule = null;
            }
        }

        public override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                do
                {
                    if (_schedule == null)
                        return;
                    var now = DateTime.Now;
                    if (now > _nextRun)
                    {
                        await Process();
                        _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                    }
                    await Task.Delay(5000, stoppingToken); //5 seconds delay
                }
                while (!stoppingToken.IsCancellationRequested);
            }
            catch (Exception)
            {


            }
        }

        private dynamic GetServiceConfiguration(string name)
        {

            dynamic obj = new ExpandoObject();
            obj.IsActive = _configuration[$"Services:{name}:IsActive"];
            obj.Uri = _configuration[$"Services:{name}:Uri"];
            obj.Schedule = _configuration[$"Services:{name}:Schedule"];
            return obj;
        }

        public bool ApiConnect(string methodType)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{this._uri}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Method = methodType;
            httpWebRequest.Timeout = System.Threading.Timeout.Infinite;
            httpWebRequest.KeepAlive = false;
            httpWebRequest.Proxy = null;
            httpWebRequest.Headers["AcceptLanguage"] = "en-GB,en-US;q=0.9,en;q=0.8,tr;q=0.7";
            httpWebRequest.Headers["Authorization"] = "eyJraWQiOiJkZWVLS0tURVQ3U3BEMlJIX2VULWJrOVZuRjRfY1EwMVg1azBGeHFza0pzIiwiYWxnIjoiUlMyNTYifQ.eyJ2ZXIiOjEsImp0aSI6IkFULi1nX200SlFVcnF5R3ZtQ1hhYXdXb0xROVUwYS1LdDlrandhSHNzYzVURjAiLCJpc3MiOiJodHRwczovL2FyY2VsaWsub2t0YXByZXZpZXcuY29tL29hdXRoMi9hdXNrZTNjbjlweWRFQlNnSjBoNyIsImF1ZCI6Imh0dHBzOi8vc3RhcnRkZXYuYXJjZWxpay5jb20iLCJpYXQiOjE2MzgyNTE3MjEsImV4cCI6MTYzODI4MDUyMSwiY2lkIjoiMG9hcWhyZTA5aDNqaU5UQjQwaDciLCJ1aWQiOiIwMHVwOXRuaDFuTktwTldxRjBoNyIsInNjcCI6WyJlbWFpbCIsIm9wZW5pZCIsInByb2ZpbGUiXSwic3ViIjoiQVItQVJDRUxJS1xcMjYwNDk3OTYiLCJsYXN0TmFtZSI6IkJ1bHV0IiwiZ2l2ZW5OYW1lIjoiSGF0aWNlIiwibWFuYWdlcklkIjoiQVIwMDIyODgiLCJlbWFpbCI6ImhhdGljZS5idWx1dEBhcmNlbGlrLmNvbSJ9.hYkb5lBXgmNXwdmJFAvY7xkUloQjamP0yab83RP0IElVCO3rI8foH9opC3ic9ZAZJAlylfqENVBZlC-XJqXGdOLRWOYvA_XL5y1bSfU490xFUQZTcNYr6U2jc09oYG522FEEM-ia2Sp3HFImvUje3G2LOALLL8jFYVyJ1Hdv-l7roUXoBqGaaBXzDpRNuTvEIeLeuOUJ_mzZu9nbXW6ivFh-pdxANYKu4_661nMDsgLQYHrq4IjzoQYaonu2oiqL-BfPIpL0BIXsbcDjUyw6lGg5Edw1e-hkr8UIWMES2y99XombYp4a5XA3FAjmLeu9DoN6X_AxubPqbZNJrazD6g";

            string data = "";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()!))
            {
                data =  streamReader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<bool>(data);
        }
    }
}
