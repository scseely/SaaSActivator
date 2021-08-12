using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SaaSActivator_Gui.Annotations;

namespace SaaSActivator_Gui
{
    public class ActivatorData : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
      
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _tenantId;
        public string TenantId
        {
            get => _tenantId;

            set
            {
                _tenantId = value;
                Properties.Settings.Default["tenantId"] = _tenantId;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private string _clientId;
        public string ClientId
        {
            get => _clientId;

            set
            {
                _clientId = value;
                Properties.Settings.Default["clientId"] = _clientId;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private string _clientSecret;
        public string ClientSecret
        {
            get => _clientSecret;

            set
            {
                _clientSecret = value;
                Properties.Settings.Default["clientSecret"] = _clientSecret;
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        private List<SubscriptionData> _subscriptions;
        public List<SubscriptionData> Subscriptions
        {
            get => _subscriptions;

            set
            {
                _subscriptions = value;
                OnPropertyChanged();
            }
        }

        public FetchSubscriptionsCommand FetchSubscriptions => new FetchSubscriptionsCommand(this);
    }
}
