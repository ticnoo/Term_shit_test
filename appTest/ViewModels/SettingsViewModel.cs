using System;
using System.Collections.Generic;
using ReactiveUI;

namespace appTest.ViewModels
{
	public class SettingsViewModel : ViewModelBase
    {
		private string testString = "kekich";
        public string TestString
        {
            get => testString;
            set => this.RaiseAndSetIfChanged(ref testString, value);
        }

        public void AddItem(object msg)
        {
            TestString = "setted";
        }
    }
}