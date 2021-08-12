using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Azure.Identity;
using Microsoft.Marketplace.SaaS.Models;
using SaaSActivator_Gui.Annotations;

namespace SaaSActivator_Gui
{
    public class SubscriptionData : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Subscription _subscription;
        public Subscription Subscription
        {
            get => _subscription;

            set
            {
                _subscription = value;
                OnPropertyChanged();
            }
        }

        public ICommand Activate
        {
            get { return new ActivateCommand(this); }
        }

        public ActivatorData ActivationData { get; set; }

        class ActivateCommand : ICommand
        {
            private readonly SubscriptionData _subscriptionData;

            public ActivateCommand(SubscriptionData subscriptionData)
            {
                _subscriptionData = subscriptionData;
            }

            public bool CanExecute(object? parameter)
            {
                return _subscriptionData.Subscription.SaasSubscriptionStatus == SubscriptionStatusEnum.PendingFulfillmentStart;
            }

            public void Execute(object? parameter)
            {
                ExecuteAsync();
            }

            public event EventHandler? CanExecuteChanged;

            public async Task ExecuteAsync()
            {
                var dispatcher = Dispatcher.CurrentDispatcher;
                var cred = new ClientSecretCredential(_subscriptionData.ActivationData.TenantId, 
                    _subscriptionData.ActivationData.ClientId, 
                    _subscriptionData.ActivationData.ClientSecret);
                var client = new Microsoft.Marketplace.SaaS.MarketplaceSaaSClient(cred);
                var subscriberPlan = new SubscriberPlan()
                {
                    PlanId = _subscriptionData.Subscription.PlanId,
                    Quantity = _subscriptionData.Subscription.Quantity
                };
                await client.Fulfillment.ActivateSubscriptionAsync(_subscriptionData.Subscription.Id.GetValueOrDefault(), subscriberPlan);
                dispatcher.Invoke(() => _subscriptionData.ActivationData.FetchSubscriptions.Execute(null));
            }
        }
    }
}