using System.Collections.ObjectModel;
using Web_Maui.Models;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Web_Maui.ViewModels
{
    internal class NotesViewModel : IQueryAttributable
    {
        public ObservableCollection<NoteViewModel> AllNotes { get; }
        public ICommand NewCommand { get; }
        public ICommand SelectNoteCommand { get; }

        public NotesViewModel()
        {
            AllNotes = new ObservableCollection<NoteViewModel>(Note.LoadAll().Select(n => new NoteViewModel(n)));
            NewCommand = new AsyncRelayCommand(NewNoteAsync);
            SelectNoteCommand = new AsyncRelayCommand<NoteViewModel>(SelectNoteAsync!);
        }

        private async Task NewNoteAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.NotePage));
        }

        private async Task SelectNoteAsync(NoteViewModel note)
        {
            if (note != null)
                await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Identifier}");
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("deleted", out object? value))
            {
                string noteId = value.ToString() ?? "";
                NoteViewModel? matchedNote = AllNotes.FirstOrDefault((n) => n.Identifier == noteId);

                // If note exists, delete it
                if (matchedNote != null)
                    AllNotes.Remove(matchedNote);
            }
            else if (query.TryGetValue("saved", out object? value2))
            {
                string noteId = value2.ToString() ?? "";
                NoteViewModel? matchedNote = AllNotes.FirstOrDefault((n) => n.Identifier == noteId);

                // If note is found, update it
                if (matchedNote != null)
                {
                    matchedNote.Reload();
                    AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
                }

                // If note isn't found, it's new; add it.
                else
                    AllNotes.Insert(0, new NoteViewModel(Note.Load(noteId)));
            }
        }

    }
}
