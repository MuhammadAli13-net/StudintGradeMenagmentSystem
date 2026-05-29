using System;
using System.Collections.Generic;
using System.Globalization;

namespace StudintGradeMenagmentSystem
{
    class Student
    {
        public int Id { get; set; }
        public string Ism { get; set; } = string.Empty;
        public string Familiya { get; set; } = string.Empty;
        public int Yosh { get; set; }
        public string Guruh { get; set; } = string.Empty;
        public double Matematika { get; set; }
        public double Ingliz { get; set; }
        public double Dasturlash { get; set; }

        public double OrtachaBaho()
        {
            return (Matematika + Ingliz + Dasturlash) / 3.0;
        }
    }

    class Program
    {
        static List<Student> students = new List<Student>();
        static int nextId = 1;

        static void Main()
        {
            while (true)
            {
                ShowMenu();
                var choice = (Console.ReadLine() ?? string.Empty).Trim();

                switch (choice)
                {
                    case "1": TalabaQoshish(); break;
                    case "2": TalabalarniKorish(); break;
                    case "3": BahoQoshish(); break;
                    case "4": OrtachaBahoniHisoblash(); break;
                    case "5": EngYaxshiTalaba(); break;
                    case "6": FailedStudentlar(); break;
                    case "7": TalabaQidirish(); break;
                    case "8":
                        Console.WriteLine("Dastur yopilmoqda...");
                        return;
                    default:
                        Console.WriteLine("Noto'g'ri tanlov!");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\n==================================");
            Console.WriteLine("   🎓 STUDENT GRADE SYSTEM");
            Console.WriteLine("==================================");
            Console.WriteLine("1. Talaba qo'shish");
            Console.WriteLine("2. Talabalarni ko'rish");
            Console.WriteLine("3. Baho qo'shish");
            Console.WriteLine("4. O'rtacha bahoni hisoblash");
            Console.WriteLine("5. Eng yaxshi talabani topish");
            Console.WriteLine("6. Failed studentlar");
            Console.WriteLine("7. Talaba qidirish");
            Console.WriteLine("8. Exit");
            Console.WriteLine("==================================");
            Console.Write("Tanlov: ");
        }

        static void TalabaQoshish()
        {
            string ism = ReadNonEmptyString("Ism: ");
            string familiya = ReadNonEmptyString("Familiya: ");
            int yosh = ReadInt("Yosh: ");
            string guruh = ReadNonEmptyString("Guruh: ");

            var stu = new Student
            {
                Id = nextId++,
                Ism = ism,
                Familiya = familiya,
                Yosh = yosh,
                Guruh = guruh
            };

            students.Add(stu);
            Console.WriteLine($"✅ Talaba qo'shildi! ID: {stu.Id}");
        }

        static void TalabalarniKorish()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("Talabalar yo'q.");
                return;
            }

            Console.WriteLine("\n{0,-5} {1,-15} {2,-15} {3,-8} {4,-10} {5}",
                "ID", "Ism", "Familiya", "Guruh", "O'rtacha", "Holat");
            Console.WriteLine(new string('-', 70));

            foreach (var s in students)
            {
                double avg = s.OrtachaBaho();
                string holat = avg >= 60 ? "Pass" : "Fail";
                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-8} {4,-10:F1} {5}",
                    s.Id, s.Ism, s.Familiya, s.Guruh, avg, holat);
            }
        }

        static void BahoQoshish()
        {
            int id = ReadInt("Talaba ID: ");
            var s = students.Find(x => x.Id == id);
            if (s == null)
            {
                Console.WriteLine("Talaba topilmadi.");
                return;
            }

            s.Matematika = ReadDoubleInRange("Matematika bahosi (0-100): ", 0, 100);
            s.Ingliz = ReadDoubleInRange("Ingliz tili bahosi (0-100): ", 0, 100);
            s.Dasturlash = ReadDoubleInRange("Dasturlash bahosi (0-100): ", 0, 100);

            Console.WriteLine($"✅ Baholar saqlandi! O'rtacha: {s.OrtachaBaho():F1}");
        }

        static void OrtachaBahoniHisoblash()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("Talabalar yo'q.");
                return;
            }

            Console.WriteLine("\nHar bir talabaning o'rtacha bahosi:");
            Console.WriteLine(new string('-', 40));

            foreach (var s in students)
            {
                Console.WriteLine($"[{s.Id}] {s.Ism} {s.Familiya}: {s.OrtachaBaho():F1}");
            }
        }

        static void EngYaxshiTalaba()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("Talabalar yo'q.");
                return;
            }

            Student eng = null!;
            double best = double.MinValue;
            foreach (var s in students)
            {
                double avg = s.OrtachaBaho();
                if (avg > best)
                {
                    best = avg;
                    eng = s;
                }
            }

            Console.WriteLine($"\n🏆 Eng yaxshi talaba:");
            Console.WriteLine($"   {eng.Ism} {eng.Familiya} | Guruh: {eng.Guruh} | O'rtacha: {eng.OrtachaBaho():F1}");
        }

        static void FailedStudentlar()
        {
            var failed = students.FindAll(s => s.OrtachaBaho() < 60);

            if (failed.Count == 0)
            {
                Console.WriteLine("Failed talaba yo'q!");
                return;
            }

            Console.WriteLine("\n❌ Failed studentlar (o'rtacha < 60):");
            Console.WriteLine(new string('-', 40));

            foreach (var s in failed)
                Console.WriteLine($"[{s.Id}] {s.Ism} {s.Familiya} | {s.OrtachaBaho():F1}");
        }

        static void TalabaQidirish()
        {
            Console.Write("Ism yoki ID kiriting: ");
            var raw = Console.ReadLine() ?? string.Empty;
            var query = raw.Trim().ToLowerInvariant();

            var results = students.FindAll(s =>
                s.Ism.ToLowerInvariant().Contains(query) ||
                s.Familiya.ToLowerInvariant().Contains(query) ||
                s.Id.ToString() == query);

            if (results.Count == 0)
            {
                Console.WriteLine("Topilmadi.");
                return;
            }

            foreach (var s in results)
                Console.WriteLine($"[{s.Id}] {s.Ism} {s.Familiya} | {s.Guruh} | O'rtacha: {s.OrtachaBaho():F1}");
        }

        // Helper input methods
        static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();
                Console.WriteLine("Bo'sh bo'lishi mumkin emas. Qayta urinib ko'ring.");
            }
        }

        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;
                Console.WriteLine("Noto'g'ri format. Butun son kiriting.");
            }
        }

        static double ReadDoubleInRange(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                var raw = Console.ReadLine() ?? string.Empty;
                if (double.TryParse(raw, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double val) ||
                    double.TryParse(raw, out val))
                {
                    if (val >= min && val <= max)
                        return val;
                    Console.WriteLine($"Qiymat {min} va {max} orasida bo'lishi kerak.");
                }
                else
                {
                    Console.WriteLine("Noto'g'ri format. Raqam kiriting.");
                }
            }
        }
    }
}