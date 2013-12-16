using Abo.Server.Application.DataTransferObjects.Enums;

namespace Abo.Server.Application.DataTransferObjects.Messages
{
    public class UserDto : BaseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Sex { get; set; }

        public int Age { get; set; }

        public int GamesCount { get; set; }

        public int VictoriesCount { get; set; }

        public int Xp { get; set; }

        public bool IsDevoiced { get; set; }

        public bool IsBanned { get; set; }

        public string Country { get; set; }

        public UserRoleEnum Role { get; set; }

        public int PhotoId { get; set; }
    }
}
