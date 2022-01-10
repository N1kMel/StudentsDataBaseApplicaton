using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDataBaseApplicaton
{
    class Student : IComparable

    {
        public string fullName, datetime, institute, group, course;
        public double avgMark;
        public string form = null;
        public int level = 0, debts = 0;

        public int id { get; set; }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string Datetime
        {
            get { return datetime; }
            set { datetime = value; }
        }
        public string Institute
        {
            get { return institute; }
            set { institute = value; }
        }
        public string Group
        {
            get { return group; }
            set { group = value; }
        }

        public string Course
        {
            get { return course; }
            set { course = value; }
        }
        public double AvgMark
        {
            get { return avgMark; }
            set { avgMark = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public int Debts
        {
            get { return debts; }
            set { debts = value; }
        }

        public string Form
        {
            get { return form; }
            set { form = value; }
        }

        public override string ToString()
        {
            return "id: " + id + " | fullName: " + fullName + " | birthday: " + datetime + " | institute: " + institute + " | group: " + group + " | course: " + course + " | avgMark: " + avgMark + " | debts: " + debts + " | form: " + form + " | level: " + level + " | " + '\n';
        }

        public int CompareTo(object obj)
        {
            Student s = obj as Student;
            if (this.avgMark < s.avgMark)
            {
                return -1;
            }
            if (this.avgMark == s.avgMark)
            {
                return 0;
            }
            if (this.avgMark > s.avgMark)
            {
                return 1;
            }
            return 0;
        }

        public Student() { }
        public Student(string fullName, string datetime, string institute, string group, string course, double avgMark, int debts, string form, int level)
        {

            this.fullName = fullName;
            this.datetime = datetime;
            this.institute = institute;
            this.group = group;
            this.course = course;
            this.avgMark = avgMark;
            this.debts = debts;
            this.form = form;
            this.level = level;
        }


    }
}
