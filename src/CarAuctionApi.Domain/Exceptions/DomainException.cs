namespace CarAuctionApi.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message) { }
    }

    public class VehicleAlreadyExistsException : DomainException
    {
        public VehicleAlreadyExistsException(string vehicleId) 
            : base($"Vehicle with ID '{vehicleId}' already exists in the inventory.") { }
    }

    public class VehicleNotFoundException : DomainException
    {
        public VehicleNotFoundException(string vehicleId) 
            : base($"Vehicle with ID '{vehicleId}' was not found.") { }
    }

    public class AuctionAlreadyActiveException : DomainException
    {
        public AuctionAlreadyActiveException(string vehicleId) 
            : base($"Vehicle with ID '{vehicleId}' already has an active auction.") { }
    }

    public class AuctionNotFoundException : DomainException
    {
        public AuctionNotFoundException(string vehicleId) 
            : base($"No active auction found for vehicle with ID '{vehicleId}'.") { }
    }

    public class InvalidBidException : DomainException
    {
        public InvalidBidException(string message) : base(message) { }
    }
}
