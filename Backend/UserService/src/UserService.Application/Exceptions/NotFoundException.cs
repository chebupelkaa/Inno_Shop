namespace UserService.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Type type, object key)
            : base($"Entity {type.Name} with key {key} was not found") { }

        public NotFoundException(Type type)
            : base($"{type.Name} does not exist") { }
    }
}
