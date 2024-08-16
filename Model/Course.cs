
using Backend.Modals;
public class Course
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public int department_id { get; set; }
    public required Departments Departments { get; set; }
}
