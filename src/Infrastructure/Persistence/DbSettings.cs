namespace Infrastructure.Persistence
{
    class DbSettings
    {
        public string? DefaultConnection { get; set; }
        public Dictionary<string, string>? Users { get; set; }
    }
}
