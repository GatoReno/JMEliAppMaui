using System;
using System.Collections.ObjectModel;
using JMEliAppMaui.Models;
using JMEliAppMaui.ProgramHelpers;

namespace JMEliAppMaui.Services.Abstractions
{
	public interface IDevNotesService
	{
	    Task<string> AddNoteDev(Note4DevModel note);
        Task<ObservableCollection<Note4DevModel>> GetNotes();
		Task DeleteNoteDev(string id);
		
    }
}

