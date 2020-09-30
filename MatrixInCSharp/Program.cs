using System;
using System.Linq;
using System.Threading;


namespace MatrixInCSharp
{

    public class Program
    {
        /*public static int Cols2Print = 119;
        public static int ColsAll = 120;
        public static int runs = 400;
        static Semaphore sem = new Semaphore(0, ColsAll);
        public static string alpha1 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static char[] alpha = alpha1.ToArray();
        volatile static char[] line = new char[ColsAll];

        public static void MyThread(object arg)
        {
            Random rd = new Random();
            int j = 0, i = (int)arg;
            bool win = false;

            Console.WriteLine($"thread {i} started");
            for (int k = 0; k < runs; k++)
            {
                sem.WaitOne();
                if (rd.Next() % 500 == 0 || win)
                {
                    win = true;
                    line[i] = alpha[j];
                    if (alpha[j] == 'Z')
                    {
                        win = false;
                        j = 0;
                    }
                    else
                    {
                        j++;
                    }
                }
                else
                {
                    line[i] = ' ';
                }
            }
        }*/
        public static int Cols2Print = 119;
        public static int ColsAll = 120;
        volatile static char[] line = new char[ColsAll];
        public static string alpha1 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static Semaphore semaphore = new Semaphore(0, Cols2Print);
        static Semaphore sem = new Semaphore(0, Cols2Print);
        static Random rand = new Random();
        public static void TestThread(object id) 
        {
            semaphore.WaitOne();
            Console.WriteLine($"Welcome From Thread {(int)id}");
            if (rand.Next() % 2 == 0)
            {
                line[(int)id] = alpha1[rand.Next() % 52];
            }
            else 
            {
                line[(int)id] = ' ';
            }
            Thread.Sleep(2000);
            sem.Release(1);
        }

        static void Main(string[] args)
        {

            //line[Cols2Print] = '0';
            Thread[] pIds = new Thread[Cols2Print];
            Console.WriteLine("main: begin\n");

            for (int k = 0; k < Cols2Print; k++)
            {
                ParameterizedThreadStart start = new ParameterizedThreadStart(TestThread);
                pIds[k] = new Thread(start)
                {
                    Name = "thread_" + k
                };
                pIds[k].Start(k);
            }

            Thread.Sleep(1000);

            semaphore.Release(Cols2Print);
            Console.WriteLine("Testing release");

            

            Thread.Sleep(3000);

            for (int i = 0; i < Cols2Print; i++)
            { 
                semaphore.Release(1);
            }

            for (int i = 0; i < Cols2Print; i++)
            {
                Console.WriteLine($"Waiting for thread {i}");
                sem.WaitOne();
            }

            Console.WriteLine(line);
            Console.WriteLine("...DONE!...\n");
        }
    }
}
