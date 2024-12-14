using Microsoft.EntityFrameworkCore;
using OOPExam;
using OOPExam.Classes;

class Program
{
    private static DirectoryInfo workDir = new DirectoryInfo(Environment.CurrentDirectory + "\\Output");

    static void Main(string[] args)
    {
        workDir.Create();
        var db = new AppDbContext();

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        AddTestData(db);
        
        Task1(db);
        Task2(db);
    }

    private static void Task1(AppDbContext db)
    {
        var filePath = workDir.FullName + "\\Task1_Output.txt";

        using (var writer = new StreamWriter(filePath, false))
        {
            var repeatedPatients = db.Visits
                .GroupBy(v => v.ClientId)
                .Where(g => g.Count() > 1)
                .Select(g => new
                {
                    ClientId = g.Key,
                    VisitCount = g.Count()
                })
                .ToList();

            foreach (var entry in repeatedPatients)
            {
                var client = db.Clients.FirstOrDefault(c => c.Id == entry.ClientId);

                var result = $"Пациент {client.Fio} был у врача {entry.VisitCount} раз(а).";
                Console.WriteLine(result);
                writer.WriteLine(result);
            }

            writer.WriteLine("\n--- Конец ---");
        }

        Console.WriteLine($"Пациенты посетившие врача более 1 раза записаны в файл: {filePath}");
    }

    private static void Task2(AppDbContext db)
    {
        var filePath = workDir.FullName + "\\Report.txt";

        using (var writer = new StreamWriter(filePath, false))
        {
            var totalVisits = db.Visits.Distinct().Count();
            var relativeWorkload = db.Visits
                .GroupBy(a => a.Doctor.Specialisation)
                .Select(g => new
                {
                    Specialization = g.Key,
                    TotalVisits = g.Count(),
                    RelativeLoad = g.Count() / (double)totalVisits
                })
                .ToList();

            foreach (var item in relativeWorkload)
            {
                var result = $"Специализация: {item.Specialization}\n" +
                             $"  Всего приемов: {item.TotalVisits}\n" +
                             $"  Относительная нагрузка: {item.RelativeLoad:F2} приемов на врача";

                Console.WriteLine(result);
                writer.WriteLine(result);
                writer.WriteLine();
            }

            writer.WriteLine("\n--- Конец отчета ---");
        }

        Console.WriteLine($"Результаты отчета записаны в файл: {filePath}");
    }


    private static void AddTestData(AppDbContext context)
    {
        var clients = new List<Client>
        {
            new Client { Fio = "Иванов Иван Иванович", Address = "г. Москва, ул. Ленина, 10" },
            new Client { Fio = "Петров Петр Петрович", Address = "г. Москва, ул. Гагарина, 5" },
            new Client { Fio = "Сидорова Мария Ивановна", Address = "г. Санкт-Петербург, Невский проспект, 20" },
        };

        var doctors = new List<Doctor>
        {
            new Doctor { Fio = "Смирнов Алексей Сергеевич", Specialisation = "Терапевт" },
            new Doctor { Fio = "Кузнецов Николай Павлович", Specialisation = "Хирург" },
            new Doctor { Fio = "Попова Наталья Викторовна", Specialisation = "Кардиолог" },
        };

        var diagnoses = new List<Diagnosis>
        {
            new Diagnosis { Name = "Грипп", Treatment = "Постельный режим, питье, противовирусные препараты" },
            new Diagnosis { Name = "Ангина", Treatment = "Антибиотики, полоскание горла" },
            new Diagnosis { Name = "Стенокардия", Treatment = "Срочная консультация кардиолога, препараты для сердца" },
        };

        context.Clients.AddRange(clients);
        context.Doctors.AddRange(doctors);
        context.Diagnoses.AddRange(diagnoses);
        context.SaveChanges();

        var visits = new List<Visit>
        {
            new Visit
            {
                Doctor = doctors[0], Client = clients[0], VisitDate = DateTime.Now.AddDays(-2), Diagnosis = diagnoses[0]
            },
            new Visit
            {
                Doctor = doctors[0], Client = clients[1], VisitDate = DateTime.Now.AddDays(-1), Diagnosis = diagnoses[0]
            },
            new Visit { Doctor = doctors[1], Client = clients[2], VisitDate = DateTime.Now, Diagnosis = diagnoses[1] },
            new Visit
            {
                Doctor = doctors[1], Client = clients[1], VisitDate = DateTime.Now.AddDays(-3), Diagnosis = diagnoses[1]
            },
            new Visit
            {
                Doctor = doctors[2], Client = clients[0], VisitDate = DateTime.Now.AddDays(-5), Diagnosis = diagnoses[2]
            }
        };

        context.Visits.AddRange(visits);
        context.SaveChanges();
    }
}