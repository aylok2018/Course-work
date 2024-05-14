using System;
using System.Collections.Generic;

namespace StudentManagementSystem
{
    class Program
    {
        static List<Student> students = new List<Student>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Додати студента");
                Console.WriteLine("2. Видалити студента");
                Console.WriteLine("3. Показати інформацію про студентів");
                Console.WriteLine("0. Вийти");
                Console.Write("Виберіть опцію: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        RemoveStudent();
                        break;
                    case "3":
                        ShowStudentInfo();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void AddStudent()
        {
            Console.Write("Введіть ім'я студента: ");
            string firstName = Console.ReadLine();
            Console.Write("Введіть прізвище студента: ");
            string lastName = Console.ReadLine();
            Console.Write("Введіть дату народження студента (у форматі YYYY-MM-DD): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
            {
                Console.WriteLine("Неправильний формат дати.");
                return;
            }
            Student student = new Student(firstName, lastName, dateOfBirth);


            AddStudentCourseInfo(student);

            students.Add(student); ;
            Console.WriteLine("Студент доданий успішно.");
        }
        static void AddStudentCourseInfo(Student student)
        {
            Console.Write("Скільки предметів ви хочете додати для цього студента? ");
            if (!int.TryParse(Console.ReadLine(), out int numCourses) || numCourses <= 0)
            {
                return;
            }

            for (int i = 0; i < numCourses; i++)
            {
                Console.WriteLine($"Предмет #{i + 1}:");
                Console.Write("Назва предмету: ");
                string courseName = Console.ReadLine();
                Console.Write("Опис предмету: ");
                string courseDescription = Console.ReadLine();
                Console.Write("Ім'я викладача: ");
                string instructorName = Console.ReadLine();

                Course course = new Course(courseName, courseDescription, instructorName);
                student.AddCourse(course);

                Console.Write("Оцінка за предмет: ");
                if (!int.TryParse(Console.ReadLine(), out int grade) || grade < 0 || grade > 100)
                {
                    return;
                }

                student.AddGrade(course, grade);
            }
        }

        static void RemoveStudent()
        {
            Console.Write("Введіть прізвище студента для видалення: ");
            string lastName = Console.ReadLine();

            var studentToRemove = students.FirstOrDefault(student => student.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));

            if (studentToRemove != null)
            {
                students.Remove(studentToRemove);
                Console.WriteLine($"Студент {lastName} видалений успішно.");
            }
            else
            {
                Console.WriteLine($"Студента з прізвищем {lastName} немає в списку.");
            }
        }

        static void ShowStudentInfo()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("Список студентів порожній.");
                return;
            }

            Console.WriteLine("Інформація про студентів:");
            foreach (var student in students)
            {
                Console.WriteLine("Ім'я: {0}, Прізвище: {1}, Дата народження: {2}", student.FirstName, student.LastName, student.DateOfBirth.ToShortDateString());
                foreach (var courseGrade in student.CourseGrades)
                {
                    Console.WriteLine("Курс: {0}, Оцінка: {1}", courseGrade.Key.Name, courseGrade.Value);
                    if (courseGrade.Value < 60)
                    {
                        Console.Write(" - Не склав залік/екзамен");
                    }
                }
                Console.WriteLine();
            }
        }
    }

    class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Dictionary<Course, int> CourseGrades { get; set; }

        public Student(string firstName, string lastName, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            CourseGrades = new Dictionary<Course, int>();
        }

        public void AddCourse(Course course)
        {
            CourseGrades.Add(course, 0);
        }

        public void AddGrade(Course course, int grade)
        {
            if (CourseGrades.ContainsKey(course))
            {
                CourseGrades[course] = grade;
            }
        }
    }

    class Course
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }

        public Course(string name, string description, string instructor)
        {
            Name = name;
            Description = description;
            Instructor = instructor;
        }
    }
}
