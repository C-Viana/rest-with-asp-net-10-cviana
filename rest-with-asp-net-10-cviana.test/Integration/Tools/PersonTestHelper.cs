using Bogus;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using System;
using System.Collections.Generic;
using System.Text;
using static Bogus.DataSets.Name;

namespace rest_with_asp_net_10_cviana.test.Integration.Tools
{
    public static class PersonTestHelper
    {
        public static PersonDTO CreateRandomPerson()
        {
            Randomizer.Seed = new Random(8675309);
            Gender _gender = (new Random().Next() % 2 == 0) ? Gender.Male : Gender.Female;

            var generatedPerson = new Faker<PersonDTO>()
                .RuleFor(u => u.Gender, (_gender == Gender.Male) ? "masculino" : "feminino")
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(_gender))
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(_gender))
                .RuleFor(u => u.Address, (f, u) => f.Address.FullAddress())
                .RuleFor(u => u.Enabled, (f, u) => true)
                ;
            return generatedPerson;
        }
    }
}
