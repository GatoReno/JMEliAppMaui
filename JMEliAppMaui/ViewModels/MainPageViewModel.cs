using System;
using JMEliAppMaui.ProgramHelpers;

namespace JMEliAppMaui.ViewModels
{
	public class MainPageViewModel : BindableObject
	{
		private string  _title;
		public string Title { get =>  _title; set { _title = value;OnPropertyChanged(); } }

		public MainPageViewModel()
		{
			Title = "HOLA";
		}
	}
}

