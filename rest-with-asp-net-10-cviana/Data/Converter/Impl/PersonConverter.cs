using rest_with_asp_net_10_cviana.Data.Converter.Contract;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Models;

namespace rest_with_asp_net_10_cviana.Data.Converter.Impl
{
    public class PersonConverter : IParser<Person, PersonDTO>, IParser<PersonDTO, Person>
    {
        private Person ConvertToEntity(PersonDTO dto)
        {
            return new Person(
                dto.Id,
                dto.FirstName,
                dto.LastName,
                dto.Gender,
                dto.Address,
                dto.Enabled
            );
        }

        private PersonDTO ConvertToDTO(Person entity)
        {
            return new PersonDTO
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                Address = entity.Address,
                Enabled = entity.Enabled
            };
        }

        public PersonDTO Parse(Person origin)
        {
            if (origin == null) return null;
            return ConvertToDTO(origin);
        }

        public Person Parse(PersonDTO origin)
        {
            if (origin == null) return null;
            return ConvertToEntity(origin);
        }

        public List<PersonDTO> ParseList(List<Person> origin)
        {
            if (origin == null) return null;
            return [.. origin.Select(ConvertToDTO)];
        }

        public List<Person> ParseList(List<PersonDTO> origin)
        {
            if (origin == null) return null;
            return [.. origin.Select(ConvertToEntity)];
        }
    }
}
