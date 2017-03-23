// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Paymetheus.Decred.Wallet;
using Paymetheus.Framework;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Paymetheus.ViewModels
{
    public class AutoBuyerViewModel : ViewModelBase
    {
        public AutoBuyerViewModel(AutoBuyerProperties properties) : base()
        {
            _autoBuyerProperties = properties;

            StartAutoBuyerCommand = new DelegateCommandAsync(StartAutoBuyerAction);
            StopAutoBuyerCommand = new DelegateCommandAsync(StopAutoBuyerAction);
        }

        public AutoBuyerViewModel AutoBuyer { get; }

        private AutoBuyerProperties _autoBuyerProperties;

        public AutoBuyerProperties AutoBuyerProperties
        {
            get { return _autoBuyerProperties; }
            internal set { _autoBuyerProperties = value; RaisePropertyChanged(); }
        }

        public DelegateCommandAsync StartAutoBuyerCommand { get; }
        public DelegateCommandAsync StopAutoBuyerCommand { get; }

        private Task StartAutoBuyerAction() => App.Current.Synchronizer.WalletRpcClient.StartAutoBuyer(AutoBuyerProperties);

        private Task StopAutoBuyerAction() => App.Current.Synchronizer.WalletRpcClient.StopAutoBuyer();

        private Task<AutoBuyerProperties> GetAutoBuyerProperties() => App.Current.Synchronizer.WalletRpcClient.AutoBuyerPropertiesAsync();
    }
}