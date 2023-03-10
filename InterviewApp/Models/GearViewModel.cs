namespace InterviewApp.Models
{
    public class GearViewModel
    {
        public string SerialNumber { get; set; }
        public int GearTypeId { get; set; }
        public string GearTypeName{ get; set; }
        public string EmailClient { get; set; }
        public string TechnicienUserName { get; set; }
        public int DurationPerMin { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }

    }
    public class GearType
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }
}
