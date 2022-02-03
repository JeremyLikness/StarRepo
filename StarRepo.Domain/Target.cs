namespace StarRepo.Domain
{
    /// <summary>
    /// An astronomical target.
    /// </summary>
    public class Target
    {
        /// <summary>
        /// Gets or sets the unique identifer for the target.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the target.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string? Description { get; set; }        
    }
}