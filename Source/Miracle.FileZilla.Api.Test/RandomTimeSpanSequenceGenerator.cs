using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Miracle.FileZilla.Api.Test
{
    /// <summary>
    /// Creates random <see cref="TimeSpan"/> with TimeSpan only specimens.
    /// </summary>
    /// <remarks>
    /// The generated <see cref="TimeSpan"/> values will be within
    /// a range of ± two years from today's TimeSpan,
    /// unless a different range has been specified in the constructor.
    /// </remarks>
    public class RandomTimeSpanSequenceGenerator : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator randomizer;
        private readonly bool ignoreFractions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Miracle.FileZilla.Api.Test.RandomTimeSpanSequenceGenerator"/> class.
        /// </summary>
        public RandomTimeSpanSequenceGenerator()
            : this(new TimeSpan(0,0,0), new TimeSpan(23,59,59), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Miracle.FileZilla.Api.Test.RandomTimeSpanSequenceGenerator"/> class
        /// for a specific range of TimeSpans.
        /// </summary>
        /// <param name="minTimeSpan">The lower bound of the TimeSpan range.</param>
        /// <param name="maxTimeSpan">The uppder bound of the TimeSpan range.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="minTimeSpan"/> is greater than <paramref name="maxTimeSpan"/>.
        /// </exception>
        public RandomTimeSpanSequenceGenerator(TimeSpan minTimeSpan, TimeSpan maxTimeSpan, bool ignoreFractions)
        {
            this.ignoreFractions = ignoreFractions;
            if (minTimeSpan >= maxTimeSpan)
            {
                throw new ArgumentException("The 'minTimeSpan' argument must be less than the 'maxTimeSpan'.");
            }

            this.randomizer = new RandomNumericSequenceGenerator(minTimeSpan.Ticks, maxTimeSpan.Ticks);
        }

        /// <summary>
        /// Creates a new <see cref="TimeSpan"/> specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// A new <see cref="TimeSpan"/> specimen, if <paramref name="request"/> is a request for a
        /// <see cref="TimeSpan"/> value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return IsNotTimeSpanRequest(request)
                ? new NoSpecimen(request)
                : this.CreateRandomTimeSpan(context);
        }

        private static bool IsNotTimeSpanRequest(object request)
        {
            return !typeof(TimeSpan).IsAssignableFrom(request as Type);
        }

        private object CreateRandomTimeSpan(ISpecimenContext context)
        {
            return new TimeSpan(this.GetRandomNumberOfTicks(context));
        }

        private long GetRandomNumberOfTicks(ISpecimenContext context)
        {
            var randomNumberOfTicks = (long)this.randomizer.Create(typeof(long), context);
            if (ignoreFractions)
                randomNumberOfTicks = randomNumberOfTicks - (randomNumberOfTicks % TimeSpan.TicksPerSecond);
            return randomNumberOfTicks;
        }
    }



    //internal class StatusGenerator : ISpecimenBuilder
    //{
    //    private readonly Status[] values;
    //    private int i;

    //    internal StatusGenerator()
    //    {
    //        this.values =
    //            Enum.GetValues(typeof(Status)).Cast<Status>().ToArray();
    //    }

    //    public object Create(object request, ISpecimenContext context)
    //    {
    //        var pi = request as ParameterInfo;
    //        if (pi == null || !pi.ParameterType.IsEnum)
    //            return new NoSpecimen(request);

    //        return this.values[i == this.values.Length - 1 ? i = 0 : ++i];
    //    }
    //}
}