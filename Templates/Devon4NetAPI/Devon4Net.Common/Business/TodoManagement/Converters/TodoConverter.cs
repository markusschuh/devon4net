using Devon4Net.Common.Business.TodoManagement.Dto;
using Devon4Net.Common.Domain.Entities;

namespace Devon4Net.Common.Business.TodoManagement.Converters
{
    public static class TodoConverter
    {
        public static TodoDto ModelToDto(Todos item)
        {
            if (item == null) return new TodoDto();

            return new TodoDto
            {
                Id = item.Id,
                Description = item.Description
            };
        }

    }
}
