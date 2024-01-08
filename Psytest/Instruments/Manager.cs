using Psytest.Data;
using System.Windows.Controls;

namespace Psytest.Instruments
{
    public class Manager
    {
        //Основной фрейм навигации
        public static Frame FrameNavigation { get; set; }

        //Класс для обеспечения удобства передачи данных
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

        //Текст для изменения заголовка окна
        public static string NavigatingText { get; set; }
    }
}
