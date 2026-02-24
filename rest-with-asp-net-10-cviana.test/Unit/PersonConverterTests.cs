using FluentAssertions;
using rest_with_asp_net_10_cviana.Data.Converter.Impl;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Models;

namespace rest_with_asp_net_10_cviana.test.Unit
{
    public class PersonConverterTests
    {
        private readonly PersonConverter _converter;

        public PersonConverterTests()
        {
            _converter = new PersonConverter(); 
        }

        [Fact(DisplayName = "Should convert a DTO of Person to entity successfully")]
        public void Parse_ShouldConvertPersonDtoToPerson()
        {
            //Arrange block
            var MockPersonDTO = new PersonDTO
            {
                Id = 1,
                FirstName = "Augustus",
                LastName = "Windham",
                Gender = "masculino",
                Address = "Av. Teste Unitario, 99, Centro, Qualidade/QA - Brasil",
                Enabled = true
            };

            var ExpectedPersonResult = new Person
            (
                1,
                "Augustus",
                "Windham",
                "masculino",
                "Av. Teste Unitario, 99, Centro, Qualidade/QA - Brasil",
                true
            );

            //Act block
            Person ConvertedPerson = _converter.Parse(MockPersonDTO);

            //Assert block
            ConvertedPerson.Should().NotBeNull();
            ConvertedPerson.Should().BeEquivalentTo(ExpectedPersonResult);
        }

        [Fact(DisplayName = "Should result NULL when converting a null DTO ")]
        public void Parse_NullPersonDtoShouldReturnNull()
        {
            PersonDTO? MockPersonDTO = null;
            Person ConvertedPerson = _converter.Parse(MockPersonDTO);
            ConvertedPerson.Should().BeNull();
        }

        [Fact(DisplayName = "Should convert an entity of Person to DTO successfully")]
        public void Parse_ShouldConvertPersonToPersonDto()
        {
            var MockPersonEntity = new Person
            (
                1,
                "Augustus",
                "Windham",
                "masculino",
                "Av. Teste Unitario, 99, Centro, Qualidade/QA - Brasil",
                true
            );

            var ExpectedDtoResult = new PersonDTO
            {
                Id = 1,
                FirstName = "Augustus",
                LastName = "Windham",
                Gender = "masculino",
                Address = "Av. Teste Unitario, 99, Centro, Qualidade/QA - Brasil",
                Enabled = true
            };

            PersonDTO ConvertedPerson = _converter.Parse(MockPersonEntity);

            ConvertedPerson.Should().NotBeNull();
            ConvertedPerson.Should().BeEquivalentTo(ExpectedDtoResult);
        }

        [Fact(DisplayName = "Should result NULL when converting a null Entity ")]
        public void Parse_NullPersonShouldReturnNull()
        {
            Person? MockPersonDTO = null;
            PersonDTO ConvertedPerson = _converter.Parse(MockPersonDTO);
            ConvertedPerson.Should().BeNull();
        }

        [Fact(DisplayName = "Should convert a DTO list of Person to an entity list successfully")]
        public void Parse_ShouldConvertListPersonDtoToPerson()
        {
            List<PersonDTO> MockListPersonDto = [
                new PersonDTO
                {
                    Id = 1,
                    FirstName = "Augustus",
                    LastName = "Windham",
                    Gender = "masculino",
                    Address = "Av. Teste Unitario, 99, Centro, Qualidade/QA - Brasil",
                    Enabled = true
                },
                new PersonDTO
                {
                    Id = 2,
                    FirstName = "Melina",
                    LastName = "Lindsfare",
                    Gender = "feminino",
                    Address = "Av. Teste Unitario, 33, Unidade, Qualidade/QA - Brasil",
                    Enabled = true
                }
            ];

            List<Person> ExpectedPersonList =
            [
                new
                (
                    1,
                    "Augustus",
                    "Windham",
                    "masculino",
                    "Av. Teste Unitario, 99, Centro, Qualidade/QA - Brasil",
                    true
                ),
                new
                (
                    2,
                    "Melina",
                    "Lindsfare",
                    "feminino",
                    "Av. Teste Unitario, 33, Unidade, Qualidade/QA - Brasil",
                    true
                )
            ];

            List<Person> ConvertedPersonList = _converter.ParseList(MockListPersonDto);

            ConvertedPersonList.Should().NotBeNull();
            ConvertedPersonList.Should().BeEquivalentTo(ExpectedPersonList);
        }
    }
}
