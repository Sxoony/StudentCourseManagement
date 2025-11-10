using System;
using System.Collections.Generic;
using System.Linq;

namespace Question_4
{
    internal class Program
    {
        // Represents a student with an ID and name
        class Student
        {
            public string StudentID { get; set; }
            public string StudentName { get; set; }

            public Student(string id, string name)
            {
                StudentID = id;
                StudentName = name;
            }
        }

        // Represents a course with an ID, name, and a list of enrolled students
        class Course
        {
            public string CourseID { get; set; }
            public string CourseName { get; set; }

            // Using LinkedList to store enrolled students
            public LinkedList<Student> EnrolledStudents { get; set; } = new LinkedList<Student>();

            public Course(string id, string name)
            {
                CourseID = id;
                CourseName = name;
            }
        }

        // Manages all courses and operations related to students and courses
        class CourseManager
        {
            // Stores all courses with the course ID as key
            private Dictionary<string, Course> courses = new Dictionary<string, Course>();

            // Adds a new course if it doesn't already exist
            public void AddCourse(string courseID, string courseName)
            {
                if (!courses.ContainsKey(courseID))
                {
                    courses[courseID] = new Course(courseID, courseName);
                    Console.WriteLine($"Course '{courseName}' added.");
                }
                else
                {
                    Console.WriteLine($"Course '{courseName}' already exists.");
                }
            }

            // Enrolls a student into the specified course
            public void EnrollStudent(string courseID, string studentID, string studentName)
            {
                if (courses.TryGetValue(courseID, out Course course))
                {
                    // Prevent duplicate enrollment by checking existing students
                    if (course.EnrolledStudents.Any(s => s.StudentID == studentID))
                    {
                        Console.WriteLine($"{studentName} is already enrolled in {course.CourseName}");
                        return;
                    }

                    // Add student to the course
                    course.EnrolledStudents.AddLast(new Student(studentID, studentName));
                    Console.WriteLine($"Enrolled {studentName} to {course.CourseName}.");
                }
                else
                {
                    Console.WriteLine($"Course '{courseID}' not found.");
                }
            }

            // Displays all students enrolled in a specified course
            public void DisplayEnrolledStudents(string courseID)
            {
                if (courses.TryGetValue(courseID, out Course course))
                {
                    Console.WriteLine($"\nStudents enrolled in {course.CourseName}:");

                    if (course.EnrolledStudents.Count == 0)
                    {
                        Console.WriteLine("No students enrolled.");
                        return;
                    }

                    // Loop through the list and display each student
                    foreach (var student in course.EnrolledStudents)
                    {
                        Console.WriteLine($"- {student.StudentID}: {student.StudentName}");
                    }
                }
                else
                {
                    Console.WriteLine($"Course '{courseID}' not found.");
                }
            }

            // Removes a student from a course based on student ID
            public void RemoveStudent(string courseID, string studentID)
            {
                if (courses.TryGetValue(courseID, out Course course))
                {
                    var node = course.EnrolledStudents.First;

                    // Loop through the LinkedList manually to find the student
                    while (node != null)
                    {
                        if (node.Value.StudentID == studentID)
                        {
                            course.EnrolledStudents.Remove(node);
                            Console.WriteLine($"Removed student {studentID} from {course.CourseName}");
                            return;
                        }
                        node = node.Next;
                    }

                    Console.WriteLine($"Student {studentID} not found in {course.CourseName}");
                }
                else
                {
                    Console.WriteLine($"Course '{courseID}' not found.");
                }
            }

            // Checks if a course exists
            public bool CourseExists(string courseID)
            {
                return courses.ContainsKey(courseID);
            }

            // Displays all available courses
            public void DisplayCourses()
            {
                Console.WriteLine("\nAvailable Courses:");
                foreach (var course in courses.Values)
                {
                    Console.WriteLine($"- {course.CourseID}: {course.CourseName}");
                }
            }
        }

        // Generates a random student ID like "EDUV12345"
        static string GenerateStudentID()
        {
            Random rng = new Random();
            return "EDUV" + rng.Next(10000, 99999);
        }

        // Generates a course ID by taking the first letter of each word and adding "IT" at the beginning
        static string GenerateCourseID(string courseName)
        {
            string[] words = courseName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string abbreviation = "IT";
            foreach (string word in words)
            {
                abbreviation += char.ToUpper(word[0]);
            }
            return abbreviation;
        }

        // Entry point of the application
        static void Main(string[] args)
        {
            CourseManager manager = new CourseManager();
            bool running = true;

            while (running)
            {
                // Display main menu
                Console.WriteLine("\n=== Course Management Menu ===");
                Console.WriteLine("1. Add a course");
                Console.WriteLine("2. Enroll a student");
                Console.WriteLine("3. Remove a student");
                Console.WriteLine("4. Display enrolled students");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option (1–5): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Add a new course
                        Console.Write("Enter course name: ");
                        string courseName = Console.ReadLine();
                        string courseID = GenerateCourseID(courseName);
                        manager.AddCourse(courseID, courseName);
                        break;

                    case "2":
                        // Enroll a student in a course
                        manager.DisplayCourses();
                        Console.Write("Enter course ID to enroll into: ");
                        string enrollID = Console.ReadLine();

                        if (!manager.CourseExists(enrollID))
                        {
                            Console.WriteLine("Course not found.");
                            break;
                        }

                        Console.Write("Enter student name: ");
                        string studentName = Console.ReadLine();
                        string studentID = GenerateStudentID();
                        manager.EnrollStudent(enrollID, studentID, studentName);
                        break;

                    case "3":
                        // Remove a student from a course
                        manager.DisplayCourses();
                        Console.Write("Enter course ID to remove from: ");
                        string removeID = Console.ReadLine();

                        if (!manager.CourseExists(removeID))
                        {
                            Console.WriteLine("Course not found.");
                            break;
                        }

                        manager.DisplayEnrolledStudents(removeID);
                        Console.Write("Enter student ID to remove: ");
                        string removeStudentID = Console.ReadLine();
                        manager.RemoveStudent(removeID, removeStudentID);
                        break;

                    case "4":
                        // Display students in a course
                        manager.DisplayCourses();
                        Console.Write("Enter course ID to view students: ");
                        string displayID = Console.ReadLine();
                        manager.DisplayEnrolledStudents(displayID);
                        break;

                    case "5":
                        // Exit the application
                        running = false;
                        Console.WriteLine("Exiting the program...");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }
    }
}
