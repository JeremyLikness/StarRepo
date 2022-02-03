using System.Linq.Expressions;

namespace StarRepo.Domain
{
    /// <summary>
    /// A telescope or camera lens.
    /// </summary>
    public class Telescope
    {
        private int _focalLength, _aperture;
        private double _fstop;

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        public string? Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public string? Model { get; set; }

        /// <summary>
        /// Gets or sets the focal length in millimeters.
        /// </summary>
        /// <remarks>
        /// This method has side effects and will trigger a computation of the f-stop.
        /// </remarks>
        public int FocalLengthMM
        {
            get => _focalLength;
            set 
            {
                _focalLength = value;
                Compute(()=>FocalLengthMM);
            }            
        }

        /// <summary>
        /// Gets or sets the aperture diameter in millimeters.
        /// </summary>
        /// <remarks>
        /// This method has side effects and will trigger a computation of the f-stop.
        /// </remarks>
        public int ApertureMM
        {
            get => _aperture;
            set
            {
                _aperture = value;
                Compute(()=>ApertureMM);
            }
        }

        /// <summary>
        /// Gets or sets the f-stop value.
        /// </summary>
        /// <remarks>
        /// This method has side effects and will trigger a computation of the aperture size.
        /// </remarks>
        public double FStop
        {
            get => _fstop;
            set
            {
                _fstop = value;
                Compute(() => FStop);
            }
        }

        /// <summary>
        /// Computes the aperture or f-stop based on mutations to other parameters.
        /// </summary>
        /// <param name="accessor">An expression that resolves to the property that was mutated.</param>
        /// <exception cref="ArgumentException">Thrown when the expression doesn't resolve to focal length,
        /// aperture, or f-stop.</exception>
        private void Compute(Expression<Func<object>> accessor)
        {
            var member = ((MemberExpression)((UnaryExpression)accessor.Body).Operand).Member.Name;
            switch (member)
            {
                case nameof(FocalLengthMM):                    
                case nameof(ApertureMM):
                    if (_aperture > 0)
                    {
                        _fstop = _focalLength / _aperture;
                    }
                    break;
                case nameof(FStop):
                    if (_focalLength > 0)
                    {
                        _aperture = (int)Math.Ceiling(_focalLength / _fstop);
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid accessor expression", nameof(accessor));
            }
        }
    }
}
