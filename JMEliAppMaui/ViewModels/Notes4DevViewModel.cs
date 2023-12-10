using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JMEliAppMaui.Models;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.ViewModels
{
	public class Notes4DevViewModel : BindableObject
	{
        #region Props
        public ICommand SendNote4DevCommand { get; private set; }

        public ICommand GetNote4DevCommand { get; private set; }
        private IDevNotesService _fibAddChild;

        private Note4DevModel _note;
        public Note4DevModel Note { get => _note; set { _note = value; OnPropertyChanged(); } }

        private string _message;
        public string Message { get => _message; set { _message = value; OnPropertyChanged(); } }


        private string _email;
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }

        
        public ObservableCollection<Note4DevModel> DevNotes { get; set; }
        #endregion 

        public Notes4DevViewModel(IDevNotesService fibAddChild)
		{
            this._fibAddChild = fibAddChild;
            DevNotes = new ObservableCollection<Note4DevModel>();
            SendNote4DevCommand = new Command(OnSendNoteCommand);
            GetNote4DevCommand = new Command(OnGetNote4DevCommand);
            GetNote4DevCommand.Execute(null);
         }

        private async void OnGetNote4DevCommand()
        {
            DevNotes.Clear();
            if (_fibAddChild != null)
            {
                var notes = await _fibAddChild.GetNotes();
                foreach (var n in notes)
                {
                    DevNotes.Add(n);
                }
            }
        }

        private async void OnSendNoteCommand()
        {
            if (!string.IsNullOrEmpty(Message))
            {
                Note = new Note4DevModel();
                Note.Message = Message;
                Note.Email = Email;
                Note.Date = DateTime.Now;
                await _fibAddChild.AddNoteDev(Note);
                DevNotes.Add(Note);
            }
             
        }
    }
}

