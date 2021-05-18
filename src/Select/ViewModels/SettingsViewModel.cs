using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Win32;
using Select.Helpers;
using Select.Models;

namespace Select.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		private readonly string _appBaseDir = Process.GetCurrentProcess().MainModule?.FileName;
		private bool _startWithWindows;
		private SettingsManager<UserSettings> _settingsManager;
		private UserSettings _userSettings = new();
		
		public SettingsViewModel()
		{
			StartWindowsCheckedChangedCommand = new RelayCommand(OnStartWindowsChanged);
			Initialize();
		}

		public bool StartWithWindows
		{
			get => _startWithWindows;
			set => SetProperty(ref _startWithWindows, value);
		}

		public RelayCommand StartWindowsCheckedChangedCommand { get;  }

		private void Initialize()
		{
			_settingsManager = new SettingsManager<UserSettings>("select_settings.json");
			_userSettings = _settingsManager.LoadSettings();
			_userSettings ??= new UserSettings();
			
			StartWithWindows = _userSettings.StartWithWindows;
		}
		
		private void OnStartWindowsChanged(object obj)
		{
			if (StartWithWindows)
			{
				_userSettings.StartWithWindows = true;
				var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				key?.SetValue(nameof(Select), _appBaseDir);
			}
			else
			{
				_userSettings.StartWithWindows = false;
				var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				key?.DeleteValue(nameof(Select), false);
			}
			
			_settingsManager.SaveSettings(_userSettings);
		}
	}
}
