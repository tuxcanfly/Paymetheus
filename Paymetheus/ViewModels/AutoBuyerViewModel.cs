// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Paymetheus.Decred.Wallet;
using Paymetheus.Framework;
using System.Windows.Input;

namespace Paymetheus.ViewModels
{
    public class AutoBuyerViewModel : ViewModelBase
    {
        public AutoBuyerViewModel(AutoBuyerProperties properties) : base()
        {
            _autoBuyerProperties = properties;

            StartAutoBuyerCommand = new DelegateCommand(StartAutoBuyerAction);
            StopAutoBuyerCommand = new DelegateCommand(StopAutoBuyerAction);
        }

        public AutoBuyerViewModel AutoBuyer { get; }

        private AutoBuyerProperties _autoBuyerProperties;

        public AutoBuyerProperties AutoBuyerProperties
        {
            get { return _autoBuyerProperties; }
            internal set { _autoBuyerProperties = value; RaisePropertyChanged(); }
        }

        public ICommand StartAutoBuyerCommand { get; }
        public DelegateCommand StopAutoBuyerCommand { get; private set; }

        private async void StartAutoBuyerAction()
        {
            await App.Current.Synchronizer.WalletRpcClient.StartAutoBuyer(AutoBuyerProperties);
        }

        private async void StopAutoBuyerAction()
        {
            await App.Current.Synchronizer.WalletRpcClient.StopAutoBuyer();
        }
    }
}