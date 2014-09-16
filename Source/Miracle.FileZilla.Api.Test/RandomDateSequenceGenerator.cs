using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Miracle.FileZilla.Api.Test
{
    /// <summary>
    /// Creates random <see cref="DateTime"/> with Date only specimens.
    /// </summary>
    /// <remarks>
    /// The generated <see cref="DateTime"/> values will be within
    /// a range of ± two years from today's date,
    /// unless a different range has been specified in the constructor.
    /// </remarks>
    public class RandomDateSequenceGenerator : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator randomizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Ploeh.AutoFixture.RandomDateTimeSequenceGenerator"/> class.
        /// </summary>
        public RandomDateSequenceGenerator()
            : this(DateTime.Today.AddYears(-2), DateTime.Today.AddYears(2))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ploeh.AutoFixture.RandomDateTimeSequenceGenerator"/> class
        /// for a specific range of dates.
        /// </summary>
        /// <param name="minDate">The lower bound of the date range.</param>
        /// <param name="maxDate">The uppder bound of the date range.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="minDate"/> is greater than <paramref name="maxDate"/>.
        /// </exception>
        public RandomDateSequenceGenerator(DateTime minDate, DateTime maxDate)
        {
            if (minDate >= maxDate)
            {
                throw new ArgumentException("The 'minDate' argument must be less than the 'maxDate'.");
            }

            this.randomizer = new RandomNumericSequenceGenerator(minDate.Ticks, maxDate.Ticks);
        }

        /// <summary>
        /// Creates a new <see cref="DateTime"/> specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// A new <see cref="DateTime"/> specimen, if <paramref name="request"/> is a request for a
        /// <see cref="DateTime"/> value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return IsNotDateTimeRequest(request)
                       ? new NoSpecimen(request)
                       : this.CreateRandomDate(context);
        }

        private static bool IsNotDateTimeRequest(object request)
        {
            return !typeof(DateTime).IsAssignableFrom(request as Type);
        }

        private object CreateRandomDate(ISpecimenContext context)
        {
            return new DateTime(this.GetRandomNumberOfTicks(context)).Date;
        }

        private long GetRandomNumberOfTicks(ISpecimenContext context)
        {
            return (long)this.randomizer.Create(typeof(long), context);
        }
    }
}
