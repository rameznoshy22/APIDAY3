namespace day1lab.DTO
{
    public class DeptoneDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public string location { get; set; }

        public List<string> StudentsName { get; set; } = new List<string>();
        public int numberOfStudents => StudentsName.Count;
    }
}
