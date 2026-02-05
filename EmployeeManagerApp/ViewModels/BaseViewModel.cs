using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EmployeeManagerApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Dictionary<string, List<string>> _errors = new();
        public bool HasErrors => _errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName != null && _errors.ContainsKey(propertyName))
            {
                return _errors[propertyName];
            }

            return Enumerable.Empty<string>();
        }

        protected void AddError(string property, string error)
        {
            if (!_errors.ContainsKey(property))
            {
                _errors[property] = new List<string>();
            }

            _errors[property].Add(error);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));
        }

        protected void ClearErrors(string property)
        {
            if (_errors.Remove(property)) 
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));
            }
        }
    }
}
