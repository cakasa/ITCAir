namespace ITCAir.Data.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PassengerId { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }
        public bool isBusinessClass { get; set; }
        public int FlightId { get; set; }
        public virtual Flight Flight { get; set; }
    }
}