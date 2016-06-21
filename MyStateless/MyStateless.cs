using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using MyStateless.Interfaces;

namespace MyStateless
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class MyStateless : StatelessService, IMyStateless
    {
        public MyStateless(StatelessServiceContext context)
            : base(context)
        { }

        public Task<DateTimeOffset> Now()
        {
            ServiceEventSource.Current.ServiceMessage(this, "Now - " + Context.PartitionId + " - " + Context.InstanceId);
            return Task.FromResult(DateTimeOffset.UtcNow);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            ServiceEventSource.Current.ServiceMessage(this, "CreateServiceInstanceListeners");

            return new[] { new ServiceInstanceListener(this.CreateServiceRemotingListener) };
        }

        protected override Task OnOpenAsync(CancellationToken cancellationToken)
        {
            ServiceEventSource.Current.ServiceMessage(this, "OnOpenAsync");

            return Task.FromResult(0);
        }

        protected override Task OnCloseAsync(CancellationToken cancellationToken)
        {
            ServiceEventSource.Current.ServiceMessage(this, "OnCloseAsync");

            return Task.FromResult(0);
        }

        protected override void OnAbort()
        {
            ServiceEventSource.Current.ServiceMessage(this, "OnAbort");
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var configurationPackage = Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            var frequencyInSeconds = int.Parse(configurationPackage.Settings.Sections["MyConfigSection"].Parameters["MyFrequency"].Value);
            long iterations = 0;
            while (!cancellationToken.IsCancellationRequested)
            {
                ServiceEventSource.Current.ServiceMessage(this, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(frequencyInSeconds), cancellationToken).ContinueWith(_ => { }, TaskContinuationOptions.NotOnFaulted);
            }
        }
    }
}
