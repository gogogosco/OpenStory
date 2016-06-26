using System;
using System.Collections.Generic;

namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Represents an abstract player group.
    /// </summary>
    /// <typeparam name="TGroupMember">The type that will be used for this group's members.</typeparam>
    /// <typeparam name="TUpdateInfo">The type that will be used for notification information.</typeparam>
    public abstract class PlayerGroup<TGroupMember, TUpdateInfo> : IPlayerGroup<TUpdateInfo>
        where TGroupMember : IEquatable<TGroupMember>
    {
        /// <inheritdoc />
        public int Id { get; }

        /// <summary>
        /// Gets a HashSet of the members of this player group.
        /// </summary>
        protected HashSet<TGroupMember> Members { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerGroup{TGroupMember,TUpdateInfo}"/> class with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier for the new group.</param>
        protected PlayerGroup(int id)
        {
            this.Id = id;
            this.Members = new HashSet<TGroupMember>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerGroup{TGroupMember,TUpdateInfo}"/> class  with the specified identifier and members.
        /// </summary>
        /// <param name="id">The identifier for the new group.</param>
        /// <param name="members">The initial member list of this group.</param>
        protected PlayerGroup(int id, IEnumerable<TGroupMember> members)
        {
            this.Id = id;
            this.Members = new HashSet<TGroupMember>(members);
        }

        /// <summary>
        /// Adds a new member to the instance.
        /// </summary>
        /// <param name="member">The new member to add.</param>
        /// <returns><see langword="true"/> if the member was added successfully; otherwise, <see langword="false"/>.</returns>
        public bool AddMember(TGroupMember member)
        {
            var success = this.Members.Add(member);
            if (success)
            {
                this.OnMemberAdded(member);
            }

            return success;
        }

        /// <summary>
        /// Called when a member is added to the group.
        /// </summary>
        /// <param name="member">The member that was added.</param>
        protected virtual void OnMemberAdded(TGroupMember member)
        {
        }

        /// <summary>
        /// Removes a member from the instance.
        /// </summary>
        /// <param name="member">The member to remove.</param>
        /// <returns><see langword="true"/> if the member was successfully found and removed from the group; otherwise, <see langword="false"/>.</returns>
        public bool RemoveMember(TGroupMember member)
        {
            var success = this.Members.Remove(member);
            if (success)
            {
                this.OnMemberRemoved(member);
            }

            return success;
        }

        /// <summary>
        /// Called when a member is removed from the group.
        /// </summary>
        /// <param name="member">The member that was removed.</param>
        protected virtual void OnMemberRemoved(TGroupMember member)
        {
        }

        /// <inheritdoc />
        public abstract void Update(TUpdateInfo updateInfo);
    }
}
