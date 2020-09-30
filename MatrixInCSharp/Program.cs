using System;
using System.Linq;
using System.Threading;


namespace MatrixInCSharp
{

    public class Program
    {
        static void Main(string[] args)
        {
            int DefaultValue = 5000;
            int repetitions = 0;
            string message = "";
            Matrix matrix = new Matrix();
            Console.WriteLine("Enter How much repetitions you want(greater than 5000): ");
            try
            {
                repetitions = Convert.ToInt32(Console.ReadLine());
                matrix.MatrixRun(repetitions);
            }
            catch(Exception e) 
            {
                message = e.Message;
                Console.WriteLine(message);
                Console.WriteLine("Invalid Input, running with default values");
                Thread.Sleep(5000);        
                matrix.MatrixRun(DefaultValue);
            }
        }
    }
}

