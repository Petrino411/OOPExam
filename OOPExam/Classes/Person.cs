namespace OOPExam.Classes;

public abstract class Person : IIdentifiable
{
    public Guid Id { get; set; }
    public string Fio { get; set; }
}