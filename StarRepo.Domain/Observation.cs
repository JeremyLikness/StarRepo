namespace StarRepo.Domain
{
    /// <summary>
    /// An observation of a target with a telescope.
    /// </summary>
    public class Observation
    {
        /// <summary>
        /// Unique identifier of the observation.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The time of the observation.
        /// </summary>
        public DateTimeOffset ObservationDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        public string? Extension { get; set; }

        /// <summary>
        /// Gets or sets the file unique identifier.
        /// </summary>
        public Guid FileId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the telescope used for the observation.
        /// </summary>
        public Telescope? Telescope { get; set; }

        /// <summary>
        /// Gets or sets the target of the observation.
        /// </summary>
        public Target? Target { get; set; }
    }
}
