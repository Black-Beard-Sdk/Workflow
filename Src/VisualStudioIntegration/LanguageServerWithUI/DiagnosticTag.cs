using System.ComponentModel;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace LanguageServerWithUI
{
    public class DiagnosticTag : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public DiagnosticSeverity Severity
        {
            get { return this.severity; }
            set
            {
                this.severity = value;
                OnPropertyChanged(nameof(Severity));
            }
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private string text;
        private DiagnosticSeverity severity;

    }
}
