using System;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace StudentsDataBaseApplicaton
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        // 11.10.2021
        // 0123456789

        // 1.10.2233
        // 012345678
        static bool checkDate(string line)
        {

            if (line.Length > 10 || line.Length < 9) { return false; }

            if (line.Length == 10)
            {
                string num = line.Substring(0, 2) + line.Substring(3, 2) + line.Substring(6, 4);

                foreach (char c in num)
                {
                    if (!Char.IsDigit(c)) { return false; }
                }

                if (int.Parse(line.Substring(0, 2)) > 31) { return false; }

                if (int.Parse(line.Substring(3, 2)) > 12) { return false; }

                if (line[2] != '.' && line[5] != '.')
                {
                    return false;
                }

                return true;
            }


            // 1.10.2233
            // 012345678

            if (line.Length == 9)
            {
                string num = line.Substring(0, 1) + line.Substring(2, 2) + line.Substring(5, 4);

                foreach (char c in num)
                {
                    if (!Char.IsDigit(c)) { return false; }
                }

                if (int.Parse(line.Substring(0, 1)) > 31) { return false; }

                if (int.Parse(line.Substring(2, 2)) > 12) { return false; }

                if (line[1] != '.' && line[4] != '.')
                {
                    return false;
                }

                return true;
            }


            return false;

        }

        static bool checkName(string line)
        {
            foreach (char c in line)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c)) { return false; }
            }
            return true;
        }

        static bool checkStudent(string[] s)
        {

            if (s.Length != 12) { return false; }
            double a;
            int cours;
            int debts;
            string form = s[10];
            int level;


            if (checkName(s[2]) && checkName(s[3]) && checkName(s[1]) && checkDate(s[4]) && double.TryParse(s[8], out a) && int.TryParse(s[7], out cours) && int.TryParse(s[9], out debts) && int.TryParse(s[11], out level))
            {
                if (a <= 5 && a >= 2 & cours > 0 && cours <= 5 && level > 0 && level <= 10 && (form == "b" || form == "s" || form == "m") && debts > -1) { return true; }
            }

            return false;


        }

        static bool isGoodCommand(string[] s)
        {
            if (s.Length == 0) { return false; }
            switch (s[0])
            {
                case "search":
                    if (s.Length < 3) { return false; }
                    if (s[1] == "date") { if (checkDate(s[2])) return true; }
                    if (s[1] == "name") { if (checkName(s[2])) return true; }
                    return false;

                case "add":
                    if (s.Length < 11) { return false; }
                    if (checkStudent(s)) { return true; }
                    return false;

                case "delete":
                    if (s.Length < 2) { return false; }
                    if (checkDate(s[1])) { return true; }
                    if (s[1] == "duplicate") return true;
                    return false;

                case "sort":
                    return true;

                case "min":
                    return true;

                case "max":
                    return true;

                case "duplicate":
                    return true;

                case "json":
                    if (s.Length != 2) { return false; }
                    if (s[1] == "des") { return true; }
                    if (s[1] == "ser") { return true; }

                    return false;

                default:
                    return false;


            }
        }

        static void Main(string[] args)
        {
            AppContext db;
            db = new AppContext();


            List<Student> stud = db.Students.ToList();


            while (true)
            {
                string lineComm = Console.ReadLine();
                string[] command = lineComm.Split(' ');

                if (command[0] == "break") { break; }

                if (isGoodCommand(command))
                {

                    if (command[0] == "search")
                    {
                        if (command[1] == "date")
                        {
                            List<Student> found = stud.FindAll(item => item.datetime == command[2]);
                            foreach (Student foundSt in found)
                                Console.WriteLine(foundSt.ToString());
                        }
                        if (command[1] == "name")
                        {
                            List<Student> found = stud.FindAll(item => item.FullName.Contains(command[2]));
                            foreach (Student foundSt in found)
                                Console.WriteLine(foundSt.ToString());

                        }


                    }

                    if (command[0] == "delete")
                    {
                        if (checkDate(command[1]))
                        {
                            db.Students.Remove(stud.Find(item => item.datetime == command[1]));
                            db.SaveChanges();
                            stud = db.Students.ToList();
                        }

                        if (command[1] == "duplicate")
                        {
                            Console.WriteLine("v dupl");
                            for (int i = 0; i < stud.Count() - 1; i++)
                            {
                                for (int j = i + 1; j < stud.Count(); j++)
                                {
                                    if (stud[i].fullName == stud[j].fullName)
                                    {
                                        Console.WriteLine(stud[i].fullName + " deleted from db" + '\n');
                                        db.Students.Remove(stud[i]);
                                    }
                                }
                            }
                            db.SaveChanges();
                            stud = db.Students.ToList();
                        }
                    }

                    if (command[0] == "sort")
                    {
                        stud.Sort();
                        foreach (Student s in stud)
                        {
                            Console.WriteLine(s.ToString());
                        }
                        stud = db.Students.ToList();
                    }

                    if (command[0] == "add")
                    {
                        string FullName = command[1] + " " + command[2] + " " + command[3];
                        Student stAdd = new Student(FullName, command[4], command[5], command[6], command[7], Math.Round(double.Parse(command[8]), 1), int.Parse(command[9]), command[10], int.Parse(command[11]));
                        db.Students.Add(stAdd);
                        db.SaveChanges();

                        Console.WriteLine("Student has been added" + '\n');
                        stud = db.Students.ToList();
                    }

                    if (command[0] == "min")
                    {
                        stud.Sort();
                        Console.WriteLine(stud[0].ToString());

                        double min = stud[0].avgMark;
                        int i = 1;

                        while (stud[i].avgMark == min)
                        {
                            Console.WriteLine(stud[i].ToString());
                            i++;
                        }
                    }

                    if (command[0] == "max")
                    {

                        stud.Sort();
                        int i = stud.Count() - 1;

                        Console.WriteLine(stud[i].ToString());

                        double max = stud[i].avgMark;


                        while (stud[i - 1].avgMark == max)
                        {
                            Console.WriteLine(stud[i - 1].ToString());
                            i--;
                        }

                    }

                    if (command[0] == "duplicate")
                    {
                        stud.Sort();


                        bool last = false;
                        for (int i = 0; i < stud.Count() - 1; i++)
                        {

                            if (stud[i].avgMark == stud[i + 1].avgMark)
                            {

                                Console.WriteLine(stud[i].ToString());
                                last = true;

                            }
                            else { if (last) { Console.WriteLine(stud[i].ToString()); last = false; } }

                            if (i + 2 == stud.Count())
                            {
                                if (stud[i].avgMark == stud[i + 1].avgMark) { Console.WriteLine(stud[i + 1].ToString()); }

                            }
                        }
                    }

                    if (command[0] == "json")
                    {
                        if (command[1] == "ser")
                        {
                            File.WriteAllText("Students.json", string.Empty);
                            StreamWriter sw = new StreamWriter("Students.json");

                            sw.WriteLine(JsonConvert.SerializeObject(stud));
                            sw.Close();
                        }


                        if (command[1] == "des")
                        {

                            StreamReader sr = new StreamReader("Students.json");
                            List<Student> s = JsonConvert.DeserializeObject<List<Student>>(sr.ReadLine());

                            foreach (Student s1 in s)
                            {
                                db.Students.Add(s1);
                            }

                            stud = db.Students.ToList();


                            sr.Close();


                        }

                    }

                }
                db.SaveChanges();
            }
        }
    }
}
