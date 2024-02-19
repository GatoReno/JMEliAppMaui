﻿using Controls.UserDialogs.Maui;
using JMEliAppMaui.ProgramHelpers;
using JMEliAppMaui.ViewModels;

namespace JMEliAppMaui;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage(MainPageViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		//UserDialogs.Instance.ShowToast("Hi frens 💀");
        UserDialogs.Instance.Loading("Hi frens 💀");
    }


}


