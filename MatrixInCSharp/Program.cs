using System;
using System.Linq;
using System.Threading;


namespace MatrixInCSharp
{

    public class Program
    {
        public static int runs = 20000;
        public static int Cols2Print = 119;
        public static int ColsAll = 120;
        volatile static char[] line = new char[ColsAll];
        public static string alpha1 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static Semaphore semaphore = new Semaphore(0, Cols2Print);
        static Semaphore sem = new Semaphore(0, Cols2Print);
        public static void TestThread(object id)
        {
            Random rand = new Random();
            int j = 0, i = (int)id;
            bool win = false;
            for (int k = 0; k < runs; k++)
            {
                semaphore.WaitOne();
                if (rand.Next() % 500 == 0 || win)
                {
                    win = true;
                    line[i] = alpha1[j];
                    if (alpha1[j] == 'Z')
                    {
                        win = false;
                        j = 0;
                    }
                    else
                    {
                        j++;
                    }
                    sem.Release(1);
                } else
                {
                    line[i] = ' ';
                    sem.Release(1);
                }
            }
            Thread.Sleep(5);
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

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

            for (int j = 0; j < runs; j++)
            {
                for (int i = 0; i < Cols2Print; i++)
                {
                    semaphore.Release(1);
                }

                for (int i = 0; i < Cols2Print; i++)
                {
                    sem.WaitOne();
                }

                Console.WriteLine(line);
            }

            Console.WriteLine("...DONE!...\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

