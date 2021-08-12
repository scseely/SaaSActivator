using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Azure.Identity;
using Microsoft.Marketplace.SaaS.Models;

namespace SaaSActivator_Gui
{
    public class FetchSubscriptionsCommand: ICommand
    {
        private readonly ActivatorData _data;
        private bool _executing = false;

        public FetchSubscriptionsCommand(ActivatorData data)
        {
            _data = data;
        }
        public bool CanExecute(object? parameter)
        {
            return !_executing;
        }

        public async Task ExecuteAsync()
        {
            _executing = true;
            var dispatcher = Dispatcher.CurrentDispatcher;
            try
            {
                var cred = new ClientSecretCredential(_data.TenantId, _data.ClientId, _data.ClientSecret);
                var client = new Microsoft.Marketplace.SaaS.MarketplaceSaaSClient(cred);
                var enumerator = client.Fulfillment.ListSubscriptionsAsync().GetAsyncEnumerator();
                var subs = new List<SubscriptionData>();
                for (; await enumerator.MoveNextAsync(); )
                {
                    var sub = enumerator.Current;
                    if (sub.SaasSubscriptionStatus != SubscriptionStatusEnum.Unsubscribed)
                    {
                        var subData = new SubscriptionData()
                        {
                            Subscription = sub,
                            ActivationData = _data
                        };
                        subs.Add(subData);
                    }
                }

                dispatcher.Invoke(() => _data.Subscriptions = subs);
            }
            finally
            {
                _executing = false;
            }
        }

        public void Execute(object? parameter)
        {
            ExecuteAsync();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
