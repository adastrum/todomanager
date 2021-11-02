using AutoMapper;

namespace TodoManager.Api.Models
{
    public record Todo(string Id, string Name, bool IsCompleted);

    public class TodoMappingProfile : Profile
    {
        public TodoMappingProfile()
        {
            CreateMap<Core.Todo, Todo>().ReverseMap();
        }
    }
}
