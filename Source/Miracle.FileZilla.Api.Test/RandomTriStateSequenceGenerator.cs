using System;
using Miracle.FileZilla.Api.Elements;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Miracle.FileZilla.Api.Test
{
    /// <summary>
    /// Creates random <see cref="TriState"/> with TriState yes/no only specimens.
    /// </summary>
    public class RandomTriStateSequenceGenerator : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator randomizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Miracle.FileZilla.Api.Test.RandomTriStateSequenceGenerator"/> class.
        /// </summary>
        public RandomTriStateSequenceGenerator()
            : this(TriState.No, TriState.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Miracle.FileZilla.Api.Test.RandomTriStateSequenceGenerator"/> class
        /// for a specific range of TriStates.
        /// </summary>
        /// <param name="minTriState">The lower bound of the TriState range.</param>
        /// <param name="maxTriState">The uppder bound of the TriState range.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="minTriState"/> is greater than <paramref name="maxTriState"/>.
        /// </exception>
        public RandomTriStateSequenceGenerator(TriState minTriState, TriState maxTriState)
        {
            if (minTriState >= maxTriState)
            {
                throw new ArgumentException("The 'minTriState' argument must be less than the 'maxTriState'.");
            }

            this.randomizer = new RandomNumericSequenceGenerator((byte)minTriState, (byte)maxTriState);
        }

        /// <summary>
        /// Creates a new <see cref="TriState"/> specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">Not used.</param>
        /// <returns>
        /// A new <see cref="TriState"/> specimen, if <paramref name="request"/> is a request for a
        /// <see cref="TriState"/> value; otherwise, a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return IsTriStateRequest(request)
                ? this.CreateRandomTriState(context)
                : new NoSpecimen(request);
        }

        private static bool IsTriStateRequest(object request)
        {
            var seededReq = request as SeededRequest;
            return seededReq != null && typeof(TriState).IsAssignableFrom(seededReq.Request as Type);
        }

        private object CreateRandomTriState(ISpecimenContext context)
        {
            return (TriState)this.randomizer.Create(typeof(byte), context);
        }
    }
}