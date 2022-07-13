using System;
using System.Collections.Generic;

namespace SpaceCards.UnitTests
{
    internal class CardGuessingStatisticsDataGenerator : BaseDataGenerator
    {
        public static IEnumerable<object[]> GenerateSetInvalidCardIdSuccessUserId(int testCount)
        {
            var rnd = new Random();

            for (int i = 0; i < testCount; i++)
            {
                var invalidCardId = -rnd.Next(0, int.MaxValue);
                var invalidSuccess = MakeInvalidSuccess();
                Guid? invalidUserId = null;

                yield return new object[]
                {
                    invalidCardId, invalidSuccess, invalidUserId
                };
            }
        }
    }
}
