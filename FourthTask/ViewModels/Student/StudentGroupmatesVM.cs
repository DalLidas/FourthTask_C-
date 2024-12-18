﻿using FourthTask.PageNavigation.Ioc;
using FourthTask.ViewModels.Base;
using System.Collections.ObjectModel;
using FourthTask.DataBase;


namespace FourthTask.ViewModels
{
    internal class StudentGroupmatesVM : ViewModelBase
    {

        public ObservableCollection<Student> Groupmates { get; set; }

        public StudentGroupmatesVM()
        {
            Groupmates = new ObservableCollection<Student>();

            GetData();
        }

        
        private async void GetData()
        {
            if (Ioc.model is not null)
            {
                List<Student> buffStudent = await Ioc.model.GetStudentStudentsGroupmates();
                if (buffStudent is not null)
                {
                    Groupmates.Clear();
                    foreach (Student groupmate in buffStudent.OrderBy(x => x.ID))
                    {
                        Groupmates.Add(groupmate);
                    }
                }
            }
        }


        private string _Title = "Технологический ВУЗ \"Сессия\"";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }


        private Student? _SelectedItem;
        public Student? SelectedItem
        {
            get => _SelectedItem;
            set => Set(ref _SelectedItem, value);
        }
    }
} 