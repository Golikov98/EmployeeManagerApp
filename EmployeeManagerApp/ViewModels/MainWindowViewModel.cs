using EmployeeManagerApp.Api;
using EmployeeManagerApp.Commands;
using EmployeeManagerApp.Models;
using EmployeeManagerApp.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Input;

namespace EmployeeManagerApp.ViewModels
{
    internal class MainWindowViewModel: BaseViewModel
    {
        //Флаг успешной авторизации пользователя
        private bool _isAuthenticated = false;

        //Флаг отображения grid-а авторизации
        private bool _showAuthGrid = true;

        private string _login = string.Empty;
        private string _password = string.Empty;

        //Флаг наличия/отсутствия ошибок авторизации
        private bool _hasAllAuthErrors = false;

        //Задача авторизации
        private readonly AuthTask _authTask;

        //Данные авторизованного пользователя
        private User _currentUser;

        private string _currentUserName = "Пользователь";

        //Задача получения списка сотрудников
        private readonly EmployeesTask _employeesTask;

        //Коллекция всех сотрудников
        public ObservableCollection<Employee> Employees { get; } = new ObservableCollection<Employee>();
        private ICollectionView _employeesView;

        private Employee _selectedEmployee = new Employee 
        { 
            Name = "Имя сотрудника"
        };

        private bool _addEmployeeEnabled = false;
        private bool _editEmployeeEnabled = false;
        private bool _deleteEmployeeEnabled = false;
        private string _selectedSort = "По алфавиту";

        /// <summary>
        /// Логин пользователя (email)
        /// При изменении выполняется валидация логина и пароля,
        /// обновляется состояние команды входа.
        /// </summary>
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

        /// <summary>
        /// Проверяет логин на пустоту и корректность email-формата.
        /// Добавляет ошибки в коллекцию ошибок MainWindowViewModel.
        /// </summary>
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

        /// <summary>
        /// Пароль пользователя.
        /// При изменении выполняется валидация и обновляется состояние команды входа.
        /// </summary>
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

        /// <summary>
        /// Проверяет пароль на пустоту и минимальную длину.
        /// </summary>
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

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
            }
        }

        public string CurrentUserName
        {
            get => _currentUserName;
            set
            {
                _currentUserName = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Команда входа пользователя.
        /// Асинхронная, доступна только при валидных данных.
        /// </summary>
        public ICommand LoginCommand { get; }
        public ICommand GetEmployeesCommand {  get; }

        /// <summary>
        /// Асинхронно выполняет авторизацию пользователя через API.
        /// </summary>
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

                CurrentUser = new User
                {
                    Password = Password,
                    AccessRights = result.AccessRights,
                    Id = result.Id,
                    Department = result.Employee.Department,
                    Position = result.Employee.Position,
                    StartDate = result.Employee.StartDate,
                    FinishDate = result.Employee.FinishDate,
                    WorkPhoneNumber = result.Employee.WorkPhoneNumber,
                    WorkEmail = result.Employee.WorkEmail,
                    WorkAddress = result.Employee.WorkAddress,
                    Name = result.Employee.Name,
                    Birthday = result.Employee.Birthday,
                    MaritalStatus = result.Employee.MaritalStatus,
                    PersonalPhoneNumber = result.Employee.PersonalPhoneNumber,
                    PersonalEmail = result.Employee.PersonalEmail,
                    PersonalAdress = result.Employee.PersonalAdress,
                    Education = result.Employee.Education
                };

                if (CurrentUser.Name is null)
                {
                    CurrentUserName = "Пользователь";
                }
                else
                {
                    CurrentUserName = CurrentUser.Name;
                }

                if (CurrentUser.AccessRights >= 999) 
                {
                    AddEmployeeEnabled = true;
                    EditEmployeeEnabled = true;
                    DeleteEmployeeEnabled = true;
                }
                else
                {
                    AddEmployeeEnabled = false;
                    EditEmployeeEnabled = false;
                    DeleteEmployeeEnabled = false;
                }


                IsAuthenticated = true;
                await EmployeesAsync();
            }
            catch (ApiException ex)
            {
                AddError(nameof(Login), ex.Message);
                HasAllAuthErrors = true;
            }
        }

        /// <summary>
        /// Определяет, может ли быть выполнена команда входа.
        /// Команда доступна, если нет ошибок и заполнены логин и пароль.
        /// </summary>
        private bool CanLogin()
        {
            return !HasErrors && !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        }

        /// <summary>
        /// Асинхронно загружает список сотрудников с сервера и обновляет коллекцию Employees
        /// </summary>
        private async Task EmployeesAsync()
        {
            try
            {
                var employees = await _employeesTask.getAllEmployeesAsync();

                Employees.Clear();

                foreach (var employee in employees) 
                {
                    Employees.Add(employee);
                }

                SortCollection(_selectedSort);
            }
            catch (ApiException ex)
            {
                AddError(nameof(Employees), ex.Message);
            }
        }

        public ICollectionView EmployeesView => _employeesView;

        public Employee SelectedEmployee 
        { 
            get => _selectedEmployee; 
            set
            {
                _selectedEmployee = value;

                if (_selectedEmployee.Id == CurrentUser.Id)
                {
                    EditEmployeeEnabled = false;
                    DeleteEmployeeEnabled = false;
                }
                else
                {
                    if(CurrentUser.AccessRights >= 999)
                    {
                        EditEmployeeEnabled = true;
                        DeleteEmployeeEnabled = true;
                    }
                }

                OnPropertyChanged();
            } 
        }

        public bool AddEmployeeEnabled
        {
            get => _addEmployeeEnabled;
            set
            {
                _addEmployeeEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool EditEmployeeEnabled
        {
            get => _editEmployeeEnabled;
            set
            {
                _editEmployeeEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool DeleteEmployeeEnabled
        {
            get => _deleteEmployeeEnabled;
            set
            {
                _deleteEmployeeEnabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Коллекция доступных сортировок
        /// </summary>
        public ObservableCollection<string> SortsList { get; } = new ObservableCollection<string> { "По алфавиту", "По возрастанию стажа", "По убыванию стажа", "По возрасту" };

        public string SelectedSort
        {
            get => _selectedSort;
            set
            {
                _selectedSort = value;
                OnPropertyChanged();
                SortCollection(_selectedSort);
            }
        }

        /// <summary>
        /// Метод сортирующий коллекцию сотрудников
        /// </summary>
        /// <param name="method"></param>
        private void SortCollection(string method)
        {
            if (_employeesView == null)
            {
                return;
            }

            _employeesView.SortDescriptions.Clear();

            switch (method)
            {
                case "По алфавиту":
                    _employeesView.SortDescriptions.Add(
                        new SortDescription(nameof(Employee.Name), ListSortDirection.Ascending));
                    break;

                case "По возрастанию стажа":
                    _employeesView.SortDescriptions.Add(
                        new SortDescription(nameof(Employee.Experience), ListSortDirection.Ascending));
                    break;

                case "По убыванию стажа":
                    _employeesView.SortDescriptions.Add(
                        new SortDescription(nameof(Employee.Experience), ListSortDirection.Descending));
                    break;

                case "По возрасту":
                    _employeesView.SortDescriptions.Add(
                        new SortDescription(nameof(Employee.Age), ListSortDirection.Ascending));
                    break;
            }

            _employeesView.Refresh();
        }

        public MainWindowViewModel()
        {
            IApiClient apiClient = new ApiClient("http://localhost:5262/api/");
            _authTask = new AuthTask(apiClient);
            _employeesTask = new EmployeesTask(apiClient);

            LoginCommand = new AsyncRelayCommand(LoginAsync, CanLogin);
            GetEmployeesCommand = new AsyncRelayCommand(EmployeesAsync);

            _employeesView = CollectionViewSource.GetDefaultView(Employees);
        }
    }
}
