using System;

namespace TodoManager.Core
{
    public class Todo : Entity
    {
        public Todo(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }

        public string Name { get; private set; }

        public bool IsCompleted { get; private set; }

        public void Rename(string newName)
        {
            EnsureNotCompleted();

            Name = newName;
        }

        public void Complete()
        {
            EnsureNotCompleted();

            IsCompleted = true;
        }

        private void EnsureNotCompleted()
        {
            if (IsCompleted)
            {
                throw new ApplicationException($"Todo with Id = '{Id}' is completed");
            }
        }
    }
}
