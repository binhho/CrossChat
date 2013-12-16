using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Abo.Server.Domain.Exceptions;
using Abo.Server.Domain.Seedwork;

namespace Abo.Server.Domain.Entities
{
    public class User : Entity
    {
        public User() { }

        public User(string name, string password, string huid, bool sex, int age, string pushUri, string country)
        {
            Name = name;
            Password = password;
            Huid = huid;
            Sex = sex;
            Age = age;
            PushUri = pushUri;
            Country = country;

            PersonalBlackList = new Collection<User>();
            Friends = new Collection<User>();

            ValidateRegistration();
        }

        public string Name { get; private set; }

        public string Huid { get; private set; }

        public string Password { get; private set; }

        public string Comments { get; private set; }

        public bool Sex { get; private set; }

        public int Age { get; private set; }

        public DateTime RegisteredAt { get; private set; }

        public DateTime LastOnline { get; private set; }

        public bool IsBanned { get; private set; }

        public DateTime? BannedAt { get; private set; }

        public bool IsDevoiced { get; private set; }

        public DateTime? DevoicedAt { get; private set; }

        public string PushUri { get; private set; }

        public string Country { get; private set; }

        public bool IsDeleted { get; private set; }

        public UserRole Role { get; private set; }

        public int PhotoId { get; private set; }
        
        /// <summary>
        /// Each player is able to add anyone to his black list (to not seeing his messages, his pushes and etc)
        /// </summary>
        public virtual ICollection<User> PersonalBlackList { get; private set; }

        /// <summary>
        /// Personal list of friends, player can monitor their states and send push notifications
        /// </summary>
        public virtual ICollection<User> Friends { get; private set; }   

        //TODO: add\remove for this collections

        private void ValidateRegistration()
        {
            //validation
            if (string.IsNullOrEmpty(Name) || Name.Length > 100)
                throw new InvalidPlayerRegistrationDataException("Name is invalid");
            if (string.IsNullOrEmpty(Password) || Password.Length > 100)
                throw new InvalidPlayerRegistrationDataException("Password is invalid");
            if (string.IsNullOrEmpty(Huid) || Huid.Length > 1000)
                throw new InvalidPlayerRegistrationDataException("Huid is invalid");
            if (Age < 13 || Age > 1000)
                throw new InvalidPlayerRegistrationDataException("Age is invalid");
            if (string.IsNullOrEmpty(Country) || Country.Length > 100)
                throw new InvalidPlayerRegistrationDataException("Contry is invalid");

            UpdateLastOnline();
            RegisteredAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            IsDeleted = true;
        }

        public void UpdateLastOnline()
        {
            LastOnline = DateTime.UtcNow;
        }

        public void ChangeCountry(string country)
        {
            Country = country;
        }

        public void ChangePushUri(string newPush)
        {
            PushUri = newPush;
        }

        public void ChangePhoto(int photoId)
        {
            PhotoId = photoId;
        }

        public void Ban(User bannedBy)
        {
            if (bannedBy.Role != UserRole.Moderator && bannedBy.Role != UserRole.Admin)
                throw new ModeratorsRightsRequiredException();

            if (Role == UserRole.Admin || Role == UserRole.Moderator)
                throw new ModeratorsRightsRequiredException();

            IsBanned = true;
            BannedAt = DateTime.UtcNow;
        }

        public void UnBan(User actor)
        {
            if (actor.Role != UserRole.Moderator && actor.Role != UserRole.Admin)
                throw new ModeratorsRightsRequiredException();

            if (Role == UserRole.Admin || Role == UserRole.Moderator)
                throw new ModeratorsRightsRequiredException();

            IsBanned = false;
        }

        /// <summary>
        /// Reset user's photo to default (in case if it's some kind of pornography ;)
        /// </summary>
        /// <param name="resetAuthor"></param>
        public int ResetPhoto(User resetAuthor)
        {
            if (resetAuthor.Role != UserRole.Moderator && resetAuthor.Role != UserRole.Admin)
                throw new ModeratorsRightsRequiredException();

            return PhotoId = -1;
        }
        
        public void Devoice(User devoicedBy)
        {
            if (devoicedBy.Role != UserRole.Moderator && devoicedBy.Role != UserRole.Admin)
                throw new ModeratorsRightsRequiredException();

            if (Role == UserRole.Admin || Role == UserRole.Moderator)
                throw new ModeratorsRightsRequiredException();

            IsDevoiced = true;
            DevoicedAt = DateTime.UtcNow;
        }

        public void BringVoiceBack(User actor)
        {
            if (actor.Role != UserRole.Moderator && actor.Role != UserRole.Admin)
                throw new ModeratorsRightsRequiredException();

            if (Role == UserRole.Admin || Role == UserRole.Moderator)
                throw new ModeratorsRightsRequiredException();

            IsDevoiced = false;
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName) || newName.Length > 100)
                throw new InvalidPlayerRegistrationDataException("New name is invalid");

            Name = newName;
        }

        public void ChangeComments(string comment)
        {
            Comments = comment;
        }

        public void ChangePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length > 100)
                throw new InvalidPlayerRegistrationDataException("New password is invalid");

            Password = password;
        }

        public void ChangeSex(bool newSex)
        {
            Sex = newSex;
        }

        public void ChangeAge(int age)
        {
            if (age < 13 && age > 1000)
                throw new ArgumentException("New age is invalid");

            Age = age;
        }

        public void GrantModeratorship()
        {
            Role = UserRole.Moderator;
        }

        public void RemoveModeratorship()
        {
            Role = UserRole.Player;
        }

        public void ChangeHuid(string huid)
        {
            Huid = huid;
        }
    }
}
