using Psytest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Psytest.Instruments
{
    public class Manager
    {
        //Основной фрейм навигации
        public static Frame FrameNavigation { get; set; }
        public class Student
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Age { get; set; }
            public Group Group { get; set; }
            public Gender Gender { get; set; }
            public Student(string name, string surname, int age, Group group, Gender gender)
            {
                Name = name; 
                Surname = surname; 
                Age = age; 
                Group = group;
                Gender = gender;
            }
        }
    }
}
