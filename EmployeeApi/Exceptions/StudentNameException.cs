namespace EmployeeApi.Exceptions
{
    public class StudentNameException:Exception
    {

        public string Name { get; set; }



        public StudentNameException(string message):base(message) { }

        public StudentNameException(string message,Exception innerException):base(message, innerException) { }

       
        public StudentNameException( string message,string name):base(message)
        {

           Name=name;
        }

        

        
    }
}
