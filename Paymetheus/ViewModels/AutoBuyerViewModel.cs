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

        private async Task<AutoBuyerProperties> GetAutoBuyerProperties()
        {
            AutoBuyerProperties autobuyerProperites = await App.Current.Synchronizer.WalletRpcClient.AutoBuyerPropertiesAsync();
            return autobuyerProperites;
        }

        private async void SetAccoun(uint account)
        {
            await App.Current.Synchronizer.WalletRpcClient.SetAccount(account);
        }

        private async void SetBalanceToMaintain(long balance)
        {
            await App.Current.Synchronizer.WalletRpcClient.SetBalanceToMaintain(balance);
        }

        private async void SetMaxFee(long maxFee)
        {
            await App.Current.Synchronizer.WalletRpcClient.SetMaxFee(maxFee);
        }

        private async void SetMaxPriceRelative(double maxPriceRelative)
        {
            await App.Current.Synchronizer.WalletRpcClient.SetMaxPriceRelative(maxPriceRelative);
        }

        private async void MaxPriceAbsolute(long maxPriceAbsolute)
        {
            await App.Current.Synchronizer.WalletRpcClient.SetMaxPriceAbsolute(maxPriceAbsolute);
        }

        private async void SetTicketAddress(string ticketAddress)
        {
            await App.Current.Synchronizer.WalletRpcClient.SetTicketAddress(ticketAddress);
        }

        private async void SetPoolAddress(string poolAddress)
        {
            await App.Current.Synchronizer.WalletRpcClient.SetPoolAddress(poolAddress);
        }

        private async void SetPoolFees(double poolFees)
        {
            await App.Current.Synchronizer.WalletRpcClient.SetPoolFees(poolFees);
        }

        private async void SetMaxPerBlock(long maxPerBlock)
        {
            await App.Current.Synchronizer.WalletRpcClient.SetMaxPerBlock(maxPerBlock);
        }
    }
}