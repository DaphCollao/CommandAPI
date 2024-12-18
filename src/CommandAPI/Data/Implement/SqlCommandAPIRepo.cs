using CommandAPI.Models;

namespace CommandAPI.Data
{
    public class SqlCommandAPIRepo : ICommandAPIRepo
    {
        private readonly CommandContext _context;

        public SqlCommandAPIRepo(CommandContext context)
        {
            _context = context;
        }

        public void CreateCommand(Command cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            _context.CommandItems.Add(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            _context.CommandItems.Remove(cmd);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _context.CommandItems.ToList();
        }

        public Command GetCommandById(int id)
        {
            var command = _context.CommandItems.FirstOrDefault(w => w.Id == id);

            if (command == null) throw new KeyNotFoundException($"Command with ID {id} not found.");

            return command;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public void UpdateCommand(Command cmd)
        {
            //No implementation needed
        }
    }
}