﻿// Copyright (c) 2016 The btcsuite developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using Paymetheus.Bitcoin.Util;
using Paymetheus.Bitcoin.Wallet;
using Paymetheus.Rpc;
using System;
using System.IO;
using System.Windows;

namespace Paymetheus
{
    class StartupWizard : WizardViewModel
    {
        public StartupWizard(ShellViewModel shell) : base(shell)
        {
            CurrentDialog = new BtcdRpcConnectionDialog(this);
            Shell = shell;
        }

        public ShellViewModel Shell { get; }
        public byte[] WalletSeed { get; set; }

        public event EventHandler WalletOpened;

        public void InvokeWalletOpened()
        {
            App.Current.MarkWalletLoaded();
            WalletOpened?.Invoke(this, new EventArgs());
        }
    }

    class ConnectionWizardDialog : WizardDialogViewModelBase
    {
        public ConnectionWizardDialog(StartupWizard wizard) : base(wizard.Shell, wizard)
        {
            Wizard = wizard;
        }

        public StartupWizard Wizard { get; }
    }

    class BtcdRpcConnectionDialog : ConnectionWizardDialog
    {
        public BtcdRpcConnectionDialog(StartupWizard wizard) : base(wizard)
        {
            ConnectCommand = new DelegateCommand(Connect);

            // Do not autofill local defaults if they don't exist.
            if (!File.Exists(BtcdCertificateFile))
            {
                BtcdNetworkAddress = "";
                BtcdCertificateFile = "";
            }
        }

        public string BtcdNetworkAddress { get; set; } = "localhost";
        public string BtcdRpcUsername { get; set; } = "";
        public string BtcdRpcPassword { private get; set; } = "";
        public string BtcdCertificateFile { get; set; } = BtcdRpcOptions.LocalCertifiateFilePath;

        public DelegateCommand ConnectCommand { get; }
        private async void Connect()
        {
            try
            {
                ConnectCommand.Executable = false;

                if (string.IsNullOrWhiteSpace(BtcdNetworkAddress))
                {
                    MessageBox.Show("Network address is required");
                    return;
                }
                if (string.IsNullOrWhiteSpace(BtcdRpcUsername))
                {
                    MessageBox.Show("RPC username is required");
                    return;
                }
                if (BtcdRpcPassword.Length == 0)
                {
                    MessageBox.Show("RPC password may not be empty");
                    return;
                }
                if (!File.Exists(BtcdCertificateFile))
                {
                    MessageBox.Show("Certificate file not found");
                    return;
                }

                var btcdOptions = new BtcdRpcOptions(BtcdNetworkAddress, BtcdRpcUsername, BtcdRpcPassword, BtcdCertificateFile);
                try
                {
                    await App.Current.WalletRpcClient.StartBtcdRpc(btcdOptions);
                }
                catch (Exception ex) when (ErrorHandling.IsTransient(ex) || ErrorHandling.IsClientError(ex))
                {
                    MessageBox.Show("Unable to start btcd RPC.\n\nCheck connection settings and try again.", "Error");
                    MessageBox.Show(ex.Message);
                    return;
                }

                var walletExists = await App.Current.WalletRpcClient.WalletExistsAsync();
                if (!walletExists)
                {
                    _wizard.CurrentDialog = new DisplaySeedDialog(Wizard);
                }
                else
                {
                    // TODO: Determine whether the public encryption is enabled and a prompt for the
                    // public passphrase prompt is needed before the wallet can be opened.  If it
                    // does not, then the wallet can be opened directly here instead of creating
                    // another dialog.
                    _wizard.CurrentDialog = new PromptPublicPassphraseDialog(Wizard);

                    //await _walletClient.OpenWallet("public");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                ConnectCommand.Executable = true;
            }
        }
    }

    class DisplaySeedDialog : ConnectionWizardDialog
    {
        public DisplaySeedDialog(StartupWizard wizard) : base(wizard)
        {
            AdvanceToConfirmationCommand = new DelegateCommand(AdvanceToConfirmation);

            if (wizard.WalletSeed == null)
            {
                wizard.WalletSeed = Wallet.RandomSeed();
            }
        }

        // TODO: Convert seed using a wordlist instead of encoding as hex so this is easier
        // for the user to write down and type into the next dialog.
        public string Bip0032SeedHex => Hexadecimal.Encode(Wizard.WalletSeed);

        public DelegateCommand AdvanceToConfirmationCommand { get; }
        private void AdvanceToConfirmation()
        {
            Wizard.CurrentDialog = new ConfirmSeedBackupDialog(Wizard);
        }
    }

    class ConfirmSeedBackupDialog : ConnectionWizardDialog
    {
        public ConfirmSeedBackupDialog(StartupWizard wizard) : base(wizard)
        {
            if (wizard.WalletSeed == null)
            {
                throw new Exception("Verification dialog requires seed");
            }

            ConfirmSeedCommand = new DelegateCommand(ConfirmSeed);
            BackCommand = new DelegateCommand(Back);
        }

        public string Input { get; set; } = "";

        public DelegateCommand ConfirmSeedCommand { get; }
        private void ConfirmSeed()
        {
            byte[] decodedSeed;
            if (Hexadecimal.TryDecode(Input, out decodedSeed))
            {
                if (ValueArray.ShallowEquals(Wizard.WalletSeed, decodedSeed))
                {
                    _wizard.CurrentDialog = new PromptPassphrasesDialog(Wizard);
                    return;
                }
            }

            MessageBox.Show("Seed does not match");
        }

        public DelegateCommand BackCommand { get; }
        private void Back()
        {
            Wizard.CurrentDialog = new DisplaySeedDialog(Wizard);
        }
    }

    class PromptPassphrasesDialog : ConnectionWizardDialog
    {
        public PromptPassphrasesDialog(StartupWizard wizard) : base(wizard)
        {
            CreateWalletCommand = new DelegateCommand(CreateWallet);
        }

        private bool _usePublicEncryption = false;

        public bool UsePublicEncryption
        {
            get { return _usePublicEncryption; }
            set { _usePublicEncryption = value; RaisePropertyChanged(); }
        }
        public string PublicPassphrase { private get; set; } = "";
        public string PrivatePassphrase { private get; set; } = "";

        public DelegateCommand CreateWalletCommand { get; }
        private async void CreateWallet()
        {
            try
            {
                CreateWalletCommand.Executable = false;

                if (string.IsNullOrEmpty(PrivatePassphrase))
                {
                    MessageBox.Show("Private passphrase is required");
                    return;
                }

                var publicPassphrase = PublicPassphrase;
                if (!UsePublicEncryption)
                {
                    publicPassphrase = "public";
                }
                else
                {
                    if (string.IsNullOrEmpty(publicPassphrase))
                    {
                        MessageBox.Show("Public passphrase is required");
                        return;
                    }
                }

                await App.Current.WalletRpcClient.CreateWallet(publicPassphrase, PrivatePassphrase, Wizard.WalletSeed);

                ValueArray.Zero(Wizard.WalletSeed);
                Wizard.InvokeWalletOpened();
            }
            finally
            {
                CreateWalletCommand.Executable = true;
            }
        }
    }

    class PromptPublicPassphraseDialog : ConnectionWizardDialog
    {
        public PromptPublicPassphraseDialog(StartupWizard wizard) : base(wizard)
        {
            OpenWalletCommand = new DelegateCommand(OpenWallet);
        }

        public string PublicPassphrase { get; set; } = "";

        public DelegateCommand OpenWalletCommand { get; }
        private async void OpenWallet()
        {
            try
            {
                OpenWalletCommand.Executable = false;
                await App.Current.WalletRpcClient.OpenWallet(PublicPassphrase);
                Wizard.InvokeWalletOpened();
            }
            catch (Exception ex) when (ErrorHandling.IsClientError(ex))
            {
                MessageBox.Show("Public data decryption was unsuccessful");
                MessageBox.Show(ex.ToString());
                return;
            }
            finally
            {
                OpenWalletCommand.Executable = true;
            }
        }
    }
}
