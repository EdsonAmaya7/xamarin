using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        DetailPageViewModel detailViewModel = new DetailPageViewModel();
        NoteModel auxNote;
        public MainPageViewModel()
        {
            Notes = new ObservableCollection<NoteModel>();

            SaveNoteCommand = new Command(() =>
            {
                Notes.Add(new NoteModel { Text = NoteText });
                NoteText = string.Empty;
            },
            () => !string.IsNullOrEmpty(NoteText));

            EraseNotesCommand = new Command(() => Notes.Clear());

            NoteSelectedCommand = new Command(async () =>
            {
                if (SelectedNote is null)
                    return;

                detailViewModel.NoteText =  SelectedNote.Text;
                {
                    auxNote = SelectedNote;
                };

                await Application.Current.MainPage.Navigation.PushAsync(new DetailPage(detailViewModel));

                SelectedNote = null;
            });

            detailViewModel.EditCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();

                int posicion = Notes.IndexOf(auxNote);
                if (posicion >= 0)
                    Notes[posicion] = new NoteModel { Text = detailViewModel.NoteText };
                string what = Notes[posicion].Text;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        string noteText;
        public string NoteText
        {
            get => noteText;
            set
            {
                noteText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoteText)));

                SaveNoteCommand.ChangeCanExecute();
            }
        }

        NoteModel selectedNote;
        public NoteModel SelectedNote
        {
            get => selectedNote;
            set
            {
                selectedNote = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNote)));
            }
        }

        public ObservableCollection<NoteModel> Notes { get; }

        public Command NoteSelectedCommand { get; }
        public Command SaveNoteCommand { get; }
        public Command EraseNotesCommand { get; }
    }
}
