using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Miracle.FileZilla.Api.Test
{
    /// <summary>
    /// Creates random <see cref="Option"/> with ether NumericValue or TextValue set.
    /// </summary>
    public class RandomOptionGenerator : ISpecimenBuilder
    {
        private readonly RandomNumericSequenceGenerator _randomizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomOptionGenerator"/> class.
        /// </summary>
        public RandomOptionGenerator()
        {
            _randomizer = new RandomNumericSequenceGenerator((byte)OptionType.Text, (byte)OptionType.Numeric);
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

            return typeof(Option).IsAssignableFrom(request as Type) 
                ? CreateRandom(context) 
                : new NoSpecimen(request);
        }

        private object CreateRandom(ISpecimenContext context)
        {
            var option = new Option
            {
                OptionType = (OptionType) _randomizer.Create(typeof (byte), context),
            };

            switch (option.OptionType)
            {
                case OptionType.Text:
                    option.TextValue = context.Create<string>();
                    break;
                case OptionType.Numeric:
                    option.NumericValue = context.Create<long>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return option;
        }
    }
}