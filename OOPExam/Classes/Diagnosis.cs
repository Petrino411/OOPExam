namespace OOPExam.Classes;

public class Diagnosis : IIdentifiable
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Treatment { get; set; }
}