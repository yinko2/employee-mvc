namespace EmployeeMVC.Models
{
    public partial class Eventlog
    {
        public int Id { get; set; }
        public EventLogType LogType { get; set; }
        public DateTime? LogDateTime { get; set; }
        public string? Source { get; set; }
        public string? FormName { get; set; }
        public string? LogMessage { get; set; }
        public string? ErrorMessage { get; set; }
        public int? UserId { get; set; }
    }
    public enum EventLogType
    {
        Info = 1,
        Error = 2,
        Warning = 3,
        Insert = 4,
        Update = 5,
        Delete = 6
    }
}
