using ConsoleApp1.MaskAttr;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            List<Person> testDatas = new List<Person>();

            for (int i = 0; i < 10000; i++)
            {
                testDatas.Add(new Person {
                    IdNo = "F123456798",
                    a = "AAAAA",
                    b = "BBBBBB"
                });
            }

            for (int i = 0; i < 10; i++)
            {
                sw.Reset();
                sw = Stopwatch.StartNew();

                testDatas.ToMaskByRef();

                sw.Stop();
                TimeSpan el = sw.Elapsed;
                Console.WriteLine($"Reflection {i+1} 花費 {el} ");

                sw.Reset();
                sw = Stopwatch.StartNew();

                testDatas.ToMaskByExpression();

                sw.Stop();
                TimeSpan el2 = sw.Elapsed;                
                Console.WriteLine($"Expression {i + 1} 花費 {el2} ");

                sw.Reset();
                sw = Stopwatch.StartNew();

                testDatas.ToMaskByDelegate();

                sw.Stop();
                TimeSpan el3 = sw.Elapsed;
                Console.WriteLine($"Delegate   {i + 1} 花費 {el3} ");

                Console.WriteLine("===============================");
            }

            Console.ReadLine();
        }
    }

    class Person
    {
        [StringMask]
        public string IdNo { get; set; }

        [StringMask]
        public string a { get; set; }

        [StringMask]
        public string b { get; set; }
    }
}
