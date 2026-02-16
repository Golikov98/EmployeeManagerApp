using EmployeeManagerApp.Api;
using EmployeeManagerApp.Commands;
using EmployeeManagerApp.Models;
using EmployeeManagerApp.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace EmployeeManagerApp.ViewModels
{
    internal class MainWindowViewModel: BaseViewModel
    {
        private bool _isAuthenticated = false;
        private bool _showAuthGrid = true;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private bool _hasAllAuthErrors = false;
        private readonly AuthTask _authTask;

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

                ((AsyncRelayCommand)LoginCommand).RaiseCanExecuteChanged();
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

                ((AsyncRelayCommand)LoginCommand).RaiseCanExecuteChanged();
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
                ShowAuthGrid = !value;
                OnPropertyChanged(); 
            }
        }
        public bool ShowAuthGrid
        {
            get => _showAuthGrid;
            set
            {
                _showAuthGrid = value;
                OnPropertyChanged();
            }
        }


        public ICommand LoginCommand { get; }

        private async Task LoginAsync()
        {
            try
            {
                var request = new LoginRequest
                {
                    Email = Login,
                    Password = Password
                };

                var result = await _authTask.LoginAsync(request);

                IsAuthenticated = true;
            }
            catch (ApiException ex)
            {
                AddError(nameof(Login), ex.Message);
                HasAllAuthErrors = true;
            }
        }

        private bool CanLogin()
        {
            return !HasErrors && !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        }

        public MainWindowViewModel()
        {
            IApiClient apiClient = new ApiClient("http://localhost:5262/api/");
            _authTask = new AuthTask(apiClient);

            LoginCommand = new AsyncRelayCommand(LoginAsync, CanLogin);
        }
    }
}
