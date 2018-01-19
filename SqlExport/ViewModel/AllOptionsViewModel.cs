namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using SqlExport.Common;
    using SqlExport.Messages;

    /// <summary>
    /// Defines the AllOptionsViewModel type.
    /// </summary>
    internal class AllOptionsViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="Options" /> property's name.
        /// </summary>
        public const string OptionsPropertyName = "Options";

        /// <summary>
        /// The <see cref="SelectedOption" /> property's name.
        /// </summary>
        public const string SelectedOptionPropertyName = "SelectedOption";

        /// <summary>
        /// The selected option
        /// </summary>
        private OptionViewModel selectedOption = null;

        /// <summary>
        /// The options
        /// </summary>
        private ObservableCollection<OptionViewModel> options = null;

        /// <summary>
        /// The open file command
        /// </summary>
        private RelayCommand openFileCommand;

        /// <summary>
        /// The ok command
        /// </summary>
        private RelayCommand okCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllOptionsViewModel"/> class.
        /// </summary>
        public AllOptionsViewModel()
        {
            this.LoadOptions();

            Messenger.Default.Register<OptionSelectedMessage>(this, this, m => this.SelectedOption = m.Option);
        }

        /// <summary>
        /// Gets the OpenFileCommand.
        /// </summary>
        public RelayCommand OpenFileCommand
        {
            get
            {
                return this.openFileCommand ?? (this.openFileCommand = new RelayCommand(
                    () =>
                        {
                            var p = new Process(); 
                            p.StartInfo = new ProcessStartInfo(Configuration.SettingsPath);
                            p.Start();

                            Messenger.Default.Send(new CloseWindow(), this);
                        },
                    () => File.Exists(Configuration.SettingsPath)));
            }
        }

        /// <summary>
        /// Gets the OkCommand.
        /// </summary>
        public RelayCommand OkCommand
        {
            get
            {
                return this.okCommand ?? (this.okCommand = new RelayCommand(() =>
                    {
                        this.SaveOptions();
                        Messenger.Default.Send(new CloseWindow(), this);
                    }));
            }
        }

        /// <summary>
        /// Gets or sets the Options property.
        /// </summary>
        public ObservableCollection<OptionViewModel> Options
        {
            get
            {
                return this.options;
            }

            set
            {
                if (this.options == value)
                {
                    return;
                }

                this.options = value;
                this.RaisePropertyChanged(OptionsPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the SelectedOption property.
        /// </summary>
        public OptionViewModel SelectedOption
        {
            get
            {
                return this.selectedOption;
            }

            set
            {
                if (this.selectedOption == value)
                {
                    return;
                }

                this.selectedOption = value;
                this.RaisePropertyChanged(SelectedOptionPropertyName);
            }
        }

        /// <summary>
        /// Loads the config.
        /// </summary>
        /// <returns>A new options loader.</returns>
        private static OptionsLoader LoadConfig()
        {
            var optionsLoader = new OptionsLoader();

            if (File.Exists(Configuration.SettingsPath))
            {
                var doc = new XmlDocument();
                using (var reader = File.OpenText(Configuration.SettingsPath))
                {
                    doc.Load(reader);
                }

                optionsLoader.LoadConfig(doc);
            }

            optionsLoader.LastLoaded = DateTime.Now;
            return optionsLoader;
        }

        /// <summary>
        /// Saves the options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionsLoader">The options loader.</param>
        private static void SaveOptions(IEnumerable<OptionViewModel> options, OptionsLoader optionsLoader)
        {
            foreach (var option in options)
            {
                optionsLoader.SetOptionValue(option.Path, option.Value);

                foreach (var property in option.Properties)
                {
                    optionsLoader.SetOptionValue(property.Path, property.Value);
                }

                SaveOptions(option.Children, optionsLoader);
            }
        }

        /// <summary>
        /// Loads the options.
        /// </summary>
        private void LoadOptions()
        {
            var optionsLoader = LoadConfig();

            var optionViewModels = from o in optionsLoader.AllOptions
                                   orderby o.DisplayName
                                   select (OptionViewModel)o;
            this.Options = new ObservableCollection<OptionViewModel>(optionViewModels);

            this.SelectedOption = this.Options.FirstOrDefault();
            if (this.SelectedOption != null)
            {
                this.SelectedOption.IsSelected = true;
            }
        }

        /// <summary>
        /// Saves the options.
        /// </summary>
        private void SaveOptions()
        {
            var optionsLoader = LoadConfig();

            SaveOptions(this.Options, optionsLoader);

            var doc = new XmlDocument();
            optionsLoader.SaveConfig(doc);

            using (var writer = File.CreateText(Configuration.SettingsPath))
            {
                doc.Save(writer);
            }

            Messenger.Default.Send(new OptionsChangedMessage());
        }
    }
}
