using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _02Vasylenko
{
    public class PersonViewModel : INotifyPropertyChanged 
    {
    
        private readonly Person _domObject;
        private readonly Action<bool> _showLoaderAction;
        public PersonViewModel(Action<bool> showLoader)
        {
            _domObject = new Person();
            new PersonManager();
            Persons = new ObservableCollection<Person>(DBAdapter.Persons);
            AddPersonCmd = new RelayCommand(Add);
            EditPersonCmd = new RelayCommand(Edit);
            SavePersonCmd = new RelayCommand(Save);
            DeletePersonCmd = new RelayCommand(Delete);
            _showLoaderAction = showLoader;
        }
        public PersonViewModel()
        {
            _domObject = new Person();
            new PersonManager();
            Persons = new ObservableCollection<Person>();
            AddPersonCmd = new RelayCommand(Add);
        }
        public string LastName
        {
            get => _domObject.LastName;
            set
            {
                _domObject.LastName = value;
                OnPropertyChanged("LastName");
            }
        }
        public string Name
        {
            get => _domObject.Name;
            set
            {
                _domObject.Name = value;
                OnPropertyChanged("Name");
            }
        }
       public string Email
        {
            get => _domObject.Email;
           set
            {
                _domObject.Email = value;
                OnPropertyChanged("Email");
            }
        }

        public DateTime DateOfBirth
        {
            get => _domObject.DateOfBirth;
            set
            {
                _domObject.DateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }
        public bool IsAdult => _domObject.IsAdult;
        public string SunSign => _domObject.SunSign;
        public string ChineseSign => _domObject.ChineseSign;
        public bool IsBirthday => _domObject.IsBirthday;

        public ObservableCollection<Person> Persons { get; }
        public ICommand AddPersonCmd { get; }
        public ICommand EditPersonCmd { get; }
        public ICommand SavePersonCmd { get; }
        public ICommand DeletePersonCmd { get; }
        public Person SelectedItem { get; set; }
        public async void Add(object obj)
        {
            _showLoaderAction.Invoke(true);
            
                await Task.Run((() =>
                { 
                    Thread.Sleep(2000);
                }));
            try
            {
                if (LastName == string.Empty)
                {
                    throw new IlligalInputException("Last Name");
                }
                if (Name == string.Empty)
                {
                    throw new IlligalInputException("Last Name");
                }
                if (!new EmailAddressAttribute().IsValid(Email))
                {
                    throw new IlligalEmailException(Email);
                }
                int check = DateTime.Today.Year - DateOfBirth.Year;
                if (DateTime.Today.Date < DateOfBirth.Date || check > 135)
                {
                    throw new IlligalDateException(DateOfBirth.ToString(CultureInfo.InvariantCulture) + " ");
                }
                var person = new Person { LastName = LastName, Name = Name, Email = Email, DateOfBirth = DateOfBirth };
                DBAdapter.AddPerson(LastName,Name,Email,DateOfBirth);
                Persons.Add(person);
                if (person.DateOfBirth.DayOfYear.Equals(DateTime.Today.DayOfYear)) MessageBox.Show("HappyBirthday");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ResetPerson();
            _showLoaderAction.Invoke(false);
        }
        public void Edit(object obj)
        {
            SetPerson(SelectedItem);
        }

        public void Save(object obj) {
            try
            {
                if (LastName == string.Empty)
                {
                    throw new IlligalInputException("Last Name");
                }
                if (Name == string.Empty)
                {
                    throw new IlligalInputException("Last Name");
                }
                if (!new EmailAddressAttribute().IsValid(Email))
                {
                    throw new IlligalEmailException(Email);
                }
                int check = DateTime.Today.Year - DateOfBirth.Year;
                if (DateTime.Today.Date < DateOfBirth.Date || check > 135)
                {
                    throw new IlligalDateException(DateOfBirth.ToString(CultureInfo.InvariantCulture) + " ");
                }

                SelectedItem.LastName = LastName;
                SelectedItem.Name = Name;
                SelectedItem.Email = Email;
                SelectedItem.DateOfBirth = DateOfBirth;
                if (DateOfBirth.DayOfYear.Equals(DateTime.Today.DayOfYear)) MessageBox.Show("HappyBirthday");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ResetPerson();
        }
        public void Delete(object obj)
        {
            Persons.Remove(SelectedItem);
            DBAdapter.Persons.Remove(SelectedItem);
        }
   
        private void ResetPerson()
        {
            LastName = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
        }
        private void SetPerson(Person p)
        {
            LastName = p.LastName;
            Name = p.Name;
            Email = p.Email;
            DateOfBirth = p.DateOfBirth;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        internal class IlligalInputException : Exception
        {
            public IlligalInputException(string error)
                : base("Error: this field" + error + " is required")
            { }
        }
        internal class IlligalDateException : Exception
        {
            public IlligalDateException(string error)
                : base("Error: illigal format of date: " + error + "You can`t be older than 135 years or have not born yet")
            { }
        }
        internal class IlligalEmailException : Exception
        {
            public IlligalEmailException(string error)
                : base("Error: illigal format of email: " + error)
            { }
        }
    }
}

