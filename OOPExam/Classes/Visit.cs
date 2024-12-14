namespace OOPExam.Classes;

public class Visit: IIdentifiable
{
    public Guid Id { get; set; }
    
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; }

    public DateTime VisitDate { get; set; }

    public Guid DiagnosisId { get; set; }
    public Diagnosis Diagnosis { get; set; }
}