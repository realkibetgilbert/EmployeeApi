namespace EmployeeApi.Exceptions
{
    public class StudentAgeException:Exception
    {
        public int Age { get; set; }

        public StudentAgeException(string message) : base(message) { }
    }
}
