using EmployeeManagerApp.Models;
using System.Text.RegularExpressions;
using System.Windows.Input;
using EmployeeManagerApp.Commands;

namespace EmployeeManagerApp.ViewModels
{
    internal class MainWindowViewModel: BaseViewModel
    {
        private bool _isAuthenticated = false;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private bool _hasAllAuthErrors = false;
        private User _userModel;
        private Employee _employeeModel;
        public string Login
        {
            get => _login;
            set 
            {
                _login = value;
                ValidateLogin();
                ValidatePassword();
                HasAllAuthErrors = !HasErrors;
                OnPropertyChanged();
            }
        }

        private void ValidateLogin()
        {
            ClearErrors(nameof(Login));

            if (string.IsNullOrWhiteSpace(Login))
            {
                AddError(nameof(Login), "Логин обязателен");
            }
            else if (!Regex.IsMatch(Login, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                AddError(nameof(Login), "Некорректный email");
            }
        }

        public string Password
        {
            get => _password;
            set 
            {
                _password = value;
                ValidatePassword();
                HasAllAuthErrors = !HasErrors;
                OnPropertyChanged();
            }
        }

        private void ValidatePassword()
        {
            ClearErrors(nameof(Password));

            if (string.IsNullOrWhiteSpace(Password))
            {
                AddError(nameof(Password), "Пароль обязателен");
            }
            else if (Password.Length < 5)
            {
                AddError(nameof(Password), "Минимум 5 символов");
            }
        }

        public bool HasAllAuthErrors
        {
            get => _hasAllAuthErrors;
            set
            {
                _hasAllAuthErrors = value;
                OnPropertyChanged();
            }
        }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set { 
                _isAuthenticated = value; 
                OnPropertyChanged(); 
            }
        }

        public ICommand LoginCommand { get; }

        public MainWindowViewModel()
        {
            _userModel = new User();
            _employeeModel = new Employee();
        }
    }
}
